using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CharacterFiller
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<IItem> skills;
		private List<IItem> attributes;

		public MainWindow()
		{
			InitializeComponent();
			attributes = new List<IItem>()
			{
				new AttributeItem { ID = 0, Name = "Strength", Value = 0 },
				new AttributeItem { ID = 1, Name = "Dexterity", Value = 0 },
				new AttributeItem { ID = 2, Name = "Constitution", Value = 0 },
				new AttributeItem { ID = 3, Name = "Intelligence", Value = 0 },
				new AttributeItem { ID = 4, Name = "Wisdom", Value = 0 },
				new AttributeItem { ID = 5, Name = "Charisma", Value = 0 },
			};
			skills = new List<IItem>()
			{
				new SkillItem() { ID = 0, Name = "Acrobatics", AttributeLink = AttributeID.DEX },
				new SkillItem() { ID = 1, Name = "Animal", AttributeLink = AttributeID.WIS },
				new SkillItem() { ID = 2, Name = "Arcana", AttributeLink = AttributeID.INT },
				new SkillItem() { ID = 3, Name = "Athletics", AttributeLink = AttributeID.STR },
				new SkillItem() { ID = 4, Name = "Deception", AttributeLink = AttributeID.CHA },
				new SkillItem() { ID = 5, Name = "History", AttributeLink = AttributeID.INT },
				new SkillItem() { ID = 6, Name = "Insight", AttributeLink = AttributeID.WIS },
				new SkillItem() { ID = 7, Name = "Intimidation", AttributeLink = AttributeID.CHA },
				new SkillItem() { ID = 8, Name = "Investigation", AttributeLink = AttributeID.INT },
				new SkillItem() { ID = 9, Name = "Medicine", AttributeLink = AttributeID.WIS },
				new SkillItem() { ID = 10, Name = "Nature", AttributeLink = AttributeID.INT },
				new SkillItem() { ID = 11, Name = "Perception", AttributeLink = AttributeID.WIS },
				new SkillItem() { ID = 12, Name = "Perfomance", AttributeLink = AttributeID.CHA },
				new SkillItem() { ID = 13, Name = "Persuasion", AttributeLink = AttributeID.CHA },
				new SkillItem() { ID = 14, Name = "Religion", AttributeLink = AttributeID.INT },
				new SkillItem() { ID = 15, Name = "Sleight of Hand", PDF_Name = "SleightofHand", AttributeLink = AttributeID.DEX },
				new SkillItem() { ID = 16, Name = "Stealth", AttributeLink = AttributeID.DEX },
				new SkillItem() { ID = 17, Name = "Survival", AttributeLink = AttributeID.WIS },
			};
			AttributesView.ItemsSource = attributes;
			SkillsView.ItemsSource = skills;
		}

		private void Check(ref List<IItem> list, ToggleButton button, bool prof)
		{
			var items = from i in list
						where i.Name == button.Name
						select i;

			foreach (var item in items)
			{
				item.Prof = prof;
				list[item.ID] = item;
			}
		}

		#region EventHandling
		private void TextBoxSelected(object sender, KeyboardFocusChangedEventArgs e)
		{
			var obj = (TextBox)sender;
			obj.Text = string.Empty;
		}

		private void Prof_Checked(object sender, RoutedEventArgs e)
			=> Check(ref skills, sender as ToggleButton, true);

		private void Prof_Unchecked(object sender, RoutedEventArgs e)
			=> Check(ref skills, sender as ToggleButton, false);

		private void AProf_Checked(object sender, RoutedEventArgs e)
			=> Check(ref attributes, sender as ToggleButton, true);

		private void AProf_Unchecked(object sender, RoutedEventArgs e)
			=> Check(ref attributes, sender as ToggleButton, false);

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !IsTextAllowed(e.Text);
		}

		private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof(string)))
			{
				string text = (string)e.DataObject.GetData(typeof(string));
				if (!IsTextAllowed(text))
					e.CancelCommand();
			}
			else
				e.CancelCommand();
		}

		private readonly Regex _regex = new Regex("[^0-9.-]+");

		private bool IsTextAllowed(string text)
			=> !_regex.IsMatch(text);

		private void FillTheFile(object sender, RoutedEventArgs e)
		{
			PDFFiller.dataContainer = new FullDataContainer()
			{
				Attributes = attributes,
				Skills = skills,
				Name = NameTB.Text,
				Alignment = AlignmentTB.Text,
				Background = BackgroundTB.Text,
				Class = ClassTB.Text,
				Level = LevelTB.Text,
				Player_Name = PNameTB.Text,
				Race = RaceTB.Text,
				Speed = SpeedTB.Text,
				XP = XPTB.Text
			};

			PDFFiller.Fill(FileName.Text);
		}

		private void UpdateValue(object sender, TextChangedEventArgs e)
		{
			var textbox = sender as TextBox;

			if (!int.TryParse(textbox.Text, out _))
				return;

			var items = from i in attributes
						where i.Name == textbox.Name
						select i;

			foreach (var item in items)
			{
				var attribute = item as AttributeItem;
				attribute.Value = int.Parse(textbox.Text);
				attributes[item.ID] = attribute;
			}
		}
		#endregion
	}

	public interface IItem
	{
		public string Name { get; set; }
		public string PDF_Name { get; set; }
		public bool Prof { get; set; }
		public int ID { get; set; }
	}

	public class AttributeItem : IItem
	{
		public string Name { get; set; }
		public string PDF_Name { get; set; } = null;
		public bool Prof { get; set; } = false;
		public int Value { get; set; }
		public int ModValue => (Value / 2) - 5;
		public int ID { get; set; }
	}

	public class SkillItem : IItem
	{
		public string Name { get; set; }
		public string PDF_Name { get; set; } = null;
		public bool Prof { get; set; } = false;
		public int ID { get; set; }
		public AttributeID AttributeLink { get; set; }
		public string AttributeName => AttributeLink.ToString();
	}

	public enum AttributeID
	{
		DEX, STR, CON, WIS, INT, CHA
	}
}
