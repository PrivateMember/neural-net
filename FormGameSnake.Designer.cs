
namespace NeuralNet
{
	partial class FormGameSnake
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnMove = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnAuto = new System.Windows.Forms.Button();
			this.labelMaxL = new System.Windows.Forms.Label();
			this.btnLoad = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSaveTrainData = new System.Windows.Forms.Button();
			this.btnLoadTrainData = new System.Windows.Forms.Button();
			this.labelDataL = new System.Windows.Forms.Label();
			this.labelMoveCount = new System.Windows.Forms.Label();
			this.labelMeanErr = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(709, 9);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(512, 512);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// btnMove
			// 
			this.btnMove.Location = new System.Drawing.Point(23, 12);
			this.btnMove.Name = "btnMove";
			this.btnMove.Size = new System.Drawing.Size(75, 23);
			this.btnMove.TabIndex = 1;
			this.btnMove.Text = "Move";
			this.btnMove.UseVisualStyleBackColor = true;
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Location = new System.Drawing.Point(104, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(590, 580);
			this.panel1.TabIndex = 2;
			// 
			// btnAuto
			// 
			this.btnAuto.Location = new System.Drawing.Point(23, 89);
			this.btnAuto.Name = "btnAuto";
			this.btnAuto.Size = new System.Drawing.Size(75, 23);
			this.btnAuto.TabIndex = 3;
			this.btnAuto.Text = "Auto";
			this.btnAuto.UseVisualStyleBackColor = true;
			this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
			// 
			// labelMaxL
			// 
			this.labelMaxL.AutoSize = true;
			this.labelMaxL.Location = new System.Drawing.Point(709, 533);
			this.labelMaxL.Name = "labelMaxL";
			this.labelMaxL.Size = new System.Drawing.Size(84, 15);
			this.labelMaxL.TabIndex = 4;
			this.labelMaxL.Text = "Max Length : 0";
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(23, 223);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 5;
			this.btnLoad.Text = "Load";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(23, 252);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 6;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnSaveTrainData
			// 
			this.btnSaveTrainData.Location = new System.Drawing.Point(23, 329);
			this.btnSaveTrainData.Name = "btnSaveTrainData";
			this.btnSaveTrainData.Size = new System.Drawing.Size(75, 23);
			this.btnSaveTrainData.TabIndex = 8;
			this.btnSaveTrainData.Text = "Save t";
			this.btnSaveTrainData.UseVisualStyleBackColor = true;
			this.btnSaveTrainData.Click += new System.EventHandler(this.btnSaveTrainData_Click);
			// 
			// btnLoadTrainData
			// 
			this.btnLoadTrainData.Location = new System.Drawing.Point(23, 300);
			this.btnLoadTrainData.Name = "btnLoadTrainData";
			this.btnLoadTrainData.Size = new System.Drawing.Size(75, 23);
			this.btnLoadTrainData.TabIndex = 7;
			this.btnLoadTrainData.Text = "Load t";
			this.btnLoadTrainData.UseVisualStyleBackColor = true;
			this.btnLoadTrainData.Click += new System.EventHandler(this.btnLoadTrainData_Click);
			// 
			// labelDataL
			// 
			this.labelDataL.AutoSize = true;
			this.labelDataL.Location = new System.Drawing.Point(709, 558);
			this.labelDataL.Name = "labelDataL";
			this.labelDataL.Size = new System.Drawing.Size(86, 15);
			this.labelDataL.TabIndex = 9;
			this.labelDataL.Text = "Data Length : 0";
			// 
			// labelMoveCount
			// 
			this.labelMoveCount.AutoSize = true;
			this.labelMoveCount.Location = new System.Drawing.Point(951, 533);
			this.labelMoveCount.Name = "labelMoveCount";
			this.labelMoveCount.Size = new System.Drawing.Size(57, 15);
			this.labelMoveCount.TabIndex = 10;
			this.labelMoveCount.Text = "Moves : 0";
			// 
			// labelMeanErr
			// 
			this.labelMeanErr.AutoSize = true;
			this.labelMeanErr.Location = new System.Drawing.Point(951, 558);
			this.labelMeanErr.Name = "labelMeanErr";
			this.labelMeanErr.Size = new System.Drawing.Size(80, 15);
			this.labelMeanErr.TabIndex = 11;
			this.labelMeanErr.Text = "Mean Error : 0";
			// 
			// FormGameSnake
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1233, 601);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.labelMeanErr);
			this.Controls.Add(this.labelMoveCount);
			this.Controls.Add(this.labelDataL);
			this.Controls.Add(this.btnSaveTrainData);
			this.Controls.Add(this.btnLoadTrainData);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.labelMaxL);
			this.Controls.Add(this.btnAuto);
			this.Controls.Add(this.btnMove);
			this.Controls.Add(this.pictureBox1);
			this.Name = "FormGameSnake";
			this.Text = "FormGameSnake";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btnMove;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnAuto;
		private System.Windows.Forms.Label labelMaxL;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnSaveTrainData;
		private System.Windows.Forms.Button btnLoadTrainData;
		private System.Windows.Forms.Label labelDataL;
		private System.Windows.Forms.Label labelMoveCount;
		private System.Windows.Forms.Label labelMeanErr;
	}
}