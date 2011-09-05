//////////////////////////////////////////////////////////////
// Example 2.1                                              //
//                                                          //
// These 'shared' parameters can be set once for every      //
// effect in the same EffectPool.  Each of these variables  //
// corresponds to an EffectParameter in the C# source.      //
//////////////////////////////////////////////////////////////
shared float4x4 world;
shared float4x4 view;
shared float4x4 projection;

struct VertexShaderOutput 
{
     float4 Position : POSITION;
     float4 Color : COLOR0;
};

struct PixelToFrame
{
    float4 Color        : COLOR0;
};


VertexShaderOutput VertexShaderFunction( float4 inPos : POSITION, float4 inColor : COLOR0)
{
	VertexShaderOutput Output = (VertexShaderOutput)0;

	float4x4 wvp = mul(mul(world, view), projection);

	//////////////////////////////////////////////////////////////
	// Example 2.4                                              //
	//                                                          //
	// Calculate the transformed position by multiplying the    //
	// input vertex by the world-view-position matrix.  Again   //
	// the "mul" intrinsic is used, only this time we're        //
	// mutiplying a poisiton vector (float4) by the wvp         //
	// matrix (float4x4).                                       //
	//                                                          //
	// The result of this is a float4 that represents the       //
	// transformed vertex position.                             //
	//////////////////////////////////////////////////////////////

	float4 transformedPosition = mul(inPos, wvp);

	Output.Position = transformedPosition;
	Output.Color	= inColor;

	return Output;
}

PixelToFrame OurFirstPixelShader(VertexShaderOutput PSIn)
 {
     PixelToFrame Output = (PixelToFrame)0;    
 
     Output.Color = PSIn.Color;    
 
     return Output;
 }

technique Simplest_3_0
{
    pass
    {
        PixelShader = compile ps_3_0 OurFirstPixelShader();
        VertexShader = compile vs_3_0 VertexShaderFunction();
    }
}