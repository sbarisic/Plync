namespace PlyncGUI {
	partial class PlyncGUI {
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
			this.bFetch = new System.Windows.Forms.Button();
			this.tbPlaylistURL = new System.Windows.Forms.TextBox();
			this.tbDownloadDir = new System.Windows.Forms.TextBox();
			this.cbDownloadVideo = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.bPaste1 = new System.Windows.Forms.Button();
			this.bBrowse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// bFetch
			// 
			this.bFetch.Location = new System.Drawing.Point(121, 64);
			this.bFetch.Name = "bFetch";
			this.bFetch.Size = new System.Drawing.Size(395, 23);
			this.bFetch.TabIndex = 0;
			this.bFetch.Text = "Fetch";
			this.bFetch.UseVisualStyleBackColor = true;
			this.bFetch.Click += new System.EventHandler(this.bFetch_Click);
			// 
			// tbPlaylistURL
			// 
			this.tbPlaylistURL.Location = new System.Drawing.Point(96, 12);
			this.tbPlaylistURL.Name = "tbPlaylistURL";
			this.tbPlaylistURL.Size = new System.Drawing.Size(342, 20);
			this.tbPlaylistURL.TabIndex = 1;
			// 
			// tbDownloadDir
			// 
			this.tbDownloadDir.Location = new System.Drawing.Point(96, 38);
			this.tbDownloadDir.Name = "tbDownloadDir";
			this.tbDownloadDir.Size = new System.Drawing.Size(342, 20);
			this.tbDownloadDir.TabIndex = 2;
			// 
			// cbDownloadVideo
			// 
			this.cbDownloadVideo.AutoSize = true;
			this.cbDownloadVideo.Location = new System.Drawing.Point(12, 68);
			this.cbDownloadVideo.Name = "cbDownloadVideo";
			this.cbDownloadVideo.Size = new System.Drawing.Size(103, 17);
			this.cbDownloadVideo.TabIndex = 3;
			this.cbDownloadVideo.Text = "Download video";
			this.cbDownloadVideo.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Playlist URL:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(78, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Save directory:";
			// 
			// bPaste1
			// 
			this.bPaste1.Location = new System.Drawing.Point(444, 11);
			this.bPaste1.Name = "bPaste1";
			this.bPaste1.Size = new System.Drawing.Size(75, 20);
			this.bPaste1.TabIndex = 6;
			this.bPaste1.Text = "Paste";
			this.bPaste1.UseVisualStyleBackColor = true;
			this.bPaste1.Click += new System.EventHandler(this.bPaste1_Click);
			// 
			// bBrowse
			// 
			this.bBrowse.Location = new System.Drawing.Point(444, 38);
			this.bBrowse.Name = "bBrowse";
			this.bBrowse.Size = new System.Drawing.Size(75, 20);
			this.bBrowse.TabIndex = 7;
			this.bBrowse.Text = "Browse";
			this.bBrowse.UseVisualStyleBackColor = true;
			this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
			// 
			// PlyncGUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(531, 98);
			this.Controls.Add(this.bBrowse);
			this.Controls.Add(this.bPaste1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbDownloadVideo);
			this.Controls.Add(this.tbDownloadDir);
			this.Controls.Add(this.tbPlaylistURL);
			this.Controls.Add(this.bFetch);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PlyncGUI";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PlyncGUI";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button bFetch;
		private System.Windows.Forms.TextBox tbPlaylistURL;
		private System.Windows.Forms.TextBox tbDownloadDir;
		private System.Windows.Forms.CheckBox cbDownloadVideo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button bPaste1;
		private System.Windows.Forms.Button bBrowse;
	}
}

