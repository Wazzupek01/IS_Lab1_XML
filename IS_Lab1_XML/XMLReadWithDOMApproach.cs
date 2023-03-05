using System.Runtime.InteropServices;
using System.Xml;

namespace IS_Lab1_XML;

public class XMLReadWithDOMApproach
{
    public static void Read(string filepath)
    {
        // odczyt zawartości dokumentu
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath);
        string postac;
        string sc;
        int count = 0;
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        Dictionary<string, List<string>> iloscPostaci = new Dictionary<string, List<string>>();
        foreach (XmlNode d in drugs)
        {
            postac = d.Attributes.GetNamedItem("postac").Value;
            sc = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;

            if (postac == "Krem" && sc == "Mometasoni furoas")
                count++;
            if (iloscPostaci.ContainsKey(sc) && !iloscPostaci[sc].Contains(postac)) iloscPostaci[sc].Add(postac);
            if (!iloscPostaci.ContainsKey(sc))
            {
                iloscPostaci[sc] = new List<string>();
                iloscPostaci[sc].Add(postac);
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
        // odczyt zawartości dokumentu
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath);
        string postac;
        string podmiot;
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        Dictionary<string, Tuple<int, int>> statPodmiotu = new Dictionary<string, Tuple<int, int>>();
        foreach (XmlNode d in drugs)
        {
            postac = d.Attributes.GetNamedItem("postac").Value;
            podmiot = d.Attributes.GetNamedItem("podmiotOdpowiedzialny").Value;
            if (!statPodmiotu.ContainsKey(podmiot)) statPodmiotu[podmiot] = new Tuple<int, int>(0, 0); 
            if (postac == "Krem") statPodmiotu[podmiot] = new Tuple<int, int>(statPodmiotu[podmiot].Item1 + 1, statPodmiotu[podmiot].Item2);
            if (postac == "Tabletki") statPodmiotu[podmiot] = new Tuple<int, int>(statPodmiotu[podmiot].Item1, statPodmiotu[podmiot].Item2 + 1);
        }
        
        var maxKremow = statPodmiotu.OrderByDescending(x => x.Value.Item1).First();
        var maxTabletek = statPodmiotu.OrderByDescending(x => x.Value.Item2).First();

        Console.WriteLine("Podmiot sprzedający najwięcej kremów: " + maxKremow.Key + " w ilości " + maxKremow.Value.Item1);
        Console.WriteLine("Podmiot sprzedający najwięcej tabletek: " + maxKremow.Key + " w ilości " + maxKremow.Value.Item2);
    }

    public static void ReadMostKremy(string filepath)
    {
        // odczyt zawartości dokumentu
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath);
        string postac;
        string podmiot;
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        Dictionary<string, int> statPodmiotu = new Dictionary<string, int>();
        foreach (XmlNode d in drugs)
        {
            postac = d.Attributes.GetNamedItem("postac").Value;
            podmiot = d.Attributes.GetNamedItem("podmiotOdpowiedzialny").Value;
            if (!statPodmiotu.ContainsKey(podmiot)) statPodmiotu[podmiot] = 0;
            if (postac == "Krem") statPodmiotu[podmiot]++;
        }

        var maxKremow = statPodmiotu.OrderByDescending(x => x.Value);

        Console.WriteLine("3 największych sprzedawców kremów: ");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine(i+ ". " + maxKremow.ElementAt(i).Key + " " + maxKremow.ElementAt(i).Value);
        }
    }

    public static void IdentifySingleComponentDrugs(string filepath)
    {
        Dictionary<string, List<string>> singleComponentDrugs = ReadDrugsWithSubstanceList(filepath)
            .Where(l => l.Value.Count == 1)
            .ToDictionary(l => l.Key, l => l.Value);
        Console.WriteLine($"Znaleziono {singleComponentDrugs.Count} jednoskładnikowych leków. Czy wyświetlić?(t/n): ");
        string decision = Console.ReadLine().ToLower();
        if (decision == "t")
        {
            foreach (var l in singleComponentDrugs)
            {
                Console.Write(l.Key + " | ");
                foreach (string s in l.Value) Console.Write(s + " ");
                Console.WriteLine();
            }
        }
    }

    public static void IdentifyMultiComponentDrugs(string filepath)
    {
        Dictionary<string, List<string>> multiComponentDrugs = ReadDrugsWithSubstanceList(filepath)
            .Where(l => l.Value.Count > 1)
            .ToDictionary(l => l.Key, l => l.Value);
        Console.WriteLine($"Znaleziono {multiComponentDrugs.Count} wieloskładnikowych leków. Czy wyświetlić?(t/n): ");
        string decision = Console.ReadLine().ToLower();
        if(decision == "t")
        {
            foreach (var l in multiComponentDrugs)
            {
                Console.Write(l.Key + " | ");
                foreach (string s in l.Value) Console.Write(s + " ");
                Console.WriteLine();
            } 
        }
    }

    private static Dictionary<string, List<string>> ReadDrugsWithSubstanceList(string filepath)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filepath);
        string nazwa;
        var drugs = doc.GetElementsByTagName("produktLeczniczy");
        Dictionary<string, List<string>> leki = new Dictionary<string, List<string>>();
        foreach (XmlNode d in drugs)
        {
            nazwa = d.Attributes.GetNamedItem("nazwaPowszechnieStosowana").Value;
            var a = d["substancjeCzynne"].ChildNodes;
            if (a != null)
            {
                leki[nazwa] = new List<string>();
                foreach (XmlNode i in a)
                {
                    if (i.InnerText != null)
                        leki[nazwa].Add(i.InnerText);
                }
            }
        }
        Console.WriteLine(leki.Count);
        return leki;
    }
}