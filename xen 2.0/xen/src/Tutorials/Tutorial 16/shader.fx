
//This shader implements a simple projection, with two extra techniques for Hardware instancing and animation support

//**** IMPORTANT NOTE ****
//In this example, a very simple shader is defined. The shader defines an Instancing and Animation extension.
//
// *** DOING THIS IS TYPICALLY NOT REQUIRED ***
//
//Internally, XenFX will attempt to automatically generate instancing and animation extensions - so you 
//don't need to worry about them. But in this case, they are implemented manually to demonstrate the
//process required to get instancing or animation shaders to work if the automatic process fails.

float4x4 worldViewProj : WORLDVIEWPROJECTION;

//An instancing shader provides it's own world matrix, it only needs the view*projection matrix
float4x4 viewProj : VIEWPROJECTION;

//The animation blending matrix data. It ideally should be 72*3 elements large. It must be float4
float4 blendMatrices[72 * 3] : BLENDMATRICES;	

float4 colour : GLOBAL;

//--------------------------------------------------------------//
// vertex shader
//--------------------------------------------------------------//
void Tutorial16_VS(	
					float4 position			: POSITION, 
				out float4 positionOut		: POSITION)
{
	positionOut = mul(position,worldViewProj);
}


//--------------------------------------------------------------//
// vertex shader, using instancing for the world matrix
// when using hardware instancing, the instance world matrix 
// is passed in {POSITION12,POSITION13,POSITION14,POSITION15}
// Note: XenFX will try and generate this shader for you automatically!
// This is a sample to demonstrate manually implementing the shader
// THIS IS AN EXAMPLE OF MANUAL IMPLEMENTATION!
//--------------------------------------------------------------//
void Tutorial16_VS_Instance(	
					float4 position			: POSITION, 
				out float4 positionOut		: POSITION,
				
					//instancing input matrix:
					float4 worldX			: POSITION12,
					float4 worldY			: POSITION13,
					float4 worldZ			: POSITION14,
					float4 worldW			: POSITION15)
{
	//get the instance world matrix
	float4x4 instanceMatrix = float4x4(worldX,worldY,worldZ,worldW);
	
	//note the world matrix is multiplied first, then the view projection
	float4 instanceWorldPosition = mul(position, instanceMatrix);
	
	//multiply by just the view*projection, as the instanceMatrix has already acted as the world matrix
	positionOut = mul(instanceWorldPosition,viewProj);
}



//--------------------------------------------------------------//
// The following code demonstrates how to manually implement
// and animation blending shader.
// XenFX will *also* automatically generate this shader if it can!
// THIS IS AN EXAMPLE OF MANUAL IMPLEMENTATION!
//--------------------------------------------------------------//
void Tutorial16_VS_Animation(	
					float4 position			: POSITION, 
				out float4 positionOut		: POSITION,
				
					//blending input values:
					float4	weights		: BLENDWEIGHT,
					int4	indices		: BLENDINDICES)
{
	//Get the blending animation matrix.
	//Don't worry about the complexity of this code, the compiler optimises it down to be highly efficient.
	float4x3 blendMatrix
				 = transpose(float3x4(
					blendMatrices[indices.x*3+0] * weights.x + blendMatrices[indices.y*3+0] * weights.y + blendMatrices[indices.z*3+0] * weights.z + blendMatrices[indices.w*3+0] * weights.w,
					blendMatrices[indices.x*3+1] * weights.x + blendMatrices[indices.y*3+1] * weights.y + blendMatrices[indices.z*3+1] * weights.z + blendMatrices[indices.w*3+1] * weights.w,
					blendMatrices[indices.x*3+2] * weights.x + blendMatrices[indices.y*3+2] * weights.y + blendMatrices[indices.z*3+2] * weights.z + blendMatrices[indices.w*3+2] * weights.w
				   ));
	
	//the blend matrix is multiplied first, then the world view projection (note, this is different to instancing!)
	float4 blendPosition =	float4(mul(position,blendMatrix).xyz, position.w); 

	//multiply by world*view*projection
	positionOut = mul(blendPosition,worldViewProj);
}



//--------------------------------------------------------------//
// pixel shader, returns the global colour
//--------------------------------------------------------------//
float4 Tutorial16_PS() : COLOR 
{
	return colour;
}





//--------------------------------------------------------------//
// Technique that uses the shaders
//--------------------------------------------------------------//
technique Tutorial16
{
	pass
	{
		VertexShader = compile vs_2_0 Tutorial16_VS();
		PixelShader = compile ps_2_0 Tutorial16_PS();
	}

	//define the instancing variation of the vertex shader!
	//Remember, XenFX will try to automatically implement this extension if this pass isn't defined!
	//This is provided as an example implementation!
	pass Instancing
	{
		VertexShader = compile vs_2_0 Tutorial16_VS_Instance();
	}

	//And now the animation pass
	pass Animation
	{
		VertexShader = compile vs_2_0 Tutorial16_VS_Animation();
	}
}