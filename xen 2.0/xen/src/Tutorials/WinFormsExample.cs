using System.Data;
using System.Windows.Forms;

namespace Tutorials
{
	public partial class WinFormsExample : Form
	{
		public WinFormsExample()
		{
			InitializeComponent();
		}

		public Xen.WinFormsHostControl XenWinFormsHostControl
		{
			get { return this.xenFormsHost; }
		}
	}
}
