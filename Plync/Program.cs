using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using YoutubeExtractor;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Plync {
	struct YTVideo {
		public string Link;
		public string Title;

		public YTVideo(PlaylistItemSnippet Snippet) {
			Link = "www.youtube.com/watch?v=" + Snippet.ResourceId.VideoId;
			Title = Snippet.Title;
			while (Title.Contains("  "))
				Title = Title.Replace("  ", " ");
		}

		public override string ToString() {
			return string.Format("{0} - {1}", Link, Title);
		}
	}

	class Program {
		const string PercFmt = "{0}%";
		static YouTubeService YTS = new YouTubeService(new BaseClientService.Initializer() {
			ApiKey = "AIzaSyB2yemY5kS-VuClwDoSvi1zCg0jkmD5wpA", // I don't care
			ApplicationName = "Plync"
		});
		static int Top, Left;

		static T[] Concat<T>(T[] A, T[] B) {
			T[] Ret = new T[A.Length + B.Length];
			A.CopyTo(Ret, 0);
			B.CopyTo(Ret, A.Length);
			return Ret;
		}

		static void Download(string Path, string Dir = null, string Name = null) {
			Download(DownloadUrlResolver.GetDownloadUrls(Path, false).ToArray(), Dir, Name);
		}

		static void Download(VideoInfo[] Vids, string Dir, string Name) {
			VideoInfo Vid = Vids.Where(I => I.CanExtractAudio && I.AudioType == AudioType.Mp3)
				.OrderByDescending(I => I.AudioBitrate)
				.First();

			if (Vid.RequiresDecryption)
				DownloadUrlResolver.DecryptDownloadUrl(Vid);

			if (string.IsNullOrEmpty(Dir))
				Dir = Environment.CurrentDirectory;
			if (string.IsNullOrEmpty(Name))
				Name = Vid.Title;

			AudioDownloader DL = new AudioDownloader(Vid, Path.Combine(Dir, Name + Vid.AudioExtension));
			DL.DownloadProgressChanged += (S, A) => {
				LoadCursor();
				Console.Write(PercFmt, Math.Round(A.ProgressPercentage * 0.85));
			};
			DL.AudioExtractionProgressChanged += (S, A) => {
				LoadCursor();
				Console.Write(PercFmt, Math.Round(85 + A.ProgressPercentage * 0.15));
			};
			DL.Execute();
		}

		static YTVideo[] GetPlaylistItems(string Playlist, string NextPageToken = null) {
			PlaylistItemsResource.ListRequest List = YTS.PlaylistItems.List("snippet");
			List.PlaylistId = Playlist;
			List.MaxResults = 50;
			if (NextPageToken != null)
				List.PageToken = NextPageToken;
			PlaylistItemListResponse Res = List.Execute();

			List<YTVideo> VideoIDs = new List<YTVideo>();
			for (int i = 0; i < Res.Items.Count; i++)
				VideoIDs.Add(new YTVideo(Res.Items[i].Snippet));

			YTVideo[] Ret = VideoIDs.ToArray();
			if (Res.NextPageToken != null)
				return Concat(Ret, GetPlaylistItems(Playlist, Res.NextPageToken));
			return Ret;
		}

		static string GetPlaylistID(string Link) {
			if (Link.Contains("list="))
				Link = Link.Substring(Link.IndexOf("list=") + 5).Split('&')[0];
			return Link;
		}

		static void WriteLineCol(string Str, ConsoleColor Clr) {
			LoadCursor();
			ConsoleColor Old = Console.ForegroundColor;
			Console.ForegroundColor = Clr;
			Console.WriteLine(Str);
			Console.ForegroundColor = Old;
		}

		static void SaveCursor() {
			Top = Console.CursorTop;
			Left = Console.CursorLeft;
		}

		static void LoadCursor() {
			Console.SetCursorPosition(Left, Top);
		}

		static void Main(string[] args) {
			//args = new[] { "https://www.youtube.com/playlist?list=PLVXfLTTzCNTO4h83ThA-UOUcy2tgSrf-e", "testdir" };
			Console.Title = "Plync";

			if (args.Length != 2) {
				Console.WriteLine("Usage: plync playlist directory");
				Environment.Exit(1);
			}

			string PlaylistID = GetPlaylistID(args[0]);
			Console.Write("Fetching items from {0} ... ", PlaylistID);
			YTVideo[] Videos = GetPlaylistItems(PlaylistID);
			Console.WriteLine("OKAY");

			string Dir = Path.GetFullPath(args[1]);
			if (!Directory.Exists(Dir))
				Directory.CreateDirectory(Dir);
			string[] ExistingFiles = Directory.GetFiles(Dir, "*.mp3");

			Console.WriteLine("Found {0} items", Videos.Length);
			Console.WriteLine("Found {0} existing items", ExistingFiles.Length);
			Console.WriteLine();

			for (int i = 0; i < ExistingFiles.Length; i++) {
				bool Exists = false;
				for (int j = 0; j < Videos.Length; j++)
					if (Videos[j].Title == Path.GetFileNameWithoutExtension(ExistingFiles[i]))
						Exists = true;
				if (!Exists) {
					Console.Write("Removing \"{0}\" ... ", Path.GetFileNameWithoutExtension(ExistingFiles[i]));
					SaveCursor();
					File.Delete(ExistingFiles[i]);
					WriteLineCol("OKAY", ConsoleColor.Green);
				}
			}

			for (int i = 0; i < Videos.Length; i++) {
				Console.Write("Fetching \"{0}\" ... ", Videos[i].Title);
				SaveCursor();

				if (File.Exists(Path.Combine(Dir, Videos[i].Title + ".mp3")))
					WriteLineCol("SKIP", ConsoleColor.Yellow);
				else {
					Download(Videos[i].Link, args[1], Videos[i].Title);
					WriteLineCol("OKAY", ConsoleColor.Green);
				}
			}
		}
	}
}