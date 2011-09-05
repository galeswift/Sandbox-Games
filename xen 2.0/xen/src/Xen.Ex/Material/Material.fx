//CompilerOptions = NoPreShader, InternalClass, AvoidFlowControl, ParentNamespace


float4x4 worldViewProjection : WORLDVIEWPROJECTION;
float4x4 world : WORLD;

float4 v_viewPoint : VIEWPOINT;
float4 v_viewDir : VIEWDIRECTION;

texture2D CustomTexture;
sampler2D CustomTextureSampler = sampler_state
{
	Texture = (CustomTexture);
};
texture2D CustomNormalMap;
sampler2D CustomNormalMapSampler = sampler_state
{
	Texture = (CustomNormalMap);
};
texture2D CustomEmissiveTexture;
sampler2D CustomEmissiveTextureSampler = sampler_state
{
	Texture = (CustomEmissiveTexture);
};

//vertex shader
float4 v_lights[9]; //max 3
float4x3 v_SH;		//spherical harmonic. 
float3 v_fogAndAlpha;

//pixel shader
float4 p_lights[6]; //max 2
float4 p_fogColourAndGamma;
float4 p_EmissiveColour;


float SampleFog(float z)
{
	float fogStart = v_fogAndAlpha.x;
	float invFogEndMinusFogStart = v_fogAndAlpha.y;

	z += fogStart;
	z *= invFogEndMinusFogStart;

	return z;
}


float3 SampleSH(float3 normal)
{
	return mul(float4(normal,1),v_SH);
}


//////////////////////////////////////////////////////////////////////////////////
// Base Vertex Shader															//
//////////////////////////////////////////////////////////////////////////////////


struct VertexInput
{
	float4 WorldPosition		: POSITION;
	float3 Normal				: NORMAL;
	float4 VertexColour			: COLOR;
	float2 TexCoord				: TEXCOORD0;
};
struct VertexOutput
{
	float4 Position				: POSITION;
	float3 Lighting				: TEXCOORD0;
	float4 VertexColour			: COLOR0;
	float4 TexCoordFog			: TEXCOORD1;
	float4 WorldPosition		: TEXCOORD2;
	float4 ViewPosition			: TEXCOORD3;
	float3 Normal				: TEXCOORD4;
};

void VertexLightCore(
			in VertexInput input,
			inout VertexOutput output,
			uniform int vsLightCount,
			uniform bool useVertexColours)
{
	output.Position			= mul(input.WorldPosition,worldViewProjection);
	output.WorldPosition	= mul(input.WorldPosition,world);

	output.Normal			= input.Normal;
	
	output.TexCoordFog.xy	= input.TexCoord;
	output.Lighting			= 0;

	output.ViewPosition		= v_viewPoint - output.WorldPosition;

	float distance = length(output.ViewPosition.xyz);
	float3 viewNorm = output.ViewPosition.xyz / distance;
	
	output.Lighting			= SampleSH(input.Normal);

	for(int n=0; n<vsLightCount; n++)
	{
		float4 lightPosition	= v_lights[n*3];
		float4 specularColour	= v_lights[n*3+1];
		float4 diffuseColour	= v_lights[n*3+2];
		float attenuation		= diffuseColour.w;

		float3 lightDif = lightPosition.xyz - output.WorldPosition.xyz * lightPosition.w;
		float len2 = dot(lightDif,lightDif);
		float len = sqrt(len2);
	
		lightDif /= len;
		
		float3 halfVector = normalize(lightDif + viewNorm);
		float2 diffuseSpecular = float2(saturate(dot(lightDif,input.Normal)),saturate(dot(halfVector,input.Normal)));

		float falloff = (1 + attenuation * len2);
		
		diffuseSpecular.y = pow(diffuseSpecular.y, specularColour.w);
		diffuseSpecular /= falloff;

		output.Lighting += specularColour.rgb * diffuseSpecular.y + diffuseColour.rgb * diffuseSpecular.x;
	}

	float fog = SampleFog(dot(output.ViewPosition.xyz,v_viewDir.xyz));
	output.TexCoordFog.zw = float2(fog,1-fog);
	
	output.VertexColour = 1;
	if (useVertexColours)
	{
		output.VertexColour.rgb = saturate(input.VertexColour.rgb * input.VertexColour.rgb);
		output.VertexColour.a = input.VertexColour.a;
	}
	output.VertexColour.a *= v_fogAndAlpha.z;
}

//////////////////////////////////////////////////////////////////////////////////
// Vertex Shaders																//
//////////////////////////////////////////////////////////////////////////////////


void VertexLight(
			in VertexInput input,
			inout VertexOutput output,
			
			uniform int vsLightCount,
			uniform bool useVertexColours)
{
	input.Normal = normalize(mul(input.Normal,(float3x3)world));
	
	VertexLightCore(input,output,vsLightCount,useVertexColours);
}

//non-blending with tangent space for normal mapping
void VertexLightTangent(
			in VertexInput input,
			inout VertexOutput output,

			float3 in_binorm : BINORMAL, // y
			float3 in_tangent : TANGENT, // x
			
			out float3 out_binorm : TEXCOORD5,
			out float3 out_tangent : TEXCOORD6,
			
			uniform int vsLightCount,
			uniform bool useVertexColours)
{
	input.Normal = normalize(mul(input.Normal,(float3x3)world));

	out_binorm = normalize(mul(in_binorm,(float3x3)world));
	out_tangent = normalize(mul(in_tangent,(float3x3)world));
	
	VertexLightCore(input,output,vsLightCount,useVertexColours);
}


//////////////////////////////////////////////////////////////////////////////////
// Pixel Shaders																//
//////////////////////////////////////////////////////////////////////////////////


float4 PS0(
			float3 colour : TEXCOORD0,
			float4 VertexColour : COLOR0,
			float4 TexCoordFog : TEXCOORD1) : COLOR
{
	float4 textureSample = tex2D(CustomTextureSampler,TexCoordFog.xy);
	float4 emissiveSample = tex2D(CustomEmissiveTextureSampler,TexCoordFog.xy);

	float2 FogAlpha = saturate(TexCoordFog.zw);

	float4 rgba = float4(textureSample.rgb * textureSample.rgb * colour, textureSample.a) * FogAlpha.y;

	rgba *= VertexColour;
	
	rgba.rgb += p_fogColourAndGamma.rgb * FogAlpha.x;

	rgba.rgb += emissiveSample.rgb * p_EmissiveColour.rgb;

	rgba.rgb = pow(rgba.rgb,p_fogColourAndGamma.w);

	return rgba;
}

//this is only 63 instructions for 2 lights! the limit in PS_2_0 is 64!
float4 PS_norm_specular(
			float3 colour : TEXCOORD0,
			float4 VertexColour : COLOR0,
			float4 TexCoordFog : TEXCOORD1,
			float3 WorldPos : TEXCOORD2,
			float3 viewPoint : TEXCOORD3,
			float3 normal : TEXCOORD4,
			float3 binorm : TEXCOORD5,
			float3 tangent : TEXCOORD6,
			const uniform int psLightCount) : COLOR
{
	float4 normalMap = tex2D(CustomNormalMapSampler,TexCoordFog.xy);
	float3x3 normalSpace = float3x3(tangent,binorm,normal);
	
	normal = normalize(mul(normalMap.xyz-0.5,normalSpace));
	float3 viewNorm = normalize(viewPoint.xyz);

	float3 specular = 0;
	
	for(int n=0; n<psLightCount; n++)
	{
		float4 lightPosition	= p_lights[n*3];
		float4 specularColour	= p_lights[n*3+1];
		float4 diffuseColour	= p_lights[n*3+2];
		float attenuation		= diffuseColour.w;

		float3 lightDif = lightPosition.xyz - WorldPos * lightPosition.w;
		float len2 = dot(lightDif,lightDif);
		float len = sqrt(len2);
	
		lightDif /= len;
		
		float3 halfVector = normalize(lightDif + viewNorm);
		float2 diffuseSpecular = float2(saturate(dot(lightDif,normal)),saturate(dot(halfVector,normal)));

		float falloff = (1 + attenuation * len2);
		
		diffuseSpecular.y = pow(diffuseSpecular.y, specularColour.w);
		diffuseSpecular /= falloff;

		specular += specularColour.rgb * diffuseSpecular.y;
		colour += diffuseColour.rgb * diffuseSpecular.x;
	}

	colour += specular * normalMap.w;

	float4 textureSample = tex2D(CustomTextureSampler,TexCoordFog.xy);
	float4 emissiveSample = tex2D(CustomEmissiveTextureSampler,TexCoordFog.xy);

	float2 FogAlpha = saturate(TexCoordFog.zw);

	float4 rgba = float4(textureSample.rgb * textureSample.rgb * colour, textureSample.a) * FogAlpha.y;

	rgba *= VertexColour;
	
	rgba.rgb += p_fogColourAndGamma.rgb * FogAlpha.x;

	rgba.rgb += emissiveSample.rgb * p_EmissiveColour.rgb;

	rgba.rgb = pow(rgba.rgb,p_fogColourAndGamma.w);

	return rgba;
}



////////////////////////////////////////////////////////////////////////////////
///
///
///  per-vertex shaders
///  'c' postfix indicates vertex colour support
///

technique vs0 < string BaseTypes = "IMS_Base"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLight(0,false);
		PixelShader = compile ps_2_0 PS0();
	}
}
technique vs1 < string BaseTypes = "IMS_PerVertex"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLight(1,false);
		PixelShader = compile ps_2_0 PS0();
	}
}
technique vs3 < string BaseTypes = "IMS_PerVertex"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLight(3,false);
		PixelShader = compile ps_2_0 PS0();
	}
}

technique vs0c < string BaseTypes = "IMS_Base, IMS_VertexColour"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLight(0,true);
		PixelShader = compile ps_2_0 PS0();
	}
}
technique vs1c < string BaseTypes = "IMS_PerVertex, IMS_VertexColour"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLight(1,true);
		PixelShader = compile ps_2_0 PS0();
	}
}
technique vs3c < string BaseTypes = "IMS_PerVertex, IMS_VertexColour"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLight(3,true);
		PixelShader = compile ps_2_0 PS0();
	}
}










////////////////////////////////////////////////////////////////////////////////
///
///
///  per-pixel shaders
///
///

technique ps1n < string BaseTypes = "IMS_PerPixel"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLightTangent(0,false);
		PixelShader = compile ps_2_0 PS_norm_specular(1);
	}
}
technique ps2n < string BaseTypes = "IMS_PerPixel"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLightTangent(0,false);
		PixelShader = compile ps_2_0 PS_norm_specular(2);
	}
}

technique ps1nc < string BaseTypes = "IMS_PerPixel, IMS_VertexColour"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLightTangent(0,true);
		PixelShader = compile ps_2_0 PS_norm_specular(1);
	}
}
technique ps2nc < string BaseTypes = "IMS_PerPixel, IMS_VertexColour"; >
{
	pass
	{
		VertexShader = compile vs_2_0 VertexLightTangent(0,true);
		PixelShader = compile ps_2_0 PS_norm_specular(2);
	}
}
