//CompilerOptions = InternalClass, ParentNamespace, DisableGenerateExtensions

float4x4 worldViewProj : WORLDVIEWPROJECTION;

float4 graphLine[200];

void VS(float index : POSITION, out float4 position : POSITION, out float good : COLOR0)
{
	float4 data = graphLine[index];
	position = float4(data.xyz,1);
	good = data.w;
	
	position = mul(position, worldViewProj);
}

float4 PS(float good : COLOR0) : COLOR0
{
	return float4(float2(1-good,good) / max(1-good,good),0,1);
}



technique DrawGraphLine
{
   pass
   {
		VertexShader = compile vs_2_0 VS();
		PixelShader = compile ps_2_0 PS();
   }
}