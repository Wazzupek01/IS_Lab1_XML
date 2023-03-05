using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace IS_Lab1_XML;

public class XMLReadWithSAXApproach
{
    public static void Read(string filepath)
    {
        // konfiguracja początkowa dla XmlReadera
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
        settings.IgnoreProcessingInstructions = true;
        settings.IgnoreWhitespace = true;
        
        // odczyt zawartości dokumentu
        XmlReader reader = XmlReader.Create(filepath, settings);
        
        // zmienne pomocnicze
        int count = 0;
        string postac;
        string sc;
        Dictionary<string, List<string>> iloscPostaci = new Dictionary<string, List<string>>();
        
        // analiza każdego z węzłów dokumentu
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "produktLeczniczy")
            {
                postac = reader.GetAttribute("postac");
                sc =
                    reader.GetAttribute("nazwaPowszechnieStosowana");
                if (postac == "Krem" && sc == "Mometasoni furoas")
                    count++;
                if (iloscPostaci.ContainsKey(sc) && !iloscPostaci[sc].Contains(postac)) iloscPostaci[sc].Add(postac);
                if (!iloscPostaci.ContainsKey(sc))
                {
                    iloscPostaci[sc] = new List<string>();
                    iloscPostaci[sc].Add(postac);
                }

            }
        }
        
        int kilkaPostaci = 0;
        foreach(var i in iloscPostaci)
        {
            if(i.Value.Count > 1) kilkaPostaci++;
        }
        Console.WriteLine("Liczba produktów leczniczych w postaci kremu, których jedyną substancją czynną jest Mometasoni furoas {0}", count);
        Console.WriteLine("Preparatow o tej samej nazwie pod różną postacią jest: " + kilkaPostaci);
    }

    public static void ReadPodmioty(string filepath)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
        settings.IgnoreProcessingInstructions = true;
        settings.IgnoreWhitespace = true;
        
        XmlReader reader = XmlReader.Create(filepath, settings);
        
        string postac;
        string podmiot;
        Dictionary<string, Tuple<int, int>> statPodmiotu = new Dictionary<string, Tuple<int, int>>();
        
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name
                == "produktLeczniczy")
            {
                postac = reader.GetAttribute("postac");
                podmiot = reader.GetAttribute("podmiotOdpowiedzialny");
                if (!statPodmiotu.ContainsKey(podmiot)) statPodmiotu[podmiot] = new Tuple<int, int>(0, 0); 
                if (postac == "Krem") statPodmiotu[podmiot] = new Tuple<int, int>(statPodmiotu[podmiot].Item1 + 1, statPodmiotu[podmiot].Item2);
                if (postac == "Tabletki") statPodmiotu[podmiot] = new Tuple<int, int>(statPodmiotu[podmiot].Item1, statPodmiotu[podmiot].Item2 + 1);
            }
        }

        var maxKremow = statPodmiotu.OrderByDescending(x => x.Value.Item1).First();
        var maxTabletek = statPodmiotu.OrderByDescending(x => x.Value.Item2).First();

        Console.WriteLine("Podmiot sprzedający najwięcej kremów: " + maxKremow.Key + " w ilości " + maxKremow.Value.Item1);
        Console.WriteLine("Podmiot sprzedający najwięcej tabletek: " + maxKremow.Key + " w ilości " + maxKremow.Value.Item2);
    }

    public static void ReadMostKremy(string filepath)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
        settings.IgnoreProcessingInstructions = true;
        settings.IgnoreWhitespace = true;

        XmlReader reader = XmlReader.Create(filepath, settings);

        string postac;
        string podmiot;
        Dictionary<string, int> statPodmiotu = new Dictionary<string, int>();

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name
                == "produktLeczniczy")
            {
                postac = reader.GetAttribute("postac");
                podmiot = reader.GetAttribute("podmiotOdpowiedzialny");
                if (!statPodmiotu.ContainsKey(podmiot)) statPodmiotu[podmiot] = 0;
                if (postac == "Krem") statPodmiotu[podmiot]++;
            }
        }

        var maxKremow = statPodmiotu.OrderByDescending(x => x.Value);

        Console.WriteLine("3 największych sprzedawców kremów: ");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine(i + ". " + maxKremow.ElementAt(i).Key + " " + maxKremow.ElementAt(i).Value);
        }
    }
}