using System;
using System.Collections.Generic;
using System.Text;
using Xen.Graphics;
using Xen.Camera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Xen.Ex.Graphics.Display;
using Xen.Ex.Graphics;
using Xen.Ex.Graphics2D;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Xen.Ex.Graphics.Content;

namespace Xen.Ex.Graphics.Processor
{
#if DEBUG && !XBOX360

	//this class needs to be here in order to hot load the particle system at runtime
	//it generates pixel shaders for particle systems.
	//the vertex shaders are consistent for all particle systems, and are embedded in GpuParticleData
	static class GpuParticleShaderBuilder
	{
		#region shader string


		//starts at line 30
		const string BaseShader =
@"

//ps registers
float4 _ps_c[6] : register(ps,c0);

_#define globals(IDX) (_ps_c[IDX])
_#define constants (_ps_c[4])
_#define constants2 (_ps_c[5])

/*
float4 globals[4]	: register(ps,c0);
// stepSize (delta time), current step, noise base XY
float4 constants	: register(ps,c4);
// current the max time step, and two random offsets (very small values)
float4 constants2	: register(ps,c5);
*/


sampler2D RandSampler				: register(ps,s0);

_#ifdef TEXTURE_PARTICLE_METHOD
sampler2D PositionSizeSampler		: register(ps,s1);
sampler2D VelocityRotationSampler	: register(ps,s2);
_#endif

float2 random(float x, float y)
{
	return tex2D(RandSampler,float2(x,y)) + constants2.xy;
}

float rand(float from, float to, float randX, float randY)
{
	return from + (to - from) * random(randX,randY).x;
}
float rand(float to, float randX, float randY)
{
	return to * random(randX,randY).x;
}
float rand_smooth(float from, float to, float randX, float randY)
{
	return from + (to - from) * random(randX,randY).y;
}
float rand_smooth(float to, float randX, float randY)
{
	return to * random(randX,randY).y;
}

//called by a generated 'PS' shader
void PS_Method(
			float4 texRandIndex,
			float4 lifeIndex,
			float4 defaultPosition,
			float4 defaultVelocity,
			float4 defaultColour,
			float4 defaultUserData,
			out float4 posOut,
			out float4 velOut,
			out float4 colOut,
			out float4 userOut)
{
	float4 _positionSize			= defaultPosition;
	float4 _velocityRotation		= defaultVelocity;
	float4 _user					= defaultUserData;
	float4 _colour					= defaultColour;
	float4 _life					= lifeIndex;

	float2 index = texRandIndex.xy;
	float2 randIndex = texRandIndex.zw + constants.zw;

_#ifdef TEXTURE_PARTICLE_METHOD

	_positionSize			= tex2D(PositionSizeSampler,index);
	_velocityRotation		= tex2D(VelocityRotationSampler,index);

_#ifdef USER_COLOUR_TEX
	_colour				= tex2D(ColourSampler,index);
_#endif

_#ifdef USER_USER_TEX
	_user				= tex2D(UserSampler,index);
_#endif

_#ifdef USER_LIFE_TEX
	_life.xy			= tex2D(LifeSampler,index).xy;
_#endif

_#endif

	//most of this gets compiled out, unless used by the shader in the user block

	float local0=0,local1=0,local2=0,local3=0;
	float delta_time = constants2.z;

	float global0	= globals(0).x,  global1  = globals(0).y,  global2  = globals(0).z,  global3  = globals(0).w;
	float global4	= globals(1).x,  global5  = globals(1).y,  global6  = globals(1).z,  global7  = globals(1).w;
	float global8	= globals(2).x,  global9  = globals(2).y,  global10 = globals(2).z,  global11 = globals(2).w;
	float global12	= globals(3).x,  global13 = globals(3).y,  global14 = globals(3).z,  global15 = globals(3).w;

	float3 position	= _positionSize.xyz;
	float size		= _positionSize.w;

	float3 velocity	= _velocityRotation.xyz;
	float rotation	= _velocityRotation.w;

	float red		= _colour.r;
	float green		= _colour.g;
	float blue		= _colour.b;
	float alpha		= _colour.a;

	float life		= _life.x;
	float age		= constants.y - _life.y;
	
	if (age < 0)
		age			+= constants.x;

	float user0		= _user.x;
	float user1		= _user.y;
	float user2		= _user.z;
	float user3		= _user.w;

_#ifdef ADD_VELOCITY

	position += velocity * delta_time;

_#endif

_#ifdef POS_WRITTEN_TO_USER

	position.x += user1;
	position.y += user2;
	position.z += user3;

_#endif

//Begin user code

%%USER_CODE%%
//End user code

_#ifdef POS_WRITTEN_TO_USER

	position.x -= user1;
	position.y -= user2;
	position.z -= user3;

_#endif

	posOut = float4(position,size);
	velOut = float4(velocity,rotation);
	colOut = float4(red,green,blue,alpha);
	userOut = float4(user0,user1,user2,user3);
}


// vertex shader

_#ifdef VS_FRAME
float4 _vs_c[7] : register(vs,c0);
_#else
float4 _vs_c[247] : register(vs,c0);
_#endif

//defines to map the constants

_#define WorldViewProjection (transpose(float4x4(_vs_c[0],_vs_c[1],_vs_c[2],_vs_c[3])))
_#define targetSize (_vs_c[4])
_#define sourceSize (_vs_c[5])
_#define randSize (_vs_c[6])
_#define indices(IDX) (_vs_c[7+IDX])

/*
float4x4 WorldViewProjection : register(vs,c0);
float4 targetSize : register(vs,c4);
float4 sourceSize : register(vs,c5);
float4 randSize : register(vs,c6);
float4 indices[240] : register(vs,c7);
*/

float InvRandSize()
{
	return randSize.y;
}
float2 GetPosition(float2 invSize, float index, float offset)
{
	index *= invSize.x;
	return float2(fmod(index,1),(floor(index+1)+offset)*invSize.y);
}

void GenerateCommonData(float4 data, float index, out float4 positionOut, out float2 randCoordOut, out float2 lifeIndexOut, float offset)
{
	float positionIndex = data.x;

	lifeIndexOut = data.zw;
	
	positionOut    = float4(0,0,0,1);
	float2 positionValue = GetPosition(targetSize.zw,positionIndex, offset);
	positionOut.xy = positionValue * 2 - 1;

	float invRand = InvRandSize();
	randCoordOut = float2(index * invRand, index * invRand * invRand);
}

_#ifdef VS_FRAME

void VS(
		float4 position		: POSITION,
	out	float4 positionOut	: POSITION,
		float4 texCoord		: TEXCOORD0,
	out	float4 texRandOut	: TEXCOORD0)
{
	positionOut = mul(position, WorldViewProjection);
	texRandOut.xy = texCoord.xy;

	texRandOut.zw = texCoord.xy * (targetSize.xy * InvRandSize());
}

_#endif

_#ifdef VS_CLONE

//read from textures
void VS(
		float4 position     : POSITION, 
	out	float4 positionOut  : POSITION,
	out	float4 texRandOut   : TEXCOORD0,
	out	float4 lifeIndexOut : TEXCOORD1)
{
	float4 data;
	data = indices(position.x);

	float2 randCoordOut = 0;

	float2 lifeIndex = 0;
	GenerateCommonData(data, position.x, positionOut, randCoordOut, lifeIndex, position.w);
	lifeIndexOut = float4(lifeIndex,0,0);

	float textureIndex  = data.y;

	float2 texCoordOut = 0;
	texCoordOut    = GetPosition(sourceSize.zw,textureIndex, position.w);
	texCoordOut   += float2(0.5,-0.5) * sourceSize.zw;
	texCoordOut.y  = 1 - texCoordOut.y;

	texRandOut = float4(texCoordOut, randCoordOut);
}

_#endif

_#ifdef VS_ONCE

//read from constants
void VS(
		float4 position        : POSITION, 
	out	float4 positionOut     : POSITION,
	out	float4 texRandCoordOut : TEXCOORD0,
	out	float4 lifeIndexOut    : TEXCOORD1,
	out	float4 defaultPosition : TEXCOORD2,
	out	float4 defaultVelocity : TEXCOORD3,
	out	float4 defaultColour   : TEXCOORD4,
	out	float4 defaultUserData : TEXCOORD5)
{
	float4 data;
	int index = position.x;
	data = indices(index);

	int dataIndex = data.y;

	defaultPosition = indices(dataIndex + 0);
	defaultVelocity = indices(dataIndex + 1);
	defaultColour   = indices(dataIndex + 2);
	defaultUserData = indices(dataIndex + 3);

	data.y = 0;

	float2 randCoordOut = 0;

	float2 lifeIndex = 0;
	GenerateCommonData(data, position.x, positionOut, randCoordOut, lifeIndex, position.w);
	lifeIndexOut = float4(lifeIndex,0,0);

	texRandCoordOut = float4(0,0,randCoordOut);
}

_#endif
";

private static string techniqueCode =
@"

//technique code

technique Shader
{
	pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader = compile ps_3_0 PS();
	}
}
";

		#endregion

		private const string replaceMarker = @"%%USER_CODE%%";

		private static Dictionary<string, char> opTypes;

		public enum LogicType
		{
			Frame,
			FrameMove,
			Once,
			OnceClone,
		}
		public enum VertexShaderType
		{
			Once,
			Clone,
			Frame,
		}


		public static ParticleSystemCompiledShaderData BuildGpuLogicPixelShader(IEnumerable<ParticleSystemLogicStep> steps, LogicType logicType, VertexShaderType vsType, bool useUserValues, bool useColours, bool storeLifeData, ContentTargetPlatform targetPlatform, bool useUserDataPositionBuffer, string pathToShaderSystem)
		{
			if (steps == null)
				throw new ArgumentNullException();

			StringBuilder output = new StringBuilder();
			Random random = new Random();

			foreach (ParticleSystemLogicStep step in steps)
				BuildStep(step, output, 1, random);

			//fixup the preprocssor defines
			string shaderCodeFixup = GpuParticleShaderBuilder.BaseShader.Replace("_#", "#");

			//insert the custom code
			string shaderCode = shaderCodeFixup.Replace(GpuParticleShaderBuilder.replaceMarker, output.ToString());

			//build the shader header and main method.

			StringBuilder header = new StringBuilder();
			StringBuilder methodHeader = new StringBuilder();
			StringBuilder methodPS = new StringBuilder();

			methodPS.Append("void PS(float4 texRandIndex : TEXCOORD0");

			if (logicType != LogicType.Frame)
				methodPS.Append(", float4 lifeIndex : TEXCOORD1");
			else
				methodHeader.Append("float4 lifeIndex = 0;");

			if (logicType == LogicType.Once)
			{
				methodPS.Append(", float4 defaultPosition : TEXCOORD2");
				methodPS.Append(", float4 defaultVelocity : TEXCOORD3");
				methodPS.Append(", float4 defaultColour   : TEXCOORD4");
				methodPS.Append(", float4 defaultUserData : TEXCOORD5");
			}
			else
			{
				methodHeader.Append("float4 defaultPosition = 0, defaultVelocity = 0;");
				methodHeader.Append("float4 defaultColour = 1, defaultUserData = 0;");
			}

			int colIndex = 2;

			methodPS.Append(", out float4 posOut : COLOR0, out float4 velOut : COLOR1");


			if (useColours)
				methodPS.AppendFormat(", out float4 colOut : COLOR{0}", colIndex++);
			else
				methodHeader.Append("float4 colOut = 1;");

			if (useUserValues)
				methodPS.AppendFormat(", out float4 userOut : COLOR{0}", colIndex++);
			else
				methodHeader.Append("float4 userOut = 0;");

			methodPS.AppendLine(")");
			methodPS.AppendLine("{");
			methodPS.Append("\t");
			methodPS.Append(methodHeader);
			methodPS.AppendLine();

			methodPS.Append("\t");
			methodPS.AppendLine(@"PS_Method(texRandIndex,lifeIndex, defaultPosition, defaultVelocity, defaultColour, defaultUserData,posOut, velOut, colOut, userOut);");
			methodPS.AppendLine("}");

			int colourIndex = -1, userIndex = -1, lifeIndex = -1;

			if (logicType != LogicType.Once)
			{
				int samplerIndex = 3;
				header.AppendLine();
				header.AppendLine("#define TEXTURE_PARTICLE_METHOD");
				if (useColours)
				{
					colourIndex = samplerIndex;
					header.AppendLine("#define USER_COLOUR_TEX");
					header.AppendLine(string.Format(@"sampler2D ColourSampler : register(ps,s{0});", samplerIndex++));
				}
				if (useUserValues)
				{
					userIndex = samplerIndex;
					header.AppendLine("#define USER_USER_TEX");
					header.AppendLine(string.Format(@"sampler2D UserSampler : register(ps,s{0});", samplerIndex++));
				}
				if (storeLifeData && logicType == LogicType.Frame)
				{
					lifeIndex = samplerIndex;
					header.AppendLine("#define USER_LIFE_TEX");
					header.AppendLine(string.Format(@"sampler2D LifeSampler : register(ps,s{0});", samplerIndex++));
				}

				if (logicType == LogicType.Frame || logicType == LogicType.FrameMove)
					header.AppendLine("#define ADD_VELOCITY");
			}

			switch (vsType)
			{
				case VertexShaderType.Clone:
					header.AppendLine("#define VS_CLONE");
					break;
				case VertexShaderType.Frame:
					header.AppendLine("#define VS_FRAME");
					break;
				case VertexShaderType.Once:
					header.AppendLine("#define VS_ONCE");
					break;
			}

			if (useUserDataPositionBuffer)
			{
				header.AppendLine("#define POS_WRITTEN_TO_USER");
			}


			StringBuilder completeShader = new StringBuilder();

			completeShader.Append(header);
			completeShader.AppendLine();
			completeShader.Append(shaderCode);
			completeShader.AppendLine();
			completeShader.Append(methodPS);
			completeShader.AppendLine();
			completeShader.Append(techniqueCode);
			completeShader.AppendLine();


			if (ShaderCompileDelegate == null)
				LinkToShaderCompiler(pathToShaderSystem);

			string errors;
			byte[] shaderBytes = ShaderCompileDelegate(completeShader.ToString(), "", targetPlatform == ContentTargetPlatform.Xbox360, out errors);

			if (errors != null)
				throw new InvalidOperationException("GPU Particle System Pixel Shader failed to compile:" + Environment.NewLine + errors);

			return new ParticleSystemCompiledShaderData(shaderBytes, targetPlatform == ContentTargetPlatform.Xbox360, colourIndex, userIndex, lifeIndex);
		}

		//callback to compile a shader
		delegate byte[] CompileEffect(string source, string filename, bool xbox, out string errors);
		private static CompileEffect ShaderCompileDelegate;

		//XNA no longer lets you dynamically build shaders without the content pipeline.
		//This action is now embedded in an external xen assembly.
		private static void LinkToShaderCompiler(string pathToShaderSystem)
		{
			string filename = Path.Combine(pathToShaderSystem, "Xen.Graphics.ShaderSystem.EffectCompiler.dll");
			if (File.Exists(filename) == false)
				throw new NotSupportedException("Unable to find Xen EffectCompiler dll in: " + Path.GetFullPath(pathToShaderSystem));
			var asm = System.Reflection.Assembly.LoadFile(Path.GetFullPath(filename));
			if (asm == null)
				throw new NotSupportedException("The Particle System was unable to link to the Xen ShaderCompiler binary");
			var type = asm.GetType("Xen.Graphics.ShaderSystem.EffectCompiler");
			ShaderCompileDelegate = Delegate.CreateDelegate(typeof(CompileEffect), type.GetMethod("CompileEffect")) as CompileEffect;
		}



		private static void BuildStep(ParticleSystemLogicStep step, StringBuilder output, int depth, Random random)
		{
			for (int i = 0; i < depth; i++)
				output.Append('\t');


			if (opTypes == null)
			{
				opTypes = new Dictionary<string, char>();
				opTypes.Add("div", '/');
				opTypes.Add("add", '+');
				opTypes.Add("sub", '-');
				opTypes.Add("mul", '*');
			}
			if (step.children == null)
			{
				//this is a simple function
				switch (step.type)
				{
					case "set":
						output.Append(step.target);
						output.Append(" = ");
						output.Append(step.arg0);
						output.AppendLine(";");
						return;
					case "madd":
						output.Append(step.target);
						output.Append(" += ");
						output.Append(step.arg0);
						output.Append(" * ");
						output.Append(step.arg1);
						output.AppendLine(";");
						return;
					case "add":
					case "sub":
					case "mul":
					case "div":
						output.Append(step.target);
						output.Append(' ');
						if (step.arg1 == null)
						{
							output.Append(opTypes[step.type]);
							output.Append("= ");
							output.Append(step.arg0);
						}
						else
						{
							output.Append("= ");
							output.Append(step.arg0);
							output.Append(' ');
							output.Append(opTypes[step.type]);
							output.Append(' ');
							output.Append(step.arg1);
						}
						output.AppendLine(";");
						return;
					default:
						//function
						output.Append(step.target);
						output.Append(" = ");
						output.Append(step.type);
						output.Append('(');
						output.Append(step.arg0);
						if (step.arg1 != null)
						{
							output.Append(',');
							output.Append(step.arg1);
						}
						if (step.type.StartsWith("rand"))
						{
							//add two static-random numbers onto the end of the function call
							output.Append(", randIndex.x + ");
							output.Append(random.NextDouble());
							output.Append(", randIndex.y + ");
							output.Append(random.NextDouble());
						}
						output.AppendLine(");");
						return;
				}
			}
			else
			{
				//branching operations
				switch (step.type)
				{
					case "loop":

						//for loop
						output.Append("for (int i");
						output.Append(depth);
						output.Append("; i");
						output.Append(depth);
						output.Append(" < ");
						output.Append(step.arg0);
						output.Append("; i");
						output.Append(depth);
						output.AppendLine("++)");
						// {
						for (int i = 0; i < depth; i++)
							output.Append('\t');
						output.AppendLine("{");

						//children
						for (int i = 0; i < step.children.Length; i++)
							BuildStep(step.children[i], output, depth + 1, random);

						// }
						for (int i = 0; i < depth; i++)
							output.Append('\t');
						output.AppendLine("}");

						return;

					case "if_equal":
					case "if_notequal":
					case "if_lessequal":
					case "if_greaterequal":
					case "if_less":
					case "if_greater":

						//if (
						output.Append("if (");
						output.Append(step.arg0);
						output.Append(' ');

						//operator
						if (step.type == "if_equal") output.Append("==");
						if (step.type == "if_notequal") output.Append("!=");
						if (step.type == "if_lessequal") output.Append("<=");
						if (step.type == "if_greaterequal") output.Append(">=");
						if (step.type == "if_less") output.Append('<');
						if (step.type == "if_greater") output.Append('>');

						output.Append(' ');
						output.Append(step.arg1);
						output.AppendLine(")");

						// {
						for (int i = 0; i < depth; i++)
							output.Append('\t');
						output.AppendLine("{");

						//children
						for (int i = 0; i < step.children.Length; i++)
							BuildStep(step.children[i], output, depth + 1,random);

						// }
						for (int i = 0; i < depth; i++)
							output.Append('\t');
						output.AppendLine("}");

						return;
					}	
			}
		}
	}

#endif
}
