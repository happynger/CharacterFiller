using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CharacterFiller
{
	public static class Downloader
	{
		public static bool Finished = false;

		public static void Download(string url, string destination)
		{
			Console.WriteLine("Started Downloading");
			WebClient webClient = new WebClient();
			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
			webClient.DownloadFileCompleted += (_, __) => Finished = true;
			webClient.DownloadProgressChanged += (_, e) => Debug.WriteLine($"{e.ProgressPercentage}%");
			try
			{
				webClient.DownloadFileAsync(new Uri(url), destination);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Dowload Error: {ex.Message}");
			}

			webClient.Dispose();
		}
	}
}
