shared float4x4 world;
shared float4x4 view;
shared float4x4 projection;

float specPower = 15.0f;
float aIntensity = 0.8f;
float dIntensity = 1.0f;
float4 aLightColor = float4(0.15, 0.15, 0.2, 1.0);;
float4 dLightColor = float4(1.0,0.0, 0.0, 1.0);
float4 sLightColor = float4(1.0f, 1.0f, 0.0f, 1.0f);
float4 lightDirection = float4(0.0f, 0.0f, 1.0f, 1.0f);

// Camera Position
float3 vEye;


struct VertexShaderOutput 
{
    float4 Pos: POSITION; 
    float3 L:   TEXCOORD0; 
    float3 N:   TEXCOORD1; 
    float3 V:	TEXCOORD2;
};

struct PixelShaderInput
{
     float4 Color: COLOR0;
};


// We take the position from the model file, as well as the normal, and pass it into the shader. 
// Based on these and our global variables, we can transform the position Pos, 
// normalize the light direction and transforming+normalizing the normal of the surface.

VertexShaderOutput VertexShaderFunction(
     float3 position : POSITION,
     float3 normal : NORMAL )
{        
    VertexShaderOutput Out = (VertexShaderOutput) 0;     
 
   	//generate the world-view-proj matrix
	float4x4 wvp = mul(mul(world, view), projection);
	
	// Tranform Pos with matWorld in order to get the correct view vector.
	float4 posWorld = mul(position,world); 
		
    Out.Pos = mul(float4(position, 1.0), wvp);              
    
    Out.L = normalize(lightDirection); 
    Out.N = normalize(mul(normal, world));     
    
    // Eye position - vertex position returns the view direction from eye to the vertex.
	Out.V = float4(vEye,0.0f) - posWorld;
    return Out; 
}

//Then, in the pixel shader we take the values in TEXCOORD0 and put it in L, 
// and the values in TEXCOORD1 and put it in N. These registers are filled by the vertex shader
float4 PixelShaderFunction(float3 L: TEXCOORD0, float3 N : TEXCOORD1, float3 V : TEXCOORD2) : COLOR
{       
	// normalize our vectors.
    float3 normal = normalize(N);
    float3 lightDir = normalize(L);
    float3 viewDir = normalize(V);    
                
    // calculate diffuse light
	float diff = saturate(dot(normal, lightDir)); 
    
    // Create our reflection shader
    // R = 2 * (N.L) * N – L
    float3 reflect = normalize(2 * diff * normal - lightDir);  
    
    // Calculate our specular light
    float specular = pow(saturate(dot(reflect, viewDir)), specPower); // R.V^n
    
    // return our final light equation
    // I = A + Dcolor * Dintensity * N.L + Scolor * Sintensity * (R.V)n
    return aLightColor + dLightColor * diff + sLightColor * specular;         
}

technique VertexLighting
{
     
    pass P0
    {
          //set the VertexShader state to the vertex shader function
          VertexShader = compile vs_2_0 VertexShaderFunction();
          
          //set the PixelShader state to the pixel shader function          
          PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}