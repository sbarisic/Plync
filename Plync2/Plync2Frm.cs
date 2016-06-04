using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace Plync2 {
	public partial class Plync2Frm : Form {
		Queue<Action> JobQueue;
		SelectPlaylists PlaylistSelector;

		public Plync2Frm() {
			InitializeComponent();
			JobQueue = new Queue<Action>();

			Out.WordWrap = false;
			OutFolder.Text = Path.Combine(Application.StartupPath, "Music");
			Program.Printer = (E) => Println("Info", E);
		}

		public void Write(string Str, Color Clr) {
			Out.Invoke(new Action<string, Color>((_Str, _Clr) => {
				Out.SelectionStart = Out.TextLength;
				Out.SelectionLength = 0;
				Out.SelectionColor = _Clr;
				Out.AppendText(_Str);
				Out.SelectionColor = Out.ForeColor;
				Out.SelectionStart = Out.Text.Length;
				Out.ScrollToCaret();
			}), Str, Clr);
		}

		public void Print(string Msg) {
			Write(Msg, Color.Black);
		}

		public void Println(string Msg) {
			Print(Msg + "\n");
		}

		public void Print(string Info, string Msg) {
			Write("[" + Info + "]", Color.DarkGreen);
			Print(" " + Msg);
		}

		public void Println(string Info, string Msg) {
			Print(Info, Msg + "\n");
		}

		public void SetProgress(int P) {
			ProgressBar.Invoke(new Action<int>((Prog) => {
				ProgressBar.Value = Prog;
				ProgressLabel.Text = Prog.ToString() + "%";
			}), P);
		}

		public void DownloadVideo(string Video, string Location) {
			Program.AddJob(() => {
				SetProgress(0);

				string Title, Link;
				Yewtube.GetVideoData(Video, out Title, out Link);

				string FileName = Path.Combine(Location, Yewtube.TitleToFileName(Title) + ".mp3");
				if (!File.Exists(FileName)) {
					Println("Downloading", FileName);
					Yewtube.DownloadTo(Link, FileName, SetProgress);
					// Println(" - Done");
				} else
					Println("Skipping", FileName);
			});
		}

		public void DownloadPlaylist(string Playlist, string Location, string PlaylistName = null) {
			if (PlaylistName == null)
				PlaylistName = Yewtube.GetPlaylistName(Playlist);
			string[] Links = Yewtube.GetPlaylistItems(Playlist).Select((YTV) => YTV.Link).ToArray();
			foreach (var Link in Links)
				DownloadVideo(Link, Path.Combine(Location, PlaylistName));
		}

		private void DownloadPlaylistBtn_Click(object sender, EventArgs e) {
			Println("Job", string.Format("Playlist {0} to {1}", Input.Text, OutFolder.Text));
			DownloadPlaylist(Input.Text, OutFolder.Text);
		}

		private void DownloadVideoBtn_Click(object sender, EventArgs e) {
			Println("Job", string.Format("Video {0} to {1}", Input.Text, OutFolder.Text));
			DownloadVideo(Input.Text, OutFolder.Text);
		}

		private void SelectPlaylistsBtn_Click(object sender, EventArgs e) {
			if (PlaylistSelector != null)
				return;

			PlaylistSelector = new SelectPlaylists();
			PlaylistSelector.FormClosed += (S, E) => PlaylistSelector = null;
			if (PlaylistSelector.Init(this, Input.Text.Trim(), OutFolder.Text))
				PlaylistSelector.Show();
			else
				PlaylistSelector = null;
		}
	}
}