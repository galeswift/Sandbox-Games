//CompilerOptions = ParentNamespace

float4x4 worldViewProj		: WORLDVIEWPROJECTION;
float4x4 viewProj			: VIEWPROJECTION;
float4x4 worldMatrix		: WORLD;

float3 viewDirection		: VIEWDIRECTION;
float3 viewPoint			: VIEWPOINT;
float2 cameraNearFar		: CAMERANEARFAR;
float clipThreshold;

texture2D AlphaTexture;
sampler2D AlphaTextureSampler = sampler_state
{
	Texture = (AlphaTexture);
};


void DepthOutVS(
			float4	positionIn	: POSITION,
		out	float4	positionOut	: POSITION,
		out	float2	depth		: TEXCOORD0,
		uniform bool nonLinear)
{
	positionOut = mul(positionIn,worldViewProj);
	
	float3 worldPoint = mul(positionIn,worldMatrix).xyz - viewPoint.xyz;
	
	if (nonLinear)
	{
		depth = positionOut.zw;
	}
	else
	{
		depth = dot(viewDirection.xyz,worldPoint.xyz);
		depth = (depth - cameraNearFar.x) / (cameraNearFar.y - cameraNearFar.x);
	}
}



void DepthOutVS_texCoord(
			float4	positionIn	: POSITION,
		out	float4	positionOut	: POSITION,
		
			float2	texCoordIn	: TEXCOORD0,
		out	float2	texCoordOut	: TEXCOORD1,
		
		out	float2	depth		: TEXCOORD0,
		uniform bool nonLinear)
{
	texCoordOut = texCoordIn;
	
	positionOut = mul(positionIn,worldViewProj);
	
	float3 worldPoint = mul(positionIn,worldMatrix).xyz - viewPoint.xyz;
	
	if (nonLinear)
	{
		depth = positionOut.zw;
	}
	else
	{
		depth = dot(viewDirection.xyz,worldPoint.xyz);
		depth = (depth - cameraNearFar.x) / (cameraNearFar.y - cameraNearFar.x);
	}
}




float4 DepthOutPS(float depth : TEXCOORD0) : COLOR0
{
	return float4(depth,depth*depth,0,1);
}

float4 DepthOutPSAlpha(float depth : TEXCOORD0, float2 texCoord : TEXCOORD1) : COLOR0
{
	clip(tex2D(AlphaTextureSampler,texCoord).a-clipThreshold);
	return float4(depth,depth*depth,0,1);
}


float4 NonLinearDepthOutPS(float2 depth : TEXCOORD0) : COLOR0
{
	float value = depth.x / depth.y;
	return float4(value.xxx,1);
}

float4 NonLinearDepthOutPSAlpha(float2 depth : TEXCOORD0, float2 texCoord : TEXCOORD1) : COLOR0
{
	clip(tex2D(AlphaTextureSampler,texCoord).a-clipThreshold);
	float value = depth.x / depth.y;
	return float4(value.xxx,1);
}




technique DepthOutRg
{
	pass
	{
		VertexShader = compile vs_2_0 DepthOutVS(false);
		PixelShader = compile ps_2_0 DepthOutPS();
	}
}

technique DepthOutRgTextureAlphaClip
{
	pass
	{
		VertexShader = compile vs_2_0 DepthOutVS_texCoord(false);
		PixelShader = compile ps_2_0 DepthOutPSAlpha();
	}
}

technique NonLinearDepthOut
{
	pass
	{
		VertexShader = compile vs_2_0 DepthOutVS(true);
		PixelShader = compile ps_2_0 NonLinearDepthOutPS();
	}
}



technique NonLinearDepthOutTextureAlphaClip
{
	pass
	{
		VertexShader = compile vs_2_0 DepthOutVS_texCoord(true);
		PixelShader = compile ps_2_0 NonLinearDepthOutPSAlpha();
	}
}