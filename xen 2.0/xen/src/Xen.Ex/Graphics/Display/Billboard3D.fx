//CompilerOptions = InternalClass, ParentNamespace, DefinePlatform, DisableGenerateExtensions




// ------------------------------------
//
// See Billboard.fx for details on how to implement a particle drawer shader
//
// ------------------------------------





float4x4 worldViewProj : WORLDVIEWPROJECTION;
float4x4 worldMatrix : WORLD;
float3 viewPoint : VIEWPOINT;

float3 worldSpaceYAxis;

//stores the inverse texture size (XY) and the starting particle offset (Z)
float3 invTextureSizeOffset;


#ifndef XBOX360 // no cpu shaders on the xbox

//for large numbers of CPU particles
float4 positionData[75];
float4 velocityData[75];
float4 colourData[75];

#endif


//samplers for GPU textures
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

// texture used to display on the particle
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


// And now for the 3D versions...


//this isn't exactly cheap...
void ParticleVS_GpuTex3D(
						float4	position		: POSITION,
					out float4	positionOut		: POSITION,
					out float2	texCoordOut		: TEXCOORD0,
					out float4	colourOut		: TEXCOORD1,
				uniform bool	useColour,				// note the uniform constant passed in, indicates if colours are sampled
				uniform bool	useUserPositionOffset)
{
	//the vertex buffer stores the particle index in position.x,
	//offset it by the starting particle
	position.x			+= invTextureSizeOffset.z;

	//index of the particle in the texture, scaled by the inverse width of the texture (0-1 for a row of particles)
	float index			= position.x * invTextureSizeOffset.x;
	
	//values above 1 will be the next row of pixels (particles)
	
	//find the sampling position of the particle in the textures
	float2 samplePosition = float2(fmod(index,1),(floor(index)) * invTextureSizeOffset.y); // the fmod isn't really needed here
	
	//bias to get the centre of the texel (to prevent FP inaccuracy on the edge of the texel)
	samplePosition		+= 0.5 * invTextureSizeOffset.xy;
	
	//invert y
	samplePosition.y	= 1 - samplePosition.y;
	
	
	float4 texCoord		= float4(samplePosition,0,0);

	//sample the two vertex textures
	float4 positionSize = tex2Dlod(PositionSampler,texCoord);
	float4 velocityRotation = tex2Dlod(VelocitySampler,texCoord);
	
	if (useUserPositionOffset)
	{
		positionSize.xyz += tex2Dlod(UserSampler,texCoord).yzw;
	}
	
	positionOut = float4(0,0,0,1);
	positionOut.xyz = positionSize.xyz;
	
	float size = positionSize.w;
	
	colourOut = 1;
	if (useColour) // sample the particle colour ?
		colourOut = tex2Dlod(ColourSampler,texCoord);
	
	//output texture coordinate (position.yz stores values for the corners, [-1,1] range)
	texCoordOut.xy = position.yz * 0.5 + 0.5;
	
	//compute projection shape for the particle in 3D
	float3 worldPoint = mul(positionOut, worldMatrix).xyz - viewPoint.xyz;
	
	float3 y_axis = normalize(cross(worldPoint.xyz,worldSpaceYAxis.xyz));
	float3 x_axis = normalize(cross(worldPoint.xyz,y_axis.xyz));
	
		
	//apply the rotation of the particle (stored in the W velocity texture value)
	float cr,sr;
	sincos(velocityRotation.w, cr, sr);
	
	//find the corner position
	float3 rot = (x_axis * cr + y_axis * sr) * position.y;
	rot += (y_axis * cr - x_axis * sr) * position.z;
	
	//generate the complete vertex position
	positionOut.xyz += rot * size;
	
	//and multiply by the WVP matrix
	positionOut = mul(positionOut,worldViewProj);
}


#ifndef XBOX360


void ParticleVS_Cpu3D(
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

	positionOut = float4(0,0,0,1);
	positionOut.xyz = positionSize.xyz;
	
	texCoord.xy = corner * 0.5 + 0.5;
	
		
	//compute projection shape for the particle in 3D
	float3 worldPoint = mul(positionOut, worldMatrix).xyz - viewPoint.xyz;
	
	float3 y_axis = normalize(cross(worldPoint,worldSpaceYAxis));
	float3 x_axis = normalize(cross(worldPoint,y_axis));
	
		
	//apply the rotation of the particle (stored in the W velocity texture value)
	float cr,sr;
	sincos(velocityRotation.w, cr, sr);
	
	//find the corner position
	float3 rot = (x_axis * cr + y_axis * sr) * position.y;
	rot += (y_axis * cr - x_axis * sr) * position.z;
	
	//generate the complete vertex position
	positionOut.xyz += rot * size;
	
	//and multiply by the WVP matrix
	positionOut = mul(positionOut,worldViewProj);
}

#endif

//the pixel shader
float4 ParticlePS(float2 texcoord : TEXCOORD0, float4 colour : TEXCOORD1) : COLOR0
{
	float4 texSample = tex2D(DisplaySampler,texcoord.xy);

	//colour is modulated by the texture
	colour.rgb *= texSample.rgb;
	
	//clamp alpha between 0 and 1
	colour.a = saturate(colour.a);
	
	//finally, perform the RGB modulation setp
	colour.rgb *= colour.a;

	colour.a *= texSample.a;
	
	return colour;
}


technique DrawBillboardParticles_GpuTex3D
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex3D(false, false); 
		PixelShader		= compile ps_3_0 ParticlePS(); 
	}
}

technique DrawBillboardParticlesColour_GpuTex3D
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex3D(true, false);
		PixelShader		= compile ps_3_0 ParticlePS();
	}
}

//user offset variants
technique DrawBillboardParticles_GpuTex3D_UserOffset
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex3D(false, true); 
		PixelShader		= compile ps_3_0 ParticlePS(); 
	}
}

technique DrawBillboardParticlesColour_GpuTex3D_UserOffset
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex3D(true, true);
		PixelShader		= compile ps_3_0 ParticlePS();
	}
}


#ifndef XBOX360

//here are the CPU techniques

technique DrawBillboardParticles_BillboardCpu3D
{
	pass 
	{ 
		VertexShader	= compile vs_2_0 ParticleVS_Cpu3D(false); 
		PixelShader		= compile ps_2_0 ParticlePS(); 
	}
}


technique DrawBillboardParticlesColour_BillboardCpu3D
{
	pass 
	{ 
		VertexShader	= compile vs_2_0 ParticleVS_Cpu3D(true); 
		PixelShader		= compile ps_2_0 ParticlePS(); 
	}
}

#endif