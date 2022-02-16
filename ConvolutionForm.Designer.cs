
namespace NeuralNet
{
	partial class ConvolutionForm
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
			this.pBoxImage = new System.Windows.Forms.PictureBox();
			this.pBoxKernel = new System.Windows.Forms.PictureBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pBoxFeatureMap = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnAuto = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.pBoxKernelB = new System.Windows.Forms.PictureBox();
			this.pBoxKernelG = new System.Windows.Forms.PictureBox();
			this.pBoxKernelR = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.pictureBox5 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pBoxImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernel)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pBoxFeatureMap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernelB)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernelG)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernelR)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
			this.SuspendLayout();
			// 
			// pBoxImage
			// 
			this.pBoxImage.Location = new System.Drawing.Point(13, 46);
			this.pBoxImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pBoxImage.Name = "pBoxImage";
			this.pBoxImage.Size = new System.Drawing.Size(192, 192);
			this.pBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pBoxImage.TabIndex = 0;
			this.pBoxImage.TabStop = false;
			// 
			// pBoxKernel
			// 
			this.pBoxKernel.Location = new System.Drawing.Point(208, 208);
			this.pBoxKernel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pBoxKernel.Name = "pBoxKernel";
			this.pBoxKernel.Size = new System.Drawing.Size(48, 48);
			this.pBoxKernel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pBoxKernel.TabIndex = 1;
			this.pBoxKernel.TabStop = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1091, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// pBoxFeatureMap
			// 
			this.pBoxFeatureMap.Location = new System.Drawing.Point(259, 46);
			this.pBoxFeatureMap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pBoxFeatureMap.Name = "pBoxFeatureMap";
			this.pBoxFeatureMap.Size = new System.Drawing.Size(192, 192);
			this.pBoxFeatureMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pBoxFeatureMap.TabIndex = 3;
			this.pBoxFeatureMap.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(75, 256);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 15);
			this.label1.TabIndex = 4;
			this.label1.Text = "Input";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(208, 259);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 15);
			this.label2.TabIndex = 5;
			this.label2.Text = "Filter";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(305, 256);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 15);
			this.label3.TabIndex = 6;
			this.label3.Text = "Feature Map R";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(197, 354);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(75, 23);
			this.btnReset.TabIndex = 7;
			this.btnReset.Text = "Reset";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnAuto
			// 
			this.btnAuto.Location = new System.Drawing.Point(116, 354);
			this.btnAuto.Name = "btnAuto";
			this.btnAuto.Size = new System.Drawing.Size(75, 23);
			this.btnAuto.TabIndex = 8;
			this.btnAuto.Text = "Auto";
			this.btnAuto.UseVisualStyleBackColor = true;
			this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(35, 354);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 9;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// pBoxKernelB
			// 
			this.pBoxKernelB.Location = new System.Drawing.Point(208, 154);
			this.pBoxKernelB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pBoxKernelB.Name = "pBoxKernelB";
			this.pBoxKernelB.Size = new System.Drawing.Size(48, 48);
			this.pBoxKernelB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pBoxKernelB.TabIndex = 10;
			this.pBoxKernelB.TabStop = false;
			// 
			// pBoxKernelG
			// 
			this.pBoxKernelG.Location = new System.Drawing.Point(208, 100);
			this.pBoxKernelG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pBoxKernelG.Name = "pBoxKernelG";
			this.pBoxKernelG.Size = new System.Drawing.Size(48, 48);
			this.pBoxKernelG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pBoxKernelG.TabIndex = 11;
			this.pBoxKernelG.TabStop = false;
			// 
			// pBoxKernelR
			// 
			this.pBoxKernelR.Location = new System.Drawing.Point(208, 46);
			this.pBoxKernelR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pBoxKernelR.Name = "pBoxKernelR";
			this.pBoxKernelR.Size = new System.Drawing.Size(48, 48);
			this.pBoxKernelR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pBoxKernelR.TabIndex = 12;
			this.pBoxKernelR.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(459, 46);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(192, 192);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 16;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Location = new System.Drawing.Point(659, 46);
			this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(192, 192);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 17;
			this.pictureBox2.TabStop = false;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(510, 256);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(84, 15);
			this.label7.TabIndex = 18;
			this.label7.Text = "Feature Map G";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(709, 256);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(83, 15);
			this.label8.TabIndex = 19;
			this.label8.Text = "Feature Map B";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(937, 28);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 15);
			this.label4.TabIndex = 21;
			this.label4.Text = "Max Pooled";
			// 
			// pictureBox3
			// 
			this.pictureBox3.Location = new System.Drawing.Point(883, 46);
			this.pictureBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(192, 192);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox3.TabIndex = 20;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Location = new System.Drawing.Point(883, 244);
			this.pictureBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(192, 192);
			this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox4.TabIndex = 22;
			this.pictureBox4.TabStop = false;
			// 
			// pictureBox5
			// 
			this.pictureBox5.Location = new System.Drawing.Point(883, 442);
			this.pictureBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(192, 192);
			this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox5.TabIndex = 23;
			this.pictureBox5.TabStop = false;
			// 
			// ConvolutionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1091, 650);
			this.Controls.Add(this.pictureBox5);
			this.Controls.Add(this.pictureBox4);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.pBoxKernelR);
			this.Controls.Add(this.pBoxKernelG);
			this.Controls.Add(this.pBoxKernelB);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.btnAuto);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pBoxFeatureMap);
			this.Controls.Add(this.pBoxKernel);
			this.Controls.Add(this.pBoxImage);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.Name = "ConvolutionForm";
			this.Text = "ConvolutionForm";
			((System.ComponentModel.ISupportInitialize)(this.pBoxImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernel)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pBoxFeatureMap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernelB)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernelG)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pBoxKernelR)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pBoxImage;
		private System.Windows.Forms.PictureBox pBoxKernel;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.PictureBox pBoxFeatureMap;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnAuto;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.PictureBox pBoxKernelB;
		private System.Windows.Forms.PictureBox pBoxKernelG;
		private System.Windows.Forms.PictureBox pBoxKernelR;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.PictureBox pictureBox5;
	}
}