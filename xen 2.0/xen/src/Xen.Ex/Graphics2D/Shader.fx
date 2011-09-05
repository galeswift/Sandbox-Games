//CompilerOptions = InternalClass, ParentNamespace, DisableGenerateExtensions
float4x4 worldViewProj : WORLDVIEWPROJECTION;
float4x4 viewProj : VIEWPROJECTION;
float4x4 spriteWorldMatrix;

texture2D CustomTexture;
sampler2D CustomTextureSampler = sampler_state
{
	Texture = (CustomTexture);
};

void SimpleVSTexInstance(
	float4 sizePosition : POSITION12,
	float4 sizeTexture : POSITION13,
	float4 colour : POSITION14,
	float4 rotation : POSITION15,
	float4 pos : POSITION,
	out float4 o_pos : POSITION,
	out float2 o_tex : TEXCOORD0,
	out float4 o_colour : TEXCOORD1)
{
	float4 rot;
	rot.x = cos(rotation.x);
	rot.y = sin(rotation.x);
	
	float3x2 mat = float3x2(rot.x,-rot.y,rot.y,rot.x,rotation.y*sizePosition.z,rotation.z*sizePosition.w);
	
	o_tex = float2(pos.x,1-pos.y) * sizeTexture.zw + sizeTexture.xy;
	
	pos.xy = mul(float3((pos.xy-rotation.yz) * sizePosition.zw,1),mat);
	pos.xy = pos.xy + sizePosition.xy;
	o_pos = mul(mul(pos,spriteWorldMatrix),viewProj);
	o_colour = colour;
}


float time;
Matrix instances[60];

void SimpleVSTexNoInstance(
	float4 pos : POSITION,
	out float4 o_pos : POSITION,
	out float2 o_tex : TEXCOORD0,
	float index : TEXCOORD0,
	out float4 o_colour : TEXCOORD1)
{
	int i = (int)index;
	
	float4x4 instance = instances[i];
	
	float4 sizePosition = instance._m00_m01_m02_m03;
	float4 sizeTexture = instance._m10_m11_m12_m13;
	float4 colour = instance._m20_m21_m22_m23;
	float4 rotation = instance._m30_m31_m32_m33;
	
	float4 rot;
	rot.x = cos(rotation.x);
	rot.y = sin(rotation.x);
	
	float3x2 mat = float3x2(rot.x,-rot.y,rot.y,rot.x,rotation.y*sizePosition.z,rotation.z*sizePosition.w);//rot.x,rot.y,0,-rot.y,rot.x,0);
	
	
	o_tex = float2(pos.x,1-pos.y) * sizeTexture.zw + sizeTexture.xy;
	
	pos.xy = mul(float3((pos.xy-rotation.yz) * sizePosition.zw,1),mat);
	pos.xy = pos.xy + sizePosition.xy;
	o_pos = mul(pos,worldViewProj);
	o_colour = colour;
}

float4 SimplePSCustomTex(float2 tex : TEXCOORD0,float4 colour : TEXCOORD1)   : COLOR 
{
	return tex2D(CustomTextureSampler,tex) * colour;
}

technique InstancingSprite
{
   pass
   {
		VertexShader = compile vs_2_0 SimpleVSTexInstance();
		PixelShader = compile ps_2_0 SimplePSCustomTex();
   }
   pass Instancing
   {
		VertexShader = compile vs_2_0 SimpleVSTexInstance();
   }
}

technique NonInstancingSprite
{
   pass
   {
		VertexShader = compile vs_2_0 SimpleVSTexNoInstance();
		PixelShader = compile ps_2_0 SimplePSCustomTex();
   }
}