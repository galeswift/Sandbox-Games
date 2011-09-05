namespace Tutorials
{
	partial class TutorialChooser
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
			System.Windows.Forms.Button button1;
			System.Windows.Forms.Button button2;
			System.Windows.Forms.RadioButton radioButton1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TutorialChooser));
			this.tutorialList = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.xnaGameHost = new System.Windows.Forms.RadioButton();
			this.winformsHost = new System.Windows.Forms.RadioButton();
			button1 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			radioButton1 = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			button1.Location = new System.Drawing.Point(320, 17);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 0;
			button1.Text = "&OK";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(this.OK);
			// 
			// button2
			// 
			button2.Location = new System.Drawing.Point(320, 46);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(75, 23);
			button2.TabIndex = 3;
			button2.Text = "&Cancel";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(this.Cancel);
			// 
			// radioButton1
			// 
			radioButton1.AutoSize = true;
			radioButton1.Checked = true;
			radioButton1.Location = new System.Drawing.Point(22, 46);
			radioButton1.Name = "radioButton1";
			radioButton1.Size = new System.Drawing.Size(99, 17);
			radioButton1.TabIndex = 6;
			radioButton1.TabStop = true;
			radioButton1.Text = "Xen Application";
			radioButton1.UseVisualStyleBackColor = true;
			radioButton1.Visible = false;
			// 
			// tutorialList
			// 
			this.tutorialList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tutorialList.FormattingEnabled = true;
			this.tutorialList.Location = new System.Drawing.Point(22, 19);
			this.tutorialList.Name = "tutorialList";
			this.tutorialList.Size = new System.Drawing.Size(292, 21);
			this.tutorialList.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(radioButton1);
			this.groupBox1.Controls.Add(this.xnaGameHost);
			this.groupBox1.Controls.Add(this.winformsHost);
			this.groupBox1.Controls.Add(button2);
			this.groupBox1.Controls.Add(button1);
			this.groupBox1.Controls.Add(this.tutorialList);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(5, 5);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(401, 117);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select a tutorial to run:";
			// 
			// xnaGameHost
			// 
			this.xnaGameHost.AutoSize = true;
			this.xnaGameHost.Location = new System.Drawing.Point(22, 80);
			this.xnaGameHost.Name = "xnaGameHost";
			this.xnaGameHost.Size = new System.Drawing.Size(192, 17);
			this.xnaGameHost.TabIndex = 5;
			this.xnaGameHost.Text = "XNA GameComponent host (BETA)";
			this.xnaGameHost.UseVisualStyleBackColor = true;
			this.xnaGameHost.Visible = false;
			// 
			// winformsHost
			// 
			this.winformsHost.AutoSize = true;
			this.winformsHost.Location = new System.Drawing.Point(22, 62);
			this.winformsHost.Name = "winformsHost";
			this.winformsHost.Size = new System.Drawing.Size(132, 17);
			this.winformsHost.TabIndex = 4;
			this.winformsHost.Text = "WinForms host (BETA)";
			this.winformsHost.UseVisualStyleBackColor = true;
			this.winformsHost.Visible = false;
			// 
			// TutorialChooser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(411, 127);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TutorialChooser";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tutorial Chooser";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox tutorialList;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton xnaGameHost;
		private System.Windows.Forms.RadioButton winformsHost;
	}
}