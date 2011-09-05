//CompilerOptions = InternalClass, ParentNamespace
float4x4 worldViewProj : WORLDVIEWPROJECTION;

texture2D CustomTexture;
sampler2D CustomTextureSampler = sampler_state
{
	Texture = (CustomTexture);
};


void SimpleVSTex(
	float4 pos : POSITION, float2 tex : TEXCOORD0,
	out float4 o_pos : POSITION,out float2 o_tex : TEXCOORD0)
{
	o_pos = mul(pos,worldViewProj);
	o_tex = tex;
}

float4 SimplePSCustomTex(float2 tex : TEXCOORD0)   : COLOR 
{
	return tex2D(CustomTextureSampler,tex);
}

technique FillCustomTexture
{
   pass
   {
		VertexShader = compile vs_2_0 SimpleVSTex();
		PixelShader = compile ps_2_0 SimplePSCustomTex();
   }
}
