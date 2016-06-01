namespace Plync2 {
	partial class Plync2Frm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.Out = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.Input = new System.Windows.Forms.TextBox();
			this.DownloadPlaylistBtn = new System.Windows.Forms.Button();
			this.DownloadVideoBtn = new System.Windows.Forms.Button();
			this.OutFolder = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.ProgressLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Out
			// 
			this.Out.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Out.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Out.Location = new System.Drawing.Point(12, 93);
			this.Out.Name = "Out";
			this.Out.ReadOnly = true;
			this.Out.Size = new System.Drawing.Size(680, 326);
			this.Out.TabIndex = 0;
			this.Out.Text = "";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Input";
			// 
			// Input
			// 
			this.Input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Input.Location = new System.Drawing.Point(57, 12);
			this.Input.Name = "Input";
			this.Input.Size = new System.Drawing.Size(635, 20);
			this.Input.TabIndex = 2;
			// 
			// DownloadPlaylistBtn
			// 
			this.DownloadPlaylistBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DownloadPlaylistBtn.Location = new System.Drawing.Point(538, 64);
			this.DownloadPlaylistBtn.Name = "DownloadPlaylistBtn";
			this.DownloadPlaylistBtn.Size = new System.Drawing.Size(154, 23);
			this.DownloadPlaylistBtn.TabIndex = 3;
			this.DownloadPlaylistBtn.Text = "Download playlist";
			this.DownloadPlaylistBtn.UseVisualStyleBackColor = true;
			this.DownloadPlaylistBtn.Click += new System.EventHandler(this.DownloadPlaylistBtn_Click);
			// 
			// DownloadVideoBtn
			// 
			this.DownloadVideoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.DownloadVideoBtn.Location = new System.Drawing.Point(410, 64);
			this.DownloadVideoBtn.Name = "DownloadVideoBtn";
			this.DownloadVideoBtn.Size = new System.Drawing.Size(122, 23);
			this.DownloadVideoBtn.TabIndex = 4;
			this.DownloadVideoBtn.Text = "Download video";
			this.DownloadVideoBtn.UseVisualStyleBackColor = true;
			this.DownloadVideoBtn.Click += new System.EventHandler(this.DownloadVideoBtn_Click);
			// 
			// OutFolder
			// 
			this.OutFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.OutFolder.Location = new System.Drawing.Point(57, 38);
			this.OutFolder.Name = "OutFolder";
			this.OutFolder.Size = new System.Drawing.Size(635, 20);
			this.OutFolder.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Output";
			// 
			// ProgressBar
			// 
			this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ProgressBar.Location = new System.Drawing.Point(12, 425);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(616, 23);
			this.ProgressBar.TabIndex = 7;
			// 
			// ProgressLabel
			// 
			this.ProgressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ProgressLabel.AutoSize = true;
			this.ProgressLabel.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ProgressLabel.Location = new System.Drawing.Point(634, 424);
			this.ProgressLabel.Name = "ProgressLabel";
			this.ProgressLabel.Size = new System.Drawing.Size(58, 24);
			this.ProgressLabel.TabIndex = 8;
			this.ProgressLabel.Text = "100%";
			// 
			// Plync2Frm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(704, 460);
			this.Controls.Add(this.ProgressLabel);
			this.Controls.Add(this.ProgressBar);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.OutFolder);
			this.Controls.Add(this.DownloadVideoBtn);
			this.Controls.Add(this.DownloadPlaylistBtn);
			this.Controls.Add(this.Input);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Out);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Plync2Frm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Plync 2";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox Out;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Input;
		private System.Windows.Forms.Button DownloadPlaylistBtn;
		private System.Windows.Forms.Button DownloadVideoBtn;
		private System.Windows.Forms.TextBox OutFolder;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ProgressBar ProgressBar;
		private System.Windows.Forms.Label ProgressLabel;
	}
}

