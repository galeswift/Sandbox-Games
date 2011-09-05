using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content;
using Xen.Ex.Graphics.Content;
using System.Reflection;
using System.ComponentModel;
using System.Xml;

namespace Xen.Ex.ModelImporter
{

	[ContentTypeWriter]
	class ParticleSystemDataWriter : ContentTypeWriter<ParticleSystemData>
	{
		protected override void Write(ContentWriter output, ParticleSystemData value)
		{
			if (output.TargetPlatform == TargetPlatform.WindowsPhone)
				throw new InvalidContentException("Windows phone is not supported");
			value.Write(output, output.TargetPlatform == TargetPlatform.Windows ? ContentTargetPlatform.Windows : ContentTargetPlatform.Xbox360);
		}

		public override string GetRuntimeReader(TargetPlatform targetPlatform)
		{
			return (string)typeof(ParticleSystemData).GetProperty("RuntimeReaderType", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, new object[0]);
		}
	}

	public sealed class ParticleSystemXmlData
	{
		public readonly byte[] XmlData;
		public readonly string fileName;

		public ParticleSystemXmlData(byte[] data, string file)
		{
			this.fileName = file;
			this.XmlData = data;
		}
	}

	[ContentImporter(".particles", DisplayName = "ParticleSystem Importer - Xen", DefaultProcessor = "ParticleSystem - Xen")]
	public sealed class ParticleSystemDataImporter : ContentImporter<ParticleSystemXmlData>
	{
		public override ParticleSystemXmlData Import(string filename, ContentImporterContext context)
		{
			ParticleSystemXmlData xml = new ParticleSystemXmlData(File.ReadAllBytes(filename), new FileInfo(filename).FullName);
			return xml;
		}
	}


	[ContentProcessor(DisplayName = "ParticleSystem - Xen")]
	public sealed class ParticleSystemDataProcessor : ContentProcessor<ParticleSystemXmlData, ParticleSystemData>
	{
		private ContentProcessorContext context;
		private string rootPath;

		public override ParticleSystemData Process(ParticleSystemXmlData input, ContentProcessorContext context)
		{
			try
			{
				return BuildParticleSystem(input, context);
			}
			catch (Exception e)
			{
				throw new InvalidContentException(e.Message, new ContentIdentity(input.fileName));
			}
		}

		private ParticleSystemData BuildParticleSystem(ParticleSystemXmlData input, ContentProcessorContext context)
		{
			this.context = context;
			this.rootPath = input.fileName;

			MemoryStream stream = new MemoryStream(input.XmlData);

			FileInfo asmFile = new FileInfo(Assembly.GetExecutingAssembly().Location);
			string schema = asmFile.Directory.FullName + @"/particlesystem.xsd";

			if (!File.Exists(schema))
				throw new FileNotFoundException(schema);

			XmlDocument doc = new XmlDocument();
			doc.Load(stream);
			XmlElement root = doc.SelectSingleNode("particlesystem") as XmlElement;

			if (root == null)
				throw new FormatException("XML data does not contain root element \'particlesystem\'");

			using (Stream file = File.OpenRead(schema))
				doc.Schemas.Add(System.Xml.Schema.XmlSchema.Read(file, ValidationEvent));

			doc.Validate(ValidationEvent, root);


			bool includeLogicTypeData = true;

			//load all the textures

			string shaderSystemPath = Path.GetDirectoryName(typeof(Xen.Graphics.ShaderSystem.EffectCompiler).Assembly.Location);
			ParticleSystemData data = new ParticleSystemData(input.fileName, root, context.TargetPlatform == TargetPlatform.Windows ? ContentTargetPlatform.Windows : ContentTargetPlatform.Xbox360, includeLogicTypeData, BuildTexture, shaderSystemPath);

			return data;
		}

		private static void ValidationEvent(object sender, System.Xml.Schema.ValidationEventArgs args)
		{
			throw new FormatException("Particle System XML data failed Schema Validation\n" + args.Exception.ToString());
		}


		#region options


		private bool importTextures = true;
		private bool colourKeyEnabled = false, premultiplyAlpha = true;
		private Color colourKeyColour = Color.Pink;
		private TextureProcessorOutputFormat destinationFormat = TextureProcessorOutputFormat.DxtCompressed;
		private bool generateMipmaps = true;
		private bool resizeTexturesToPowerOfTwo = true;
		private Dictionary<string, string> loadedTextures = new Dictionary<string, string>();


		private string BuildTexture(string textureName)
		{
			if (textureName == null || textureName.Length == 0)
				return "";

			string assetLocation = new FileInfo(rootPath).DirectoryName + @"/" + textureName;
			string comparePath = rootPath;

			if (importTextures)
			{
				OpaqueDataDictionary processorParameters = new OpaqueDataDictionary();
				processorParameters.Add("ColorKeyColor", this.colourKeyColour);
				processorParameters.Add("ColorKeyEnabled", this.ColorKeyEnabled);
				processorParameters.Add("PremultiplyAlpha", this.PremultiplyAlpha);
				processorParameters.Add("TextureFormat", this.TextureFormat);
				processorParameters.Add("GenerateMipmaps", this.GenerateMipmaps);
				processorParameters.Add("ResizeToPowerOfTwo", this.ResizeTexturesToPowerOfTwo);

				ExternalReference<TextureContent> texture = context.BuildAsset<TextureContent, TextureContent>(new ExternalReference<TextureContent>(textureName, new ContentIdentity(rootPath)), typeof(TextureProcessor).Name, processorParameters, null, null);
				assetLocation = texture.Filename;
				comparePath = context.OutputFilename;
			}
			else
			{
				if (!File.Exists(assetLocation))
				{
					context.Logger.LogWarning("", new ContentIdentity(rootPath), "File not found:\t{1} ({0})", textureName, @".\" + GetSharedPath(assetLocation, comparePath) + new FileInfo(assetLocation).Extension);
					return "";
				}
			}

			return GetSharedPath(assetLocation, comparePath);
		}

		static string GetSharedPath(string filePath, string rootPath)
		{
			FileInfo file = new FileInfo(filePath);
			FileInfo root = new FileInfo(rootPath);

			DirectoryInfo fileDir = file.Directory;
			DirectoryInfo rootDir = root.Directory;
			int subDirs = 0;

			while (fileDir.Parent != null && rootDir.Parent != null)
			{
				if (fileDir.FullName == rootDir.FullName)
				{
					string path = Path.ChangeExtension(file.FullName.Substring(fileDir.FullName.Length), null);
					while (subDirs-- > 0)
					{
						if (subDirs != 0)
							path = @"\" + path;
						path = @".." + path;
					}
					if (path.StartsWith(@"\"))
						path = path.Substring(1);
					return path;
				}
				if (fileDir.Parent == null)
				{
					rootDir = rootDir.Parent;
					subDirs++;
					continue;
				}
				if (rootDir.Parent == null)
				{
					fileDir = fileDir.Parent;
					continue;
				}
				if (fileDir.FullName.Length > rootDir.FullName.Length)
				{
					fileDir = fileDir.Parent;
				}
				else
				{
					rootDir = rootDir.Parent;
					subDirs++;
				}
			}
			return filePath;
		}


		[DefaultValue(false), DisplayName("Manual texture import"), Category("Textures")]
		public bool ManualTextureImport
		{
			get
			{
				return !this.importTextures;
			}
			set
			{
				this.importTextures = !value;
			}
		}


		[DefaultValue(TextureProcessorOutputFormat.DxtCompressed), DisplayName("Texture format"), Category("Textures")]
		public TextureProcessorOutputFormat TextureFormat
		{
			get
			{
				return this.destinationFormat;
			}
			set
			{
				this.destinationFormat = value;
			}
		}


		[DefaultValue(typeof(Color), "255, 0, 255, 255"), DisplayName("Colour Key Colour"), Category("Textures")]
		public Color ColorKeyColour
		{
			get
			{
				return this.colourKeyColour;
			}
			set
			{
				this.colourKeyColour = value;
			}
		}

		[DefaultValue(true), DisplayName("Premultiply Alpha"), Category("Textures")]
		public bool PremultiplyAlpha
		{
			get
			{
				return this.premultiplyAlpha;
			}
			set
			{
				this.premultiplyAlpha = value;
			}
		}

		[DefaultValue(true), DisplayName("Colour Key Enabled"), Category("Textures")]
		public bool ColorKeyEnabled
		{
			get
			{
				return this.colourKeyEnabled;
			}
			set
			{
				this.colourKeyEnabled = value;
			}
		}

		[DefaultValue(true), DisplayName("Generate mipmaps"), Category("Textures")]
		public bool GenerateMipmaps
		{
			get
			{
				return this.generateMipmaps;
			}
			set
			{
				this.generateMipmaps = value;
			}
		}

		[DefaultValue(true), DisplayName("Resize textures to power of two"), Category("Textures")]
		public bool ResizeTexturesToPowerOfTwo
		{
			get
			{
				return this.resizeTexturesToPowerOfTwo;
			}
			set
			{
				this.resizeTexturesToPowerOfTwo = value;
			}
		}

		#endregion
	}


}