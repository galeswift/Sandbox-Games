using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

namespace XenFxRegistrySetup
{

	static class Program
	{
		private static void Main(string[] args)
		{
			string dll = "", name = "";
			bool remove = false, quiet = false;
			try
			{
				if (args.Length < 3)
				{
					Console.WriteLine("Usage: register.exe filename.dll COMname GUID [remove]");

					Console.WriteLine("Press any key to continue...");
					Console.ReadKey();
					return;
				}

				dll = args[0];//bin/XnaPlus.Effect.dll
				name = args[1];//XnaFX
				string guid = args[2];//{F3829EFE-0362-4FE4-928A-7784A6334E76}

				if (File.Exists(dll) == false)
				{
					Console.WriteLine("File: " + dll + " Does not exist");
					MessageBox.Show("File: " + dll + " Does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					Console.WriteLine("Press any key to continue...");
					Console.ReadKey();
					return;
				}

				if (args.Length > 3 && args[3].Contains("remove"))
					remove = true;

				if (args.Length > 3 && args[3].Contains("quiet"))
					quiet = true;

				string[] reg = new string[]
				{
					@"SOFTWARE\Microsoft\VCSExpress\10.0\Generators\",
					@"SOFTWARE\Microsoft\VBExpress\10.0\Generators\",
					@"SOFTWARE\Microsoft\VisualStudio\10.0\Generators\",
					//x64
					@"SOFTWARE\Wow6432Node\Microsoft\VCSExpress\10.0\Generators\",
					@"SOFTWARE\Wow6432Node\Microsoft\VBExpress\10.0\Generators\",
					@"SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\Generators\",
				};
		
				try
				{
					foreach (string rootkey in reg)
					{
						RegistryKey key = Registry.LocalMachine.OpenSubKey(rootkey);
						if (key == null)
							continue;

						string[] names = key.GetSubKeyNames();

						foreach (string keyname in names)
						{
							RegistryKey sub = key.OpenSubKey(keyname, true);

							string[] subs = sub.GetSubKeyNames();

							foreach (string subkeyname in subs)
							{
								if (subkeyname == name)
								{
									sub.DeleteSubKey(name);
								}
							}

							if (!remove)
							{
								RegistryKey namedKey = sub.CreateSubKey(name);
								namedKey.SetValue("CLSID", guid, RegistryValueKind.String);
								namedKey.SetValue("GeneratesDesignTimeSource", (int)1, RegistryValueKind.DWord);
								namedKey.Close();
							}

							sub.Close();
						}
					}
				}
				catch (Exception e)
				{
					string error = "An error occured while writing registry data" + Environment.NewLine + "(This application may require administrator privaleges to run properly)" + Environment.NewLine + Environment.NewLine + e.ToString();
					MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					Console.WriteLine(error);

					Console.WriteLine("Press any key to continue...");
					Console.ReadKey();
					return;
				}


				//I have no idea why this is occasionally needed...?
				System.AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

				Assembly asm = Assembly.LoadFrom(Path.GetFullPath(dll));

				bool complete;
				RegistrationServices register = new RegistrationServices();
				if (remove)
					complete = register.UnregisterAssembly(asm);
				else
					complete = register.RegisterAssembly(asm, AssemblyRegistrationFlags.SetCodeBase);


				if (complete)
					Console.WriteLine("Operation complete");
				else
				{
					if (remove)
						Console.WriteLine("Operation failed (Type may already be removed)");
					else
						Console.WriteLine("Operation failed");
				}

				if (!quiet)
				{
					Console.WriteLine("Press any key to continue...");
					Console.ReadKey();
				}

			}
			catch (Exception ex)
			{
				if (!quiet)
				System.Windows.Forms.MessageBox.Show(
					string.Format(@"Failed to {0} assembly {1}

If the .dll is already registered, it may need to be manually removed from the GAC.
To do this, in Explorer, navigate to %windir%\assembly

Find the assembly in the list, and manually remove it by right clicking and selecting 'Uninstall'

Exception:
{2}", remove ? "unregister" : "register", dll, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				Console.WriteLine(@"{2}: Failed to {0} assembly {1}", remove ? "unregister" : "register", dll, typeof(Program).Assembly.ManifestModule.Name);
				Console.Error.WriteLine(@"Failed to {0} assembly {1}", remove ? "unregister" : "register", dll);
			}
		}


		//no idea why this is needed sometimes. really,
		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("Microsoft.VisualStudio.BaseCodeGeneratorWithSite"))
			{
				return Assembly.LoadFrom("Microsoft.VisualStudio.BaseCodeGeneratorWithSite.dll");
			}
			return null;
		}
	}
}