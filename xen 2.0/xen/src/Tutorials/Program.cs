using System;
using System.Collections.Generic;
using Xen;



namespace Tutorials
{
	static class Program
	{
		static void Main(string[] args)
		{
#if XBOX360
			int selection = 0;
			while (true)
			{
				Type tutorial = null;
				using (var menu = new XenMenu.XenMenuApp(selection))
				{
					menu.Run();

					if (menu.SelectedTutorial != null)
					{
						selection = menu.SelectedIndex + 1;
						tutorial = menu.SelectedTutorial;
					}
				}

				GC.Collect();

				if (tutorial != null)
				{
					using (Application app = Activator.CreateInstance(tutorial) as Application)
						app.Run();
				}
				else
					break;
			}
#else
			while (true)
			{
				bool runInWinForms;
				bool runInXnaGame;

				Type type = TutorialChooser.ChooseTutorial(out runInWinForms, out runInXnaGame);
				if (type == null)
					break;

				using (Application tutorial = Activator.CreateInstance(type) as Application)
				{
					tutorial.Run();
				}
			}
#endif
		}


		//helper code for the tutorial selection
		private static int SortItems(KeyValuePair<string, Type> a, KeyValuePair<string, Type> b)
		{
			return a.Key.CompareTo(b.Key);
		}

		public static void FindTutorials(Dictionary<string, Type> types)
		{
			//work around the xbox's lack of SortedList / SortedDictionary
			var items = new List<KeyValuePair<string, Type>>();

			foreach (Type type in typeof(Program).Assembly.GetExportedTypes())
			{
#if XBOX360
				//hack! :-)
				if (type == typeof(Tutorials.XenMenu.XenMenuApp))
					continue;
#endif

				if (typeof(Xen.Application).IsAssignableFrom(type))
				{
					string name = type.FullName;
					foreach (object attribute in type.GetCustomAttributes(typeof(DisplayNameAttribute), true))
						name = ((DisplayNameAttribute)attribute).Name;

					items.Add(new KeyValuePair<string, Type>(name, type));
				}
			}

			items.Sort(SortItems);
			foreach (var item in items)
			{
				types.Add(item.Key, item.Value);
			}
		}
	}


	public class DisplayNameAttribute : Attribute
	{
		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}

