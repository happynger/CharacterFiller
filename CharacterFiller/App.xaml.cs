using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace CharacterFiller
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly string _url = "https://github.com/happynger/CharacterFiller/raw/master/CharacterFiller/5E_CharacterSheet_Fillable.pdf";
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			if (!File.Exists(PDFFiller.SourcePDF))
				Downloader.Download(_url, PDFFiller.SourcePDF);
		}
	}
}
