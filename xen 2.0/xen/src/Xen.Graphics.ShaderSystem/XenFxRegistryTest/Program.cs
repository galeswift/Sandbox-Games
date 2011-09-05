using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace XenFxRegistryTest
{
	static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
#if DEBUG
			try
#endif
			{
				if (args.Length != 4)
				{
					Console.WriteLine("Usage: XenFxRegistryTest.exe registerApp.exe filename.dll COMname GUID");

					Console.WriteLine("Press any key to continue...");
					Console.ReadKey();
					return;
				}

				Type comType = null;
				try
				{
					comType = Type.GetTypeFromCLSID(new Guid(args[3]));
				}
				catch { }

				if (comType != null)
				{
					if (Path.GetFullPath(comType.Assembly.Location) == Path.GetFullPath(args[1]) &&
						File.Exists(comType.Assembly.Location))
					{
						Console.WriteLine("Skipping registration (File is already registered)...");
						return;
					}
				}

				bool exists = File.Exists(args[0]);

				ProcessStartInfo proc = new ProcessStartInfo(args[0]);
				proc.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" quiet", args[1], args[2], args[3]);

				Process.Start(proc).WaitForExit();
			}
#if DEBUG
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString());
				throw;
			}
#endif
		}
	}
}