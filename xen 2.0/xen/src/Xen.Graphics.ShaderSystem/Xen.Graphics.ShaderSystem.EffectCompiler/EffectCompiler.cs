using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Xen.Graphics.ShaderSystem
{
	//wrapper class on XNA content pipeline compiler
	public static class EffectCompiler
	{
		public static byte[] CompileEffect(string source, string filename, bool buildForXbox, out string errors)
		{
			errors = null;
			try
			{
				var compiledEffect = BuildEffect(source, filename, buildForXbox ? TargetPlatform.Xbox360 : TargetPlatform.Windows);

				return compiledEffect.GetEffectCode();
			}
			catch (Exception e)
			{
				errors = e.Message;
				return null;
			}
		}

		static Microsoft.Xna.Framework.Content.Pipeline.Processors.CompiledEffectContent BuildEffect(string source, string filename, TargetPlatform platform)
		{
			var processor = new Microsoft.Xna.Framework.Content.Pipeline.Processors.EffectProcessor();
			processor.DebugMode = Microsoft.Xna.Framework.Content.Pipeline.Processors.EffectProcessorDebugMode.Optimize;

			var content = new Microsoft.Xna.Framework.Content.Pipeline.Graphics.EffectContent();
			content.EffectCode = source;
			content.Identity = new ContentIdentity(filename);

			var context = new EffectProcessorContext(platform);

			return processor.Process(content, context);
		}

		class EffectLogger : ContentBuildLogger
		{
			public override void LogMessage(string message, params object[] messageArgs) { }
			public override void LogImportantMessage(string message, params object[] messageArgs) { }
			public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs) { }
		}
		class EffectProcessorContext : ContentProcessorContext
		{
			private TargetPlatform platform;
			public EffectProcessorContext(TargetPlatform platform)
			{
				this.platform = platform;
			}

			public override TargetPlatform TargetPlatform { get { return platform; } }
			public override GraphicsProfile TargetProfile { get { return GraphicsProfile.HiDef; } }
			public override string BuildConfiguration { get { return string.Empty; } }
			public override string IntermediateDirectory { get { return string.Empty; } }
			public override string OutputDirectory { get { return string.Empty; } }
			public override string OutputFilename { get { return string.Empty; } }

			public override OpaqueDataDictionary Parameters { get { return parameters; } }
			OpaqueDataDictionary parameters = new OpaqueDataDictionary();

			public override ContentBuildLogger Logger { get { return logger; } }
			ContentBuildLogger logger = new EffectLogger();

			public override void AddDependency(string filename) { }
			public override void AddOutputFile(string filename) { }

			public override TOutput Convert<TInput, TOutput>(TInput input, string processorName, OpaqueDataDictionary processorParameters) { throw new NotImplementedException(); }
			public override TOutput BuildAndLoadAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName) { throw new NotImplementedException(); }
			public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName, string assetName) { throw new NotImplementedException(); }
		}
	}

}
