using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace PlyncGUI {
	public partial class PlyncGUI : Form {
		public PlyncGUI() {
			InitializeComponent();
		}

		private void bPaste1_Click(object sender, EventArgs e) {
			if (!Clipboard.ContainsText())
				return;
			tbPlaylistURL.Text = Clipboard.GetText();
		}

		private void bBrowse_Click(object sender, EventArgs e) {
			FolderBrowserDialog FBD = new FolderBrowserDialog();
			FBD.RootFolder = Environment.SpecialFolder.Desktop;
			if (Directory.Exists(tbDownloadDir.Text))
				FBD.SelectedPath = tbDownloadDir.Text;
			FBD.ShowNewFolderButton = true;
			if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				tbDownloadDir.Text = FBD.SelectedPath;
		}

		void EnableStuff(bool B) {
			bBrowse.Invoke(new Action<bool>((BB) => {
				bBrowse.Enabled = BB;
				bPaste1.Enabled = BB;
				bFetch.Enabled = BB;
				tbPlaylistURL.Enabled = BB;
				tbDownloadDir.Enabled = BB;
			}), B);
		}

		private void bFetch_Click(object sender, EventArgs e) {
			EnableStuff(false);
			string CmdLine = tbPlaylistURL.Text + " " + tbDownloadDir.Text;
			if (cbDownloadVideo.Checked)
				CmdLine += " /vid";

			Process P = new Process();
			P.StartInfo.Arguments = CmdLine;
			P.StartInfo.FileName = "plync.exe";
			P.EnableRaisingEvents = true;
			P.Exited += (S, E) => {
				EnableStuff(true);
				if (P.ExitCode > 0) {
					if (MessageBox.Show("Failed to fetch " + P.ExitCode + " items", "Warning", MessageBoxButtons.RetryCancel)
						== System.Windows.Forms.DialogResult.Retry) {
						bFetch.Invoke(new Action(() => {
							bFetch_Click(null, null);
						}));
					}
				} else if (P.ExitCode < 0)
					MessageBox.Show(P.StartInfo.FileName + " " + CmdLine, "Invalid arguments", MessageBoxButtons.OK);
			};

			P.Start();
		}
	}
}