using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 

namespace CharacterFiller
{
	public static class PDFFiller
	{
		public static FullDataContainer dataContainer;

		private static readonly string SourcePDF = AppDomain.CurrentDomain.BaseDirectory + "5E_CharacterSheet_Fillable.pdf";
		private static readonly string _url = "";
		
		public static void Fill(string filename)
		{

		}

		private static void ConfirmFile()
		{
			if (File.Exists(SourcePDF))
				return;


		}

	}
}
