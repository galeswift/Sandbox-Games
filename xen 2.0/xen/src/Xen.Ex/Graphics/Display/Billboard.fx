//CompilerOptions = InternalClass, ParentNamespace, DefinePlatform, DisableGenerateExtensions



//
//
// This shader is used to display billboard particles in 2D.
// This shader implements GPU and CPU particle drawing shaders.
// The CPU shaders are ommitted on XBOX builds
// 
// ----------------
// Any particle shader must note the following:
// ----------------
// 
// A particle system may not store colour or user value textures / arrays.
// This is done when the values are not accessed by the particle system.
// If colours are not stored, the colour of each particle is assumed to be white (RGBA=1)
// 
// Most shaders should implement a 'no colour' variation for each technique.
// 
// With GPU particles, it is possible for particles to be drawn in multiple
// draw calls. Because of this, a starting value should be passed in
// 
// Pixel shaders should multiply the returned RGB colour value with the alpha value .
// (All particle systems assume RGB alpha modulation by the shader)
// 
// GPU particle systems must be shader model 3 to support vertex texture fetch, however 
// CPU particle systems must be shader model 2 for backwrads compatibility.
//
//
// Some GPU particle systems request a special mode, where the position is offset by
// the User.yzw (user1,user2,user3) value. This is done to reduces problems with FP16
// 


float4x4 worldViewProj : WORLDVIEWPROJECTION;

//stores the inverse texture size (XY) and the starting particle offset (Z)
float3 invTextureSizeOffset;


#ifndef XBOX360 // no cpu shaders on the xbox

//for large numbers of particles
float4 positionData[120];
float4 colourData[120];

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

//vertex shader for gpu textures
void ParticleVS_GpuTex(
						float4	position		: POSITION,
					out float4	positionOut		: POSITION,
					out float2	texCoordOut		: TEXCOORD0,
					out float4	colourOut		: TEXCOORD1,
				uniform bool	useColour,				// note the uniform constant passed in, indicates if colours are sampled
				uniform bool	useUserPositionOffset)	// or if the position is offset by [user1,user2,user3]
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
	
	if (useUserPositionOffset)
	{
		positionSize.xyz += tex2Dlod(UserSampler,texCoord).yzw;
	}
	
	float4 velocityRotation = tex2Dlod(VelocitySampler,texCoord);
	
	positionOut = float4(0,0,0,1);
	positionOut.xy = positionSize.xy;
	
	float size = positionSize.w;
	
	colourOut = 1;
	if (useColour) // sample the particle colour ?
		colourOut = tex2Dlod(ColourSampler,texCoord);
	
	//output texture coordinate (position.yz stores values for the corners, [-1,1] range)
	texCoordOut.xy = position.yz * 0.5 + 0.5;
		
	//apply the rotation of the particle (stored in the W velocity texture value)
	float cr,sr;
	sincos(velocityRotation.w, cr, sr);
	
	//find the corner position
	float2 rot;
	rot.x = sr * position.y + cr * position.z;
	rot.y = sr * position.z - cr * position.y;
	
	//generate the complete vertex position
	positionOut.xy = (positionOut.xy + rot * size);
	
	//and multiply by the WVP matrix
	positionOut = mul(positionOut,worldViewProj);
}




#ifndef XBOX360


void ParticleVS_Cpu(
						float4	position		: POSITION,
					out float4	positionOut		: POSITION,
					out float2	texCoord		: TEXCOORD0,
					out float4	colourBase		: TEXCOORD1,
				uniform bool	useColour)
{
/*	//this works in a similar way to the GPU shader, except shader constants are sampled
	
	//sample the position
	
	//in this case, the drawer class preprocesses the positions,
	//the z value is replaced with the rotation value, this cuts
	//down the number of shader constants required
	//the particle system is displayed in 2D, so Z isn't needed
	
	float4 positionRotationSize = positionData[position.x];
	
	colourBase = 1;
	if (useColour)
		colourBase		= (colourData[position.x]);

	position			= float4(0,0,0,1);
	position.xy			= positionRotationSize.xy;
	
	float size			= positionRotationSize.w;
	float rotation		= positionRotationSize.z;
	
	float2 corner		= position.yz;
	texCoord.xy			= corner * 0.5 + 0.5;
	
	
	float cr,sr;
	sincos(positionRotationSize.z, cr, sr);
	
	float2 rot;
	rot.x = cr * corner.x + sr * corner.y;
	rot.y = cr * corner.y - sr * corner.x;
	
	
	position.xy			= (position.xy + size * rot);
	positionOut			= position;
	
	positionOut			= mul(positionOut,worldViewProj);*/
	
	
	float2 corner = position.yz;
	
	float4 positionRotationSize = positionData[position.x];
	float4 velocityRotation = float4(0,0,0,0);
	
	float size = positionRotationSize.w;
	
	colourBase = 1;
	if (useColour)
		colourBase = (colourData[position.x]);

	position = float4(0,0,0,1);
	position.xy = positionRotationSize.xy;
	
	texCoord.xy = corner * 0.5 + 0.5;
	
	
		
	float cr,sr;
	sincos(positionRotationSize.z, cr, sr);
	
	float2 offset;
	offset.x = sr * corner.x + cr * corner.y;
	offset.y = sr * corner.y - cr * corner.x;
	
	
	position.xy = (position.xy + offset * size);
	positionOut = position;
	
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


//Here, the various techniques used are defined
//Note that some functions have a boolean passed in, this sets if colours are sampled

technique DrawBillboardParticles_GpuTex
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex(false, false); 
		PixelShader		= compile ps_3_0 ParticlePS(); 
	}
}


technique DrawBillboardParticlesColour_GpuTex
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex(true, false);
		PixelShader		= compile ps_3_0 ParticlePS();
	}
}

//user offset versions
technique DrawBillboardParticles_GpuTex_UserOffset
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex(false, true); 
		PixelShader		= compile ps_3_0 ParticlePS(); 
	}
}


technique DrawBillboardParticlesColour_GpuTex_UserOffset
{
	pass 
	{
		VertexShader	= compile vs_3_0 ParticleVS_GpuTex(true, true);
		PixelShader		= compile ps_3_0 ParticlePS();
	}
}

#ifndef XBOX360

//here are the CPU techniques

technique DrawBillboardParticles_BillboardCpu
{
	pass 
	{ 
		VertexShader	= compile vs_2_0 ParticleVS_Cpu(false); 
		PixelShader		= compile ps_2_0 ParticlePS(); 
	}
}


technique DrawBillboardParticlesColour_BillboardCpu
{
	pass 
	{ 
		VertexShader	= compile vs_2_0 ParticleVS_Cpu(true); 
		PixelShader		= compile ps_2_0 ParticlePS(); 
	}
}

#endif
