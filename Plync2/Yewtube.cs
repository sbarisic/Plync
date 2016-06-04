using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Libraria.Serialization;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Plync2 {
	public struct YTVideo {
		public string Link;
		public string Title;

		public YTVideo(PlaylistItemSnippet Snippet) {
			Link = "http://www.youtube.com/watch?v=" + Snippet.ResourceId.VideoId;
			Title = Snippet.Title;
		}

		public static implicit operator YTVideo(PlaylistItemSnippet Snippet) {
			return new YTVideo(Snippet);
		}
	}

	public static class Yewtube {
		static T[] Concat<T>(T[] A, T[] B) {
			T[] Ret = new T[A.Length + B.Length];
			A.CopyTo(Ret, 0);
			B.CopyTo(Ret, A.Length);
			return Ret;
		}

		static YouTubeService YTS = new YouTubeService(new BaseClientService.Initializer() {
			ApiKey = "AIzaSyB2yemY5kS-VuClwDoSvi1zCg0jkmD5wpA", // I don't care
			ApplicationName = "Plync"
		});

		static string GetPlaylistID(string Link) {
			if (Link.Contains("list="))
				Link = Link.Substring(Link.IndexOf("list=") + 5).Split('&')[0];
			return Link;
		}

		static string GetChannelID(string Link) {
			const string Chan = "channel/";

			if (Link.Contains(Chan))
				Link = Link.Substring(Link.IndexOf(Chan) + Chan.Length);
			if (Link.Contains('/'))
				Link = Link.Substring(0, Link.IndexOf('/'));
			return Link;
		}

		public static YTVideo[] GetPlaylistItems(string Playlist, ref int DeletedVideos, string NextPageToken = null) {
			PlaylistItemsResource.ListRequest List = YTS.PlaylistItems.List("snippet");
			List.PlaylistId = Playlist;
			List.MaxResults = 50;
			if (NextPageToken != null)
				List.PageToken = NextPageToken;
			PlaylistItemListResponse Res = List.Execute();

			List<YTVideo> VideoIDs = new List<YTVideo>();
			for (int i = 0; i < Res.Items.Count; i++) {
				PlaylistItemSnippet Snippet = Res.Items[i].Snippet;
				if (Snippet.Thumbnails == null && Snippet.Title == "Deleted video") {
					DeletedVideos++;
					continue;
				}
				VideoIDs.Add(Snippet);
			}

			YTVideo[] Ret = VideoIDs.ToArray();
			if (Res.NextPageToken != null)
				return Concat(Ret, GetPlaylistItems(Playlist, ref DeletedVideos, Res.NextPageToken));
			return Ret;
		}

		public static Tuple<string, string>[] GetPlaylists(string Channel, string NextPageToken = null) {
			Channel = GetChannelID(Channel);

			PlaylistsResource.ListRequest List = YTS.Playlists.List("snippet");
			List.ChannelId = Channel;
			List.MaxResults = 50;

			if (NextPageToken != null)
				List.PageToken = NextPageToken;
			PlaylistListResponse Res = List.Execute();

			List<Tuple<string, string>> PlaylistIDs = new List<Tuple<string, string>>();
			for (int i = 0; i < Res.Items.Count; i++)
				PlaylistIDs.Add(new Tuple<string, string>(TitleToFileName(Res.Items[i].Snippet.Title), Res.Items[i].Id));

			Tuple<string, string>[] Ret = PlaylistIDs.ToArray();
			if (Res.NextPageToken != null)
				return Concat(Ret, GetPlaylists(Channel, Res.NextPageToken));
			return Ret;
		}

		public static string GetPlaylistName(string Playlist) {
			Playlist = GetPlaylistID(Playlist);

			string Src = DownloadString("http://www.youtube.com/playlist?list=" + Playlist);
			string Title = Regex.Match(Src, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>",
				RegexOptions.IgnoreCase).Groups["Title"].Value;

			return TitleToFileName(Title.Replace("- YouTube", ""));
		}

		public static YTVideo[] GetPlaylistItems(string Playlist) {
			Playlist = GetPlaylistID(Playlist);

			int Deleted = 0;
			return GetPlaylistItems(Playlist, ref Deleted);
		}

		/*public static void GetVideoData(string VideoURL, out string Title, out string Link) {
			VideoURL = "http://www.youtubeinmp3.com/fetch/?format=JSON&video=" + VideoURL;

			using (WebClient WC = new WebClient()) {
				dynamic Ret = JSON.Deserialize(WC.DownloadString(VideoURL));
				Link = Ret["link"];
				Title = Ret["title"];
			}
		}*/

		public static string DownloadString(string URL) {
			return Encoding.UTF8.GetString(DownloadData(URL, (P) => { }));
		}

		public static void GetVideoData(string VideoURL, out string Title, out string Link) {
			const string Site = "http://www.youtubeinmp3.com";
			string ButtonURL = Site + "/widget/button/?video=" + VideoURL;
			string ButtonSrc = DownloadString(ButtonURL);

			Link = ButtonSrc.Substring(ButtonSrc.IndexOf("downloadButton"));
			for (int i = 0; i < 2; i++)
				Link = Link.Substring(Link.IndexOf('\"') + 1);
			Link = HttpUtility.HtmlDecode(Site + Link.Substring(0, Link.IndexOf('\"')));

			Title = ButtonSrc.Substring(ButtonSrc.IndexOf("buttonTitle"));
			Title = Title.Substring(Title.IndexOf('>') + 1);
			Title = HttpUtility.HtmlDecode(Title.Substring(0, Title.IndexOf("</div></div>")));
		}

		// TODO: Optimize
		public static string TitleToFileName(string Title) {
			Title = Title.Trim();

			char[] InvChars = Path.GetInvalidFileNameChars();
			List<char> TitleNew = new List<char>();

			foreach (var C in Title) {
				if (C == '.') {
				} else if (!InvChars.Contains(C))
					TitleNew.Add(C);
			}

			return new string(TitleNew.ToArray());
		}

		public static byte[] DownloadData(string Link, Action<int> ProgressChanged) {
			bool Completed = false;
			byte[] Data = null;

			using (WebClient WC = new WebClient()) {
				WC.DownloadProgressChanged += (S, E) => ProgressChanged(E.ProgressPercentage);
				WC.DownloadDataCompleted += (S, E) => {
					ProgressChanged(100);
					Data = E.Result;
					Completed = true;
				};

				WC.DownloadDataAsync(new Uri(Link));

				while (!Completed)
					Thread.Sleep(10);
			}

			return Data;
		}

		public static void DownloadTo(string Link, string FileName, Action<int> ProgressChanged) {
			string Dir = Path.GetDirectoryName(FileName);
			if (!Directory.Exists(Dir))
				Directory.CreateDirectory(Dir);

			byte[] Data = DownloadData(Link, ProgressChanged);
			string Magic = "";
			if (Data.Length >= 3)
				Magic = new string(new char[] { (char)Data[0], (char)Data[1], (char)Data[2] });

			if (Magic != "ID3") {
				Thread.Sleep(100);
				Data = DownloadData(Link, ProgressChanged);
			}
			File.WriteAllBytes(FileName, Data);
		}
	}
}