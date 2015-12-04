using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gitdate;
using System.Diagnostics;

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

		public static implicit operator YTVideo(PlaylistItemSnippet Snippet) {
			return new YTVideo(Snippet);
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
		static bool Video;
		static string Ext;

		static T[] Concat<T>(T[] A, T[] B) {
			T[] Ret = new T[A.Length + B.Length];
			A.CopyTo(Ret, 0);
			B.CopyTo(Ret, A.Length);
			return Ret;
		}

		static string NormalizeTitle(string Title) {
			if (Title == null)
				return null;
			Title = Title.Replace("-", " - ").Replace("=", " - ");

			string Name = "";
			for (int i = 0; i < Title.Length; i++) {
				if (!Path.GetInvalidPathChars().Contains(Title[i]) && Title[i] != '\\' && Title[i] != '/') {
					if (Title[i] == ':')
						Name += '=';
					else
						Name += Title[i];
				}
			}

			while (Name.Contains("  "))
				Name = Name.Replace("  ", " ");
			return Name;
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
			Name = NormalizeTitle(Name);

			Downloader DLoader = null;
			string SavePath = Path.Combine(Dir, Name + Ext);

			if (Video) {
				VideoDownloader DL = new VideoDownloader(Vid, SavePath);
				DLoader = DL;

				DL.DownloadProgressChanged += (S, A) => {
					LoadCursor();
					Console.Write(PercFmt, Math.Round(A.ProgressPercentage));
				};
			} else {
				AudioDownloader DL = new AudioDownloader(Vid, SavePath);
				DLoader = DL;

				DL.DownloadProgressChanged += (S, A) => {
					LoadCursor();
					Console.Write(PercFmt, Math.Round(A.ProgressPercentage * 0.85));
				};
				DL.AudioExtractionProgressChanged += (S, A) => {
					LoadCursor();
					Console.Write(PercFmt, Math.Round(85 + A.ProgressPercentage * 0.15));
				};
			}

			DLoader.Execute();
		}

		static YTVideo[] GetPlaylistItems(string Playlist, ref int DeletedVideos, string NextPageToken = null) {
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
			Console.Title = "Plync";

			int Downloaded = 0;
			int Failed = 0;
			int Skipped = 0;
			int Invalid = 0;
			int Removed = 0;

			Updater.Username = "cartman300";
			Updater.Repository = "Plync";
			Console.WriteLine("Version: {0}", Updater.Version);
			Console.WriteLine("Checking for updates");
			Updater.CheckAndUpdate((L) => {
				Console.WriteLine("Downloading version {0}", L.tag_name);
			}, false);

			if (!(args.Length == 2 || (args.Length == 3 && args[2] == "/vid"))) {
				Console.WriteLine("Usage: plync playlist directory [/vid]");
				Environment.Exit(-1);
			}

			Video = (args.Length == 3 && args[2] == "/vid");
			if (Video) {
				Ext = ".mp4";
				Console.WriteLine("Fetching audio and video");
			} else {
				Ext = ".mp3";
				Console.WriteLine("Fetching audio only");
			}

			string PlaylistID = GetPlaylistID(args[0]);
			Console.Write("Fetching items from {0} ... ", PlaylistID);
			SaveCursor();
			YTVideo[] Videos = null;
			try {
				Videos = GetPlaylistItems(PlaylistID, ref Invalid);
				WriteLineCol("OKAY", ConsoleColor.Green);
			} catch (Exception E) {
				WriteLineCol("FAIL", ConsoleColor.Red);
				Console.WriteLine(E.Message);
				SaveCursor();
				WriteLineCol("Make sure the playlist is either public or unlisted", ConsoleColor.Cyan);
				Environment.Exit(-1);
			}

			string Dir = Path.GetFullPath(args[1]);
			if (!Directory.Exists(Dir))
				Directory.CreateDirectory(Dir);
			string[] ExistingFiles = Directory.GetFiles(Dir, "*" + Ext);

			Console.WriteLine("Found {0} valid items in playlist", Videos.Length);
			Console.WriteLine("Found {0} existing items", ExistingFiles.Length);
			Console.WriteLine();

			for (int i = 0; i < ExistingFiles.Length; i++) {
				bool Exists = false;
				for (int j = 0; j < Videos.Length; j++)
					if (NormalizeTitle(Videos[j].Title) == Path.GetFileNameWithoutExtension(ExistingFiles[i]))
						Exists = true;
				if (!Exists) {
					Console.Write("Removing \"{0}\" ... ", Path.GetFileNameWithoutExtension(ExistingFiles[i]));
					SaveCursor();
					File.Delete(ExistingFiles[i]);
					WriteLineCol("OKAY", ConsoleColor.DarkYellow);
					Removed++;
				}
			}

			for (int i = 0; i < Videos.Length; i++) {
				if (File.Exists(Path.Combine(Dir, NormalizeTitle(Videos[i].Title + Ext))))
					Skipped++;
				else {
					try {
						Console.Write("Fetching \"{0}\" ... ", Videos[i].Title);
						SaveCursor();
						Download(Videos[i].Link, args[1], Videos[i].Title);
						WriteLineCol("OKAY", ConsoleColor.Green);
						Downloaded++;
					} catch (Exception E) {
						WriteLineCol("FAIL", ConsoleColor.Red);
						Failed++;

						Console.Write("REASON: ");
						if (E is YoutubeParseException)
							Console.WriteLine("Failed to parse");
						else if (E is NotSupportedException)
							Console.WriteLine(E.Message);
						else
							Console.WriteLine("Unknown");
					}
				}
			}

			Console.WriteLine();
			Console.WriteLine("Invalid: {0}", Invalid);
			Console.WriteLine("Removed: {0}", Removed);
			Console.WriteLine("Fetched: {0}", Downloaded);
			Console.WriteLine("Skipped: {0}", Skipped);
			Console.WriteLine("Total items: {0}", Skipped + Downloaded);

			if (Failed > 0) {
				Console.WriteLine("Failed to fetch {0} items", Failed);
				Environment.Exit(Failed);
			}
		}
	}
}