using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plync2 {
	public partial class SelectPlaylists : Form {
		class PlaylistItemThingie {
			public string Title;
			public string ID;
			public string Location;

			public PlaylistItemThingie(string Title, string ID, string Location) {
				this.Title = Title;
				this.ID = ID;
				this.Location = Location;
			}

			public override string ToString() {
				return Title;
			}
		}

		Plync2Frm Plync2;

		public SelectPlaylists() {
			InitializeComponent();
		}

		private void SelectPlaylists_Load(object sender, EventArgs e) {

		}

		public bool Init(Plync2Frm Plync2, string Channel, string Location) {
			this.Plync2 = Plync2;

			if (Channel.Length == 0) {
				Plync2.Println("Error", "Channel cannot be empty");
				return false;
			}

			Tuple<string, string>[] Playlists = Yewtube.GetPlaylists(Channel);
			foreach (var PL in Playlists) 
				PlaylistItems.Items.Add(new PlaylistItemThingie(PL.Item1, PL.Item2, Location));
			return true;
		}

		private void DownloadBtn_Click(object sender, EventArgs e) {
			int Cnt = PlaylistItems.SelectedItems.Count;
			if (Cnt > 0) {
				Plync2.Println("Job", "Downloading " + Cnt + " playlists");

				foreach (var Itm in PlaylistItems.SelectedItems) {
					PlaylistItemThingie Thingie = (PlaylistItemThingie)Itm;
					Plync2.DownloadPlaylist(Thingie.ID, Thingie.Location, Thingie.Title);
				}

				Program.AddJob(() => {
					Plync2.Println("Download complete!");
				});
				Close();
			}
		}
	}
}