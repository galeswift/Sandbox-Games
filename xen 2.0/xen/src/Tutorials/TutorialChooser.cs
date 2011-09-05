using System;
using System.Collections.Generic;

using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tutorials
{
	public partial class TutorialChooser : Form
	{
		bool canceled = true;

		public string SelectedTutorial
		{
			get { return (tutorialList.SelectedItem ?? "").ToString(); }
		}

		public bool Canceled
		{
			get { return canceled; }
		}

		public TutorialChooser(IEnumerable<string> tutorialNames, string startName)
		{
			Cursor.Show();

			InitializeComponent();

			int selectIndex = 0;
			int i = 0;

			foreach (string str in tutorialNames)
			{
				i++;
				if (startName == str)
					selectIndex = i;

				tutorialList.Items.Add(str);
			}

			if (tutorialList.Items.Count > 0)
				tutorialList.SelectedIndex = Math.Min(tutorialList.Items.Count-1,selectIndex);
		}

		private void OK(object sender, EventArgs e)
		{
			Cursor.Hide();

			canceled = false;
			Close();
		}

		private void Cancel(object sender, EventArgs e)
		{
			Close();
		}

		public static Type ChooseTutorial(out bool useWinForms, out bool useXnaGame)
		{
			useWinForms = false;
			useXnaGame = false;

			Dictionary<string, Type> tutorials = new Dictionary<string, Type>();
			Program.FindTutorials(tutorials);

			TutorialChooser chooser = new TutorialChooser(tutorials.Keys, chosenTutorial);
			chooser.ShowDialog();

			if (chooser.Canceled)
				return null;

			Type type;
			tutorials.TryGetValue(chooser.SelectedTutorial, out type);

			chosenTutorial = chooser.SelectedTutorial;
			useWinForms = chooser.winformsHost.Checked;
			useXnaGame = chooser.xnaGameHost.Checked;
			return type;
		}

		static string chosenTutorial;
	}


}