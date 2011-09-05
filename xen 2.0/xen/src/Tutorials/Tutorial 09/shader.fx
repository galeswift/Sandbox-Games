
float4x4 worldViewProj : WORLDVIEWPROJECTION;

//This shader demonstrates how to sample a texture


//First, the texture itself must be declared:
texture2D DisplayTexture;

//However, this cannot be directly used in the shader code.
//A sampler must be declared:

sampler2D DisplayTextureSampler = sampler_state
{
	Texture = (DisplayTexture);

	//Note the line above, 'Texture = DisplayTexture'.
	//This links this texture sampler to the texture resource.
	//Also note that both the texture and sampler are declared as '2D'.
	//They could also be declared as 3D, or CUBE.

	//The following are optional properties that set how the texture is sampled:
	//Most applications won't need to include these.
	//Xen, unlike XNA Effects, has default values for these properties
	
	//The Xen.Graphics.TextureSamplerState struct represents these settings.
	//it has documentation detailing how they work and what they do visually.
	
	MagFilter = LINEAR;		//(default - bilinear)
	MinFilter = LINEAR;		//(default - bilinear)
	MipFilter = POINT;		//(default - bilinear)
    AddressU = Clamp;		//(default is Wrap, or Clamp for Cubemaps)
    AddressV = Clamp;		//(default is Wrap, or Clamp for Cubemaps)
};


//--------------------------------------------------------------//
// vertex shader, simply outputs the texture coordinate
//--------------------------------------------------------------//
void Tutorial09VS(	
					float4 position			: POSITION, 
				out float4 positionOut		: POSITION,
					float2 texCoord			: TEXCOORD0,
				out float2 texCoordOut		: TEXCOORD0)
{
	positionOut = mul(position,worldViewProj);
	
	//pass the texture coordinate unmodified on to the pixel shader
	texCoordOut = texCoord;
}


//--------------------------------------------------------------//
// pixel shader, samples the texture
//--------------------------------------------------------------//
float4 Tutorial09PS(float2 texCoord : TEXCOORD0) : COLOR 
{
	//sample and return the texture, using unmodified texture coordinate
	return tex2D(DisplayTextureSampler, texCoord);
}



//--------------------------------------------------------------//
// Technique that uses the shaders
//--------------------------------------------------------------//
technique Tutorial09Technique
{
   pass
   {
		VertexShader = compile vs_2_0 Tutorial09VS();
		PixelShader = compile ps_2_0 Tutorial09PS();
   }
}