//CompilerOptions = InternalClass, ParentNamespace, NoPreShader, DefinePlatform, DisableGenerateExtensions



// ------------------------------------
//
// See Billboard.fx for details on how to implement a particle drawer shader
//
// ------------------------------------




float4x4 worldViewProj : WORLDVIEWPROJECTION;
float3 textureSizeOffset;
float2 velocityScale;

#ifndef XBOX360

//used for numbers of CPU particles
float4 positionData[80];
float4 velocityData[80];
float4 colourData[80];

#endif

texture2D PositionTexture;
sampler2D PositionSampler = sampler_state
{
	Texture = (PositionTexture);
	MagFilter = POINT;
	MinFilter = POINT;
	MipFilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};
texture2D ColourTexture;
sampler2D ColourSampler = sampler_state
{
	Texture = (ColourTexture);
	MagFilter = POINT;
	MinFilter = POINT;
	MipFilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};
texture2D VelocityTexture;
sampler2D VelocitySampler = sampler_state
{
	Texture = (VelocityTexture);
	MagFilter = POINT;
	MinFilter = POINT;
	MipFilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};

texture2D UserTexture;
sampler2D UserSampler = sampler_state
{
	Texture = (UserTexture);
	MagFilter = POINT;
	MinFilter = POINT;
	MipFilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};

texture2D DisplayTexture;
sampler2D DisplaySampler = sampler_state
{
	Texture = (DisplayTexture);
	MagFilter = LINEAR;
	MinFilter = LINEAR;
	MipFilter = POINT;
	AddressU = CLAMP;
	AddressV = CLAMP;
};

void ParticleVS_GpuTex(
						float4	position : POSITION,
					out float4	positionOut : POSITION,
					out float2	texCoordOut : TEXCOORD0,
					out float4	colourOut : TEXCOORD1,
				uniform bool	useColour,				// note the uniform constant passed in, indicates if colours are sampled
				uniform bool	useUserPositionOffset)
{
	position.x += textureSizeOffset.z;

	float index = position.x * textureSizeOffset.x;
	
	float2 samplePosition = float2(fmod(index,1),(floor(index)) * textureSizeOffset.y);
	samplePosition    += 0.5 * textureSizeOffset.xy;
	samplePosition.y  = 1 - samplePosition.y;
	
	float4 texCoord = float4(samplePosition,0,0);

	float4 positionSize = tex2Dlod(PositionSampler,texCoord);
	float4 velocityRotation = tex2Dlod(VelocitySampler,texCoord);
	
	if (useUserPositionOffset)
	{
		positionSize.xyz += tex2Dlod(UserSampler,texCoord).yzw;
	}
	
	positionOut = float4(0,0,0,1);
	positionOut.xy = positionSize.xy;
	
	float size = positionSize.w;
	
	colourOut = 1;
	if (useColour)
		colourOut = tex2Dlod(ColourSampler,texCoord);
		
	texCoordOut.xy = position.yz * 0.5 + 0.5;
		
		
	velocityRotation.y += 1 - any(velocityRotation.xy);
	
	float len = length(velocityRotation.xy);
	float2 yaxis = velocityRotation.xy / len;
	float2 xaxis = float2(yaxis.y,-yaxis.x);
	
	float scale = velocityScale.x + velocityRotation.w * velocityScale.y;
	
	yaxis *= (1 + len * scale);

	float2 offset = position.y * xaxis + position.z * yaxis;
	
	
	positionOut.xy = (positionOut.xy + offset * size);
	
	positionOut = mul(positionOut,worldViewProj);
}




#ifndef XBOX360


void ParticleVS_Cpu(
						float4 position : POSITION,
					out float4 positionOut : POSITION,
					out float2 texCoord : TEXCOORD0,
					out float4 colourBase : TEXCOORD1,
						uniform bool useColour)
{
	float2 corner = position.yz;
	
	float4 positionSize = positionData[position.x];
	float4 velocityRotation = velocityData[position.x];
	
	float size = positionSize.w;
	
	colourBase = 1;
	if (useColour)
		colourBase = (colourData[position.x]);

	position = float4(0,0,0,1);
	position.xy = positionSize.xy;
	
	texCoord.xy = corner * 0.5 + 0.5;
	
		
		
	velocityRotation.y += 1 - any(velocityRotation.xy);
	
	float len = length(velocityRotation.xy);
	float2 yaxis = velocityRotation.xy / len;
	float2 xaxis = float2(yaxis.y,-yaxis.x);
	
	float scale = velocityScale.x + velocityRotation.w * velocityScale.y;
	
	yaxis *= (1+len * scale);

	float2 offset = corner.x * xaxis + corner.y * yaxis;
	
	
	
	position.xy = (position.xy + offset * size);
	positionOut = position;
	
	positionOut = mul(positionOut,worldViewProj);
}

#endif

float4 ParticlePS(float2 texcoord : TEXCOORD0, float4 colour : TEXCOORD1) : COLOR0
{
	float4 output = tex2D(DisplaySampler,colour.xy);
	output *= saturate(colour.a);
	return output;
}




technique DrawVelocityParticles_GpuTex
{
   pass { VertexShader = compile vs_3_0 ParticleVS_GpuTex(false, false); PixelShader = compile ps_3_0 ParticlePS(); }
}

technique DrawVelocityParticlesColour_GpuTex
{
   pass { VertexShader = compile vs_3_0 ParticleVS_GpuTex(true, false); PixelShader = compile ps_3_0 ParticlePS(); }
}

//user offset variants

technique DrawVelocityParticles_GpuTex_UserOffset
{
   pass { VertexShader = compile vs_3_0 ParticleVS_GpuTex(false, true); PixelShader = compile ps_3_0 ParticlePS(); }
}

technique DrawVelocityParticlesColour_GpuTex_UserOffset
{
   pass { VertexShader = compile vs_3_0 ParticleVS_GpuTex(true, true); PixelShader = compile ps_3_0 ParticlePS(); }
}

#ifndef XBOX360




technique DrawVelocityParticles_BillboardCpu
{
   pass { VertexShader = compile vs_2_0 ParticleVS_Cpu(false); PixelShader = compile ps_2_0 ParticlePS(); }
}


technique DrawVelocityParticlesColour_BillboardCpu
{
   pass { VertexShader = compile vs_2_0 ParticleVS_Cpu(true); PixelShader = compile ps_2_0 ParticlePS(); }
}

#endif