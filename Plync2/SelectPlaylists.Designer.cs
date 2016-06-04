namespace Plync2 {
	partial class SelectPlaylists {
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
			this.DownloadBtn = new System.Windows.Forms.Button();
			this.PlaylistItems = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// DownloadBtn
			// 
			this.DownloadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DownloadBtn.Location = new System.Drawing.Point(12, 394);
			this.DownloadBtn.Name = "DownloadBtn";
			this.DownloadBtn.Size = new System.Drawing.Size(651, 23);
			this.DownloadBtn.TabIndex = 1;
			this.DownloadBtn.Text = "Download playlists";
			this.DownloadBtn.UseVisualStyleBackColor = true;
			this.DownloadBtn.Click += new System.EventHandler(this.DownloadBtn_Click);
			// 
			// PlaylistItems
			// 
			this.PlaylistItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PlaylistItems.FormattingEnabled = true;
			this.PlaylistItems.Location = new System.Drawing.Point(12, 12);
			this.PlaylistItems.Name = "PlaylistItems";
			this.PlaylistItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.PlaylistItems.Size = new System.Drawing.Size(651, 368);
			this.PlaylistItems.Sorted = true;
			this.PlaylistItems.TabIndex = 2;
			// 
			// SelectPlaylists
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(675, 429);
			this.Controls.Add(this.PlaylistItems);
			this.Controls.Add(this.DownloadBtn);
			this.Name = "SelectPlaylists";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select playlists";
			this.Load += new System.EventHandler(this.SelectPlaylists_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button DownloadBtn;
		private System.Windows.Forms.ListBox PlaylistItems;
	}
}