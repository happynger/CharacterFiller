using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;

namespace CharacterFiller
{
	public static class PDFFiller
	{
		public static FullDataContainer dataContainer;

		public static readonly string SourcePDF = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "5E_CharacterSheet_Fillable.pdf");
		
		public static void Fill(string filename)
		{
			var pdf = ConfirmFile();

			var form = pdf.Form as PdfLoadedForm;
			(form.Fields["CharacterName"] as PdfLoadedTextBoxField).Text = dataContainer.Name;
			(form.Fields["ClassLevel"] as PdfLoadedTextBoxField).Text = dataContainer.Level + dataContainer.Class;
			(form.Fields["Background"] as PdfLoadedTextBoxField).Text = dataContainer.Background;
			(form.Fields["PlayerName"] as PdfLoadedTextBoxField).Text = dataContainer.Player_Name;
			(form.Fields["Race "] as PdfLoadedTextBoxField).Text = dataContainer.Race;
			(form.Fields["Alignment"] as PdfLoadedTextBoxField).Text = dataContainer.Alignment;
			(form.Fields["XP"] as PdfLoadedTextBoxField).Text = dataContainer.XP;

			FillAttributes(form);
			FillSkills(form);

			pdf.Save(filename);
			pdf.Close(true);
		}

		private static void FillAttributes(PdfLoadedForm form)
		{
			foreach (var item in dataContainer.Attributes)
			{
				var attribute = item as AttributeItem;

				var value = attribute.Prof ? dataContainer.ProfBonus + attribute.ModValue : attribute.ModValue;
				
				var field = attribute.PDF_Name ?? attribute.Name;
				(form.Fields[$"{field}"] as PdfLoadedTextBoxField).Text = attribute.Value.ToString();
				(form.Fields[$"{field}mod"] as PdfLoadedTextBoxField).Text = attribute.ModValue.ToString();
				(form.Fields[$"{attribute.ST_Field}"] as PdfLoadedTextBoxField).Text = value.ToString();
				(form.Fields[$"{attribute.ST_Prof}"] as PdfLoadedCheckBoxField).Checked = attribute.Prof;
			}
		}

		private static void FillSkills(PdfLoadedForm form)
		{
			foreach (var item in dataContainer.Skills)
			{
				var skill = item as SkillItem;

				int value = skill.Prof ?	dataContainer.ProfBonus +
										   (dataContainer.Attributes[(int)skill.AttributeLink] as AttributeItem).ModValue
										 : (dataContainer.Attributes[(int)skill.AttributeLink] as AttributeItem).ModValue;

				var field = skill.PDF_Name ?? skill.Name;
				(form.Fields[$"{field}"] as PdfLoadedTextBoxField).Text = value.ToString();
				(form.Fields[$"{skill.Prof_Name}"] as PdfLoadedCheckBoxField).Checked = skill.Prof;
			}
		}

		private static PdfLoadedDocument ConfirmFile()
		{
			if (!File.Exists(SourcePDF))
				throw new Exception("The File was not dowloaded");
			return new PdfLoadedDocument(SourcePDF);
		}

	}
}
