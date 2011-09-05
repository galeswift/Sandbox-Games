int Iterations = 84;
float2 Pan = float2(0.5, 0);
float Zoom = 4;
float Aspect = 1;
float2 JuliaSeed = float2(0.39, -0.2);
float3 ColorScale = float3(4, 5, 6);

shared float4x4 world;
shared float4x4 view;
shared float4x4 projection;
float2 ViewportSize    : register(c0);
float2 TextureSize     : register(c1);

struct VertexShaderOutput 
{
     float4 Position : POSITION;
     float4 Color : COLOR0;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};

// Vertex shader for rendering sprites on Windows.
void SpriteVertexShader(inout float4 position : POSITION0,
		  				inout float4 color    : COLOR0,
						inout float2 texCoord : TEXCOORD0)
{
	float4x4 wvp = mul(mul(world, view), projection);
	
    // Apply the matrix transform.
    position = mul(position, wvp);
    
	// Half pixel offset for correct texel centering.
	position.xy -= 0.5;

	// Viewport adjustment.
	position.xy /= ViewportSize;
	position.xy *= float2(2, -2);
	position.xy -= float2(1, -1);

	// Compute the texture coordinate.
	texCoord /= TextureSize;
}

PixelToFrame OurFirstPixelShader(VertexShaderOutput PSIn)
 {
     PixelToFrame Output = (PixelToFrame)0;    
 
     Output.Color = PSIn.Color;
     return Output;
 }
 
/////////////////////
// BEGIN FRACTAL
/////////////////////
float ComputeValue(float2 v, float2 offset)
{
	float vxsquare = 0;
	float vysquare = 0;

	int iteration = 0;
	int lastIteration = Iterations;

	do
	{
		vxsquare = v.x * v.x;
		vysquare = v.y * v.y;

		v = float2(vxsquare - vysquare, v.x * v.y * 2) + offset;

		iteration++;

		if ((lastIteration == Iterations) && (vxsquare + vysquare) > 4.0)
		{
			lastIteration = iteration + 1;
		}
	}
	while (iteration < lastIteration);

	return (float(iteration) - (log(log(sqrt(vxsquare + vysquare))) / log(2.0))) / float(Iterations);
}

float ComputeValuePoly(float2 v, float2 offset)
{
	float vxsquared = 0;
	float vysquared = 0;

	int iteration = 0;
	int lastIteration = Iterations;

	do
	{
		vxsquared = v.x * v.x;
		vysquared = v.y * v.y;
		
		v = float2( vxsquared - vysquared, v.x * v.y * 2.0 + 0.2 * vxsquared * vysquared ) + offset;

		iteration++;

		if ((lastIteration == Iterations) && (vxsquared + vysquared) >4.0)
		{
			lastIteration = iteration + 1;
		}
	}
	while (iteration < lastIteration);

	return (float(iteration) - (log(log(sqrt(vxsquared + vysquared))) / log(2.0))) / float(Iterations);
}
 
float4 Mandelbrot_PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 v = (texCoord - 0.5) * Zoom * float2(1, Aspect) - Pan;

	float val = ComputeValue(v, v);

	return float4(sin(val * ColorScale.x), sin(val * ColorScale.y), sin(val * ColorScale.z), 1);
}

float4 Julia_PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 v = (texCoord - 0.5) * Zoom * float2(1, Aspect) - Pan;

	float val = ComputeValue(v, JuliaSeed);

	return float4(sin(val * ColorScale.x), sin(val * ColorScale.y), sin(val * ColorScale.z), 1);
}

float4 Poly_PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 v = (texCoord - 0.5) * Zoom * float2(1, Aspect) - Pan;

	float val = ComputeValuePoly(v, JuliaSeed);

	return float4(sin(val * ColorScale.x), sin(val * ColorScale.y), sin(val * ColorScale.z), 1);
}

technique Mandelbrot
{
	pass 
	{
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 Mandelbrot_PixelShader();
	}
}

technique Julia
{
	pass
	{		
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 Julia_PixelShader();
	}
}

technique Poly
{
	pass
	{		
		VertexShader = compile vs_3_0 SpriteVertexShader();
		PixelShader = compile ps_3_0 Poly_PixelShader();
	}
}