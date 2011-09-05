//CompilerOptions = ParentNamespace

float4x4 worldViewProj : WORLDVIEWPROJECTION;
float4 FillColour = 1;



void SimpleVS(
	float4 pos : POSITION,
	out float4 o_pos : POSITION)
{
	o_pos = mul(pos,worldViewProj);
}

float4 SimplePSGenericColour()   : COLOR 
{
	return FillColour;
}

void SimpleVSCol(
	float4 pos : POSITION, float4 colour : COLOR0, out float4 colOut : COLOR0, out float4 o_pos : POSITION)
{
	o_pos = mul(pos,worldViewProj);
	colOut = colour;
}

float4 SimplePSCol(float4 colour : COLOR)   : COLOR 
{
	return colour;
}




technique FillVertexColour
{
   pass
   {
		VertexShader = compile vs_2_0 SimpleVSCol();
		PixelShader = compile ps_2_0 SimplePSCol();
   }
}

technique FillSolidColour
{
   pass
   {
		VertexShader = compile vs_2_0 SimpleVS();
		PixelShader = compile ps_2_0 SimplePSGenericColour();
   }
}


