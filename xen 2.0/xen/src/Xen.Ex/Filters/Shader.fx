//CompilerOptions = NoPreShader, InternalClass, ParentNamespace, DisableGenerateExtensions
//preshaders are a bit of a waste for simple filters (in the VS at least)...

float4x4 worldViewProj : WORLDVIEWPROJECTION;

float2 textureSize = 1;

float2 sampleDirection = 1;

float3 kernel[16];

texture2D Texture;
sampler2D TextureSampler = sampler_state
{
	Texture = (Texture);
	AddressU = CLAMP;
	AddressV = CLAMP;
};
sampler2D PointSampler = sampler_state
{
	Texture = (Texture);
	AddressU = CLAMP;
	AddressV = CLAMP;
	MinFilter = POINT;
	MagFilter = POINT;
	MipFilter = NONE;
};



void VS_Down8(float4 pos : POSITION, out float4 o_pos : POSITION,
	in float2 tex : TEXCOORD0,
	
	out float2 tex0 : TEXCOORD0,
	out float2 tex1 : TEXCOORD1,
	out float2 tex2 : TEXCOORD2,
	out float2 tex3 : TEXCOORD3,
	out float2 tex4 : TEXCOORD4,
	out float2 tex5 : TEXCOORD5,
	out float2 tex6 : TEXCOORD6,
	out float2 tex7 : TEXCOORD7	
)
{
	o_pos = mul(pos,worldViewProj);
	
	
	tex0 = tex + sampleDirection * +3.5;
	tex1 = tex + sampleDirection * +2.5;
	tex2 = tex + sampleDirection * +1.5;
	tex3 = tex + sampleDirection * +0.5;
	tex4 = tex + sampleDirection * -0.5;
	tex5 = tex + sampleDirection * -1.5;
	tex6 = tex + sampleDirection * -2.5;
	tex7 = tex + sampleDirection * -3.5;
}
void VS_Down4(float4 pos : POSITION, out float4 o_pos : POSITION,
	in float2 tex : TEXCOORD0,
	
	out float2 tex0 : TEXCOORD0,
	out float2 tex1 : TEXCOORD1,
	out float2 tex2 : TEXCOORD2,
	out float2 tex3 : TEXCOORD3	
)
{
	o_pos = mul(pos,worldViewProj);
	
	
	tex0 = tex + sampleDirection * +1.5;
	tex1 = tex + sampleDirection * +0.5;
	tex2 = tex + sampleDirection * -0.5;
	tex3 = tex + sampleDirection * -1.5;
}
void VS_Down2(float4 pos : POSITION, out float4 o_pos : POSITION,
	in float2 tex : TEXCOORD0,
	
	out float2 tex0 : TEXCOORD0,
	out float2 tex1 : TEXCOORD1	
)
{
	o_pos = mul(pos,worldViewProj);
	
	
	tex0 = tex + sampleDirection * +0.5;
	tex1 = tex + sampleDirection * -0.5;
}
float4 PS_Down8(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1,
	float2 tex2 : TEXCOORD2,
	float2 tex3 : TEXCOORD3,
	float2 tex4 : TEXCOORD4,
	float2 tex5 : TEXCOORD5,
	float2 tex6 : TEXCOORD6,
	float2 tex7 : TEXCOORD7,
	uniform bool log_out,
	uniform int components)   : COLOR 
{
	float4 output = 0;
	if (log_out)
	{
		output= (
			log(tex2D(PointSampler,tex0)) + 
			log(tex2D(PointSampler,tex1)) + 
			log(tex2D(PointSampler,tex2)) + 
			log(tex2D(PointSampler,tex3)) + 
			log(tex2D(PointSampler,tex4)) + 
			log(tex2D(PointSampler,tex5)) + 
			log(tex2D(PointSampler,tex6)) + 
			log(tex2D(PointSampler,tex7))) / 8;
	}
	else
	{
		output= (
			tex2D(PointSampler,tex0) + 
			tex2D(PointSampler,tex1) + 
			tex2D(PointSampler,tex2) + 
			tex2D(PointSampler,tex3) + 
			tex2D(PointSampler,tex4) + 
			tex2D(PointSampler,tex5) + 
			tex2D(PointSampler,tex6) + 
			tex2D(PointSampler,tex7)) / 8;
	}
	if (components == 1)
		return float4(output.x,0,0,0);
	if (components == 2)
		return float4(output.xy,0,0);
	if (components == 3)
		return float4(output.xyz,0);
	if (components == 4)
		return output;
		
	return 0;
}
float4 PS_Down4(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1,
	float2 tex2 : TEXCOORD2,
	float2 tex3 : TEXCOORD3,
	uniform bool log_out,
	uniform int components
	)   : COLOR 
{
	float4 output = 0;
	if (log_out)
	{
		output =  (
			log(tex2D(PointSampler,tex0)) + 
			log(tex2D(PointSampler,tex1)) + 
			log(tex2D(PointSampler,tex2)) + 
			log(tex2D(PointSampler,tex3))) / 4;
	}
	else
	{
		output =  (
			tex2D(PointSampler,tex0) + 
			tex2D(PointSampler,tex1) + 
			tex2D(PointSampler,tex2) + 
			tex2D(PointSampler,tex3)) / 4;
	}
	if (components == 1)
		return float4(output.x,0,0,0);
	if (components == 2)
		return float4(output.xy,0,0);
	if (components == 3)
		return float4(output.xyz,0);
	if (components == 4)
		return output;
		
	return 0;
}
float4 PS_Down2(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1,
	uniform bool log_out,
	uniform int components
	)   : COLOR 
{
	float4 output = 0;
	if (log_out)
	{
		output =  (
			log(tex2D(PointSampler,tex0)) + 
			log(tex2D(PointSampler,tex1))) / 2;
	}
	else
	{
		output =  (
			tex2D(PointSampler,tex0) + 
			tex2D(PointSampler,tex1)) / 2;
	}	
	if (components == 1)
		return float4(output.x,0,0,0);
	if (components == 2)
		return float4(output.xy,0,0);
	if (components == 3)
		return float4(output.xyz,0);
	if (components == 4)
		return output;
		
	return 0;
}

















void VS_Filter16(float4 pos : POSITION, out float4 o_pos : POSITION,
	in float2 tex : TEXCOORD0,
	
	out float4 tex0 : TEXCOORD0,
	out float4 tex1 : TEXCOORD1,
	out float4 tex2 : TEXCOORD2,
	out float4 tex3 : TEXCOORD3,
	out float4 tex4 : TEXCOORD4,
	out float4 tex5 : TEXCOORD5,
	out float4 tex6 : TEXCOORD6,
	out float4 tex7 : TEXCOORD7
	
)
{
	o_pos = mul(pos,worldViewProj);
	
	float2 size = textureSize;
	
	tex0.xy = tex + kernel[0].xy * size;
	tex1.xy = tex + kernel[1].xy * size;
	tex2.xy = tex + kernel[2].xy * size;
	tex3.xy = tex + kernel[3].xy * size;
	tex4.xy = tex + kernel[4].xy * size;
	tex5.xy = tex + kernel[5].xy * size;
	tex6.xy = tex + kernel[6].xy * size;
	tex7.xy = tex + kernel[7].xy * size;
	
	tex0.zw = tex + kernel[8].xy * size;
	tex1.zw = tex + kernel[9].xy * size;
	tex2.zw = tex + kernel[10].xy * size;
	tex3.zw = tex + kernel[11].xy * size;
	tex4.zw = tex + kernel[12].xy * size;
	tex5.zw = tex + kernel[13].xy * size;
	tex6.zw = tex + kernel[14].xy * size;
	tex7.zw = tex + kernel[15].xy * size;
}



void VS_Filter8(float4 pos : POSITION, out float4 o_pos : POSITION,
	in float2 tex : TEXCOORD0,
	
	out float2 tex0 : TEXCOORD0,
	out float2 tex1 : TEXCOORD1,
	out float2 tex2 : TEXCOORD2,
	out float2 tex3 : TEXCOORD3,
	out float2 tex4 : TEXCOORD4,
	out float2 tex5 : TEXCOORD5,
	out float2 tex6 : TEXCOORD6,
	out float2 tex7 : TEXCOORD7
	
)
{
	o_pos = mul(pos,worldViewProj);
	
	float2 size = textureSize;
	
	tex0.xy = tex + kernel[0].xy * size;
	tex1.xy = tex + kernel[1].xy * size;
	tex2.xy = tex + kernel[2].xy * size;
	tex3.xy = tex + kernel[3].xy * size;
	
	tex4.xy = tex + kernel[4].xy * size;
	tex5.xy = tex + kernel[5].xy * size;
	tex6.xy = tex + kernel[6].xy * size;
	tex7.xy = tex + kernel[7].xy * size;
}

void VS_Filter4(float4 pos : POSITION, out float4 o_pos : POSITION,
	in float2 tex : TEXCOORD0,
	
	out float2 tex0 : TEXCOORD0,
	out float2 tex1 : TEXCOORD1,
	out float2 tex2 : TEXCOORD2,
	out float2 tex3 : TEXCOORD3
	
)
{
	o_pos = mul(pos,worldViewProj);
	
	float2 size = textureSize;
	
	tex0.xy = tex + kernel[0].xy * size;
	tex1.xy = tex + kernel[1].xy * size;
	
	tex2.xy = tex + kernel[2].xy * size;
	tex3.xy = tex + kernel[3].xy * size;
}

void VS_Filter2(float4 pos : POSITION, out float4 o_pos : POSITION,
	in float2 tex : TEXCOORD0,
	
	out float2 tex0 : TEXCOORD0,
	out float2 tex1 : TEXCOORD1
)
{
	o_pos = mul(pos,worldViewProj);
	
	float2 size = textureSize;
	
	tex0.xy = tex + kernel[0].xy * size;
	
	tex1.xy = tex + kernel[1].xy * size;
}


float4 PS_Filter16(
	float4 tex0 : TEXCOORD0,
	float4 tex1 : TEXCOORD1,
	float4 tex2 : TEXCOORD2,
	float4 tex3 : TEXCOORD3,
	float4 tex4 : TEXCOORD4,
	float4 tex5 : TEXCOORD5,
	float4 tex6 : TEXCOORD6,
	float4 tex7 : TEXCOORD7
	)   : COLOR 
{
	return 
		tex2D(TextureSampler,tex0.xy) * kernel[0].z + 
		tex2D(TextureSampler,tex1.xy) * kernel[1].z + 
		tex2D(TextureSampler,tex2.xy) * kernel[2].z + 
		tex2D(TextureSampler,tex3.xy) * kernel[3].z + 
		tex2D(TextureSampler,tex4.xy) * kernel[4].z + 
		tex2D(TextureSampler,tex5.xy) * kernel[5].z + 
		tex2D(TextureSampler,tex6.xy) * kernel[6].z + 
		tex2D(TextureSampler,tex7.xy) * kernel[7].z
		+
		tex2D(TextureSampler,tex0.zw) * kernel[8].z + 
		tex2D(TextureSampler,tex1.zw) * kernel[9].z + 
		tex2D(TextureSampler,tex2.zw) * kernel[10].z + 
		tex2D(TextureSampler,tex3.zw) * kernel[11].z + 
		tex2D(TextureSampler,tex4.zw) * kernel[12].z + 
		tex2D(TextureSampler,tex5.zw) * kernel[13].z + 
		tex2D(TextureSampler,tex6.zw) * kernel[14].z + 
		tex2D(TextureSampler,tex7.zw) * kernel[15].z;
}

float4 PS_Filter15(
	float4 tex0 : TEXCOORD0,
	float4 tex1 : TEXCOORD1,
	float4 tex2 : TEXCOORD2,
	float4 tex3 : TEXCOORD3,
	float4 tex4 : TEXCOORD4,
	float4 tex5 : TEXCOORD5,
	float4 tex6 : TEXCOORD6,
	float2 tex7 : TEXCOORD7
	)   : COLOR 
{
	return 
		tex2D(TextureSampler,tex0.xy) * kernel[0].z + 
		tex2D(TextureSampler,tex1.xy) * kernel[1].z + 
		tex2D(TextureSampler,tex2.xy) * kernel[2].z + 
		tex2D(TextureSampler,tex3.xy) * kernel[3].z + 
		tex2D(TextureSampler,tex4.xy) * kernel[4].z + 
		tex2D(TextureSampler,tex5.xy) * kernel[5].z + 
		tex2D(TextureSampler,tex6.xy) * kernel[6].z + 
		tex2D(TextureSampler,tex7.xy) * kernel[7].z
		+
		tex2D(TextureSampler,tex0.zw) * kernel[8].z + 
		tex2D(TextureSampler,tex1.zw) * kernel[9].z + 
		tex2D(TextureSampler,tex2.zw) * kernel[10].z + 
		tex2D(TextureSampler,tex3.zw) * kernel[11].z + 
		tex2D(TextureSampler,tex4.zw) * kernel[12].z + 
		tex2D(TextureSampler,tex5.zw) * kernel[13].z + 
		tex2D(TextureSampler,tex6.zw) * kernel[14].z;
}


float4 PS_Filter8(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1,
	float2 tex2 : TEXCOORD2,
	float2 tex3 : TEXCOORD3,
	float2 tex4 : TEXCOORD4,
	float2 tex5 : TEXCOORD5,
	float2 tex6 : TEXCOORD6,
	float2 tex7 : TEXCOORD7
	)   : COLOR 
{
	return 
		tex2D(TextureSampler,tex0.xy) * kernel[0].z + 
		tex2D(TextureSampler,tex1.xy) * kernel[1].z + 
		tex2D(TextureSampler,tex2.xy) * kernel[2].z + 
		tex2D(TextureSampler,tex3.xy) * kernel[3].z
		+
		tex2D(TextureSampler,tex4.xy) * kernel[4].z + 
		tex2D(TextureSampler,tex5.xy) * kernel[5].z + 
		tex2D(TextureSampler,tex6.xy) * kernel[6].z + 
		tex2D(TextureSampler,tex7.xy) * kernel[7].z;
}

float4 PS_Filter7(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1,
	float2 tex2 : TEXCOORD2,
	float2 tex3 : TEXCOORD3,
	float2 tex4 : TEXCOORD4,
	float2 tex5 : TEXCOORD5,
	float2 tex6 : TEXCOORD6
	)   : COLOR 
{
	return 
		tex2D(TextureSampler,tex0.xy) * kernel[0].z + 
		tex2D(TextureSampler,tex1.xy) * kernel[1].z + 
		tex2D(TextureSampler,tex2.xy) * kernel[2].z + 
		tex2D(TextureSampler,tex3.xy) * kernel[3].z
		+
		tex2D(TextureSampler,tex4.xy) * kernel[4].z + 
		tex2D(TextureSampler,tex5.xy) * kernel[5].z + 
		tex2D(TextureSampler,tex6.xy) * kernel[6].z;
}

float4 PS_Filter4(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1,
	float2 tex2 : TEXCOORD2,
	float2 tex3 : TEXCOORD3
	)   : COLOR 
{
	return 
		tex2D(TextureSampler,tex0.xy) * kernel[0].z + 
		tex2D(TextureSampler,tex1.xy) * kernel[1].z
		+
		tex2D(TextureSampler,tex2.xy) * kernel[2].z + 
		tex2D(TextureSampler,tex3.xy) * kernel[3].z;
}

float4 PS_Filter3(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1,
	float2 tex2 : TEXCOORD2
	)   : COLOR 
{
	return 
		tex2D(TextureSampler,tex0.xy) * kernel[0].z + 
		tex2D(TextureSampler,tex1.xy) * kernel[1].z
		+
		tex2D(TextureSampler,tex2.xy) * kernel[2].z;
}

float4 PS_Filter2(
	float2 tex0 : TEXCOORD0,
	float2 tex1 : TEXCOORD1
	)   : COLOR 
{
	return 
		tex2D(TextureSampler,tex0.xy) * kernel[0].z
		+
		tex2D(TextureSampler,tex1.xy) * kernel[1].z;
}



technique Kernel16
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Filter16();
		PixelShader = compile ps_2_0 PS_Filter16();
   }
}
technique Kernel8
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Filter8();
		PixelShader = compile ps_2_0 PS_Filter8();
   }
}
technique Kernel4
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Filter4();
		PixelShader = compile ps_2_0 PS_Filter4();
   }
}

technique Kernel15
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Filter16();
		PixelShader = compile ps_2_0 PS_Filter15();
   }
}
technique Kernel7
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Filter8();
		PixelShader = compile ps_2_0 PS_Filter7();
   }
}
technique Kernel3
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Filter4();
		PixelShader = compile ps_2_0 PS_Filter3();
   }
}
technique Kernel2
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Filter2();
		PixelShader = compile ps_2_0 PS_Filter2();
   }
}


technique Downsample8 < string BaseTypes = "IDownsampleShader"; >
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Down8();
		PixelShader = compile ps_2_0 PS_Down8(false,4);
   }
}
technique Downsample4 < string BaseTypes = "IDownsampleShader"; >
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Down4();
		PixelShader = compile ps_2_0 PS_Down4(false,4);
   }
}
technique Downsample2 < string BaseTypes = "IDownsampleShader"; >
{
   pass
   {
		VertexShader = compile vs_2_0 VS_Down2();
		PixelShader = compile ps_2_0 PS_Down2(false,4);
   }
}
