using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
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

		public static YTVideo[] GetPlaylistItems(string Playlist) {
			Playlist = GetPlaylistID(Playlist);

			int Deleted = 0;
			return GetPlaylistItems(Playlist, ref Deleted);
		}

		public static void GetVideoData(string VideoURL, out string Title, out string Link) {
			VideoURL = "http://www.youtubeinmp3.com/fetch/?format=JSON&video=" + VideoURL;

			using (WebClient WC = new WebClient()) {
				dynamic Ret = JSON.Deserialize(WC.DownloadString(VideoURL));
				Link = Ret["link"];
				Title = Ret["title"];
			}
		}

		// TODO: Optimize
		public static string TitleToFileName(string Title) {
			char[] InvChars = Path.GetInvalidFileNameChars();
			List<char> TitleNew = new List<char>();

			foreach (var C in Title)
				if (!InvChars.Contains(C))
					TitleNew.Add(C);

			return new string(TitleNew.ToArray()) + ".mp3";
		}

		public static void DownloadTo(string Link, string File, Action<int> ProgressChanged, Action OnCompleted) {
			bool Completed = false;

			string Dir = Path.GetDirectoryName(File);
			if (!Directory.Exists(Dir))
				Directory.CreateDirectory(Dir);

			using (WebClient WC = new WebClient()) {
				WC.DownloadProgressChanged += (S, E) => ProgressChanged(E.ProgressPercentage);
				WC.DownloadFileCompleted += (S, E) => {
					OnCompleted();
					Completed = true;
				};

				WC.DownloadFileAsync(new Uri(Link), File);

				while (!Completed)
					Thread.Sleep(10);
			}
		}
	}
}