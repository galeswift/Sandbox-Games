//CompilerOptions = InternalClass, ParentNamespace, NoPreShader, DisableGenerateExtensions

float2 invTargetSize;
float3 indices[128];

void GetPosition(float3 data, out float4 position, out float2 lifeData, float offset)
{
	float index				= data.x;
	index					*= invTargetSize.x;
	
	float2 xy				= float2(fmod(index,1),(floor(index+1)+offset)*invTargetSize.y);
	
	position				= float4(0,0,0,1);
	position.xy				= xy * 2 - 1;
	
	lifeData				= data.yz;
}

void ParticleFill_VS_128(
		float4 position		: POSITION, 
	out	float4 positionOut	: POSITION,
	out	float2 texCoordOut	: TEXCOORD0)
{
	GetPosition(indices[position.x],positionOut,texCoordOut, position.w);
}
float4 ParticleFill_PS(float2 data : TEXCOORD0) : COLOR0
{
	return float4(data,0,0);
}

technique ParticleStoreLife128
{
   pass { VertexShader = compile vs_2_0 ParticleFill_VS_128(); PixelShader = compile ps_2_0 ParticleFill_PS(); }
}