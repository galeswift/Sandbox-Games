namespace Tutorials
{
	partial class WinFormsExample
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Panel panel1;
			this.xenFormsHost = new Xen.WinFormsHostControl();
			groupBox1 = new System.Windows.Forms.GroupBox();
			panel1 = new System.Windows.Forms.Panel();
			groupBox1.SuspendLayout();
			panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this.xenFormsHost);
			groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox1.Location = new System.Drawing.Point(8, 8);
			groupBox1.Name = "groupBox1";
			groupBox1.Padding = new System.Windows.Forms.Padding(9, 3, 9, 9);
			groupBox1.Size = new System.Drawing.Size(968, 488);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Xen WinFormsHost";
			// 
			// xenFormsHost
			// 
			this.xenFormsHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xenFormsHost.Location = new System.Drawing.Point(9, 16);
			this.xenFormsHost.Name = "xenFormsHost";
			this.xenFormsHost.Size = new System.Drawing.Size(950, 463);
			this.xenFormsHost.TabIndex = 0;
			this.xenFormsHost.Text = "XenHost";
			// 
			// panel1
			// 
			panel1.Controls.Add(groupBox1);
			panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Padding = new System.Windows.Forms.Padding(8);
			panel1.Size = new System.Drawing.Size(984, 504);
			panel1.TabIndex = 1;
			// 
			// WinFormsExample
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(984, 504);
			this.Controls.Add(panel1);
			this.Name = "WinFormsExample";
			this.Text = "WinFormsExample";
			groupBox1.ResumeLayout(false);
			panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Xen.WinFormsHostControl xenFormsHost;
	}
}