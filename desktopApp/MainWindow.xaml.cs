using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace desktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, List<string>> properties = new Dictionary<string, List<string>>();
        private string xmlpath;
        private XmlDocument document;
        private List<Drug> DrugList = new List<Drug>();

        public MainWindow()
        {
            xmlpath = "./Assets/data.xml";
            document = new XmlDocument();
            document.Load(xmlpath);
            this.properties.Add("nazwa", new List<string>());
            this.properties.Add("postac", new List<string>());
            this.properties.Add("rodzaj", new List<string>());
            this.properties.Add("substancje", new List<string>());
            this.properties["nazwa"].Add("");
            this.properties["rodzaj"].Add("");
            this.properties["postac"].Add("");
            this.properties["substancje"].Add("");
            this.InitDrugsAndProperties();
            InitializeComponent();
            for(char c = 'A'; c <= 'Z'; c++)
            {
                LiteraSubstancji.Items.Add(c);
            }
            LiteraSubstancji.SelectedIndex = 0;

            PostacComboBox.ItemsSource = properties["postac"];
            RodzajComboBox.ItemsSource = properties["rodzaj"];
            SubstancjaComboBox.ItemsSource = properties["substancje"].Where(s => s.StartsWith(LiteraSubstancji.Text));
        }

        private void LiteraSubstancji_DropDownClosed(object sender, EventArgs e)
        {
            SubstancjaComboBox.ItemsSource = properties["substancje"].Where(s => s.StartsWith(LiteraSubstancji.Text));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string searchedNazwa = NazwaProduktu.Text;
            string searchedPostac = PostacComboBox.Text;
            string searchedRodzaj = RodzajComboBox.Text;
            string searchedSubstancja = SubstancjaComboBox.Text;

            List<string> znalezione = new List<string>(); 
            
            foreach(Drug d in this.DrugList)
            {
                bool found = true;

                if (d.GetNazwa() == "" || d.GetNazwa() == String.Empty || d.GetNazwa().ToLower().Contains(searchedNazwa.ToLower()))
                {
                    if(searchedPostac != "" && searchedPostac != String.Empty)
                    {
                        if (!(d.GetPostac().ToLower() == searchedPostac.ToLower())) found = false;
                    }

                    if (searchedRodzaj != "" && searchedRodzaj != String.Empty)
                    {
                        if (!(d.GetRodzaj().ToLower() == searchedRodzaj.ToLower())) found = false;
                    }

                    if (searchedSubstancja != "" && searchedSubstancja != String.Empty)
                    {
                        if (!(d.GetSubstancje().Contains(searchedSubstancja))) found = false;
                    }
                }
                else
                {
                    found = false;
                }
                if(found)
                {
                    znalezione.Add(d.ToString());
                }
            }
            ListaZnalezionych.ItemsSource = znalezione;
            LabelZnalezione.Content = "Znalezione rekodry: " + znalezione.Count;
        }

        private void InitDrugsAndProperties()
        {
            var drugs = document.GetElementsByTagName("produktLeczniczy");
            foreach (XmlNode d in drugs)
            {
                string nazwa = d.Attributes.GetNamedItem("nazwaProduktu").Value;
                if (!this.properties["nazwa"].Contains(nazwa)) this.properties["nazwa"].Add(nazwa);

                string postac = d.Attributes.GetNamedItem("postac").Value;
                if (!this.properties["postac"].Contains(postac)) this.properties["postac"].Add(postac);

                string rodzaj = d.Attributes.GetNamedItem("rodzajPreparatu").Value;
                if (!this.properties["rodzaj"].Contains(rodzaj)) this.properties["rodzaj"].Add(rodzaj);

                List<string> substancje = new List<string>();
                foreach (XmlNode substancja in d["substancjeCzynne"])
                {
                    substancje.Add(substancja.InnerText);
                    if (!this.properties["substancje"].Contains(substancja.InnerText)) this.properties["substancje"].Add(substancja.InnerText);
                }

                DrugList.Add(new Drug(nazwa, postac, rodzaj, substancje));
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            LiteraSubstancji.SelectedIndex = 0;
            SubstancjaComboBox.ItemsSource = properties["substancje"].Where(s => s.StartsWith(LiteraSubstancji.Text));
            SubstancjaComboBox.SelectedValue = String.Empty;
            NazwaProduktu.Text = string.Empty;
            PostacComboBox.SelectedIndex = 0;
            RodzajComboBox.SelectedIndex = 0;
        }
    }
}
