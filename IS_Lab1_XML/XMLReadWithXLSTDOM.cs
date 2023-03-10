using System;
using System.Xml;
using System.Xml.XPath;

namespace IS_Lab1_XML;

public class XMLReadWithXLSTDOM
{
    public static void Read(string filepath)
    {
        XPathDocument document = new XPathDocument(filepath);
        XPathNavigator navigator = document.CreateNavigator();
        
        XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");
        
        XPathExpression query =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem' and @nazwaPowszechnieStosowana='Mometasoni furoas']");

        query.SetContext(manager);
        int count = navigator.Select(query).Count;

        XPathExpression queryNazwy =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy/@nazwaPowszechnieStosowana");
        
        queryNazwy.SetContext(manager);

        var enumerator = navigator.Select(queryNazwy).GetEnumerator();
        int kilkaPostaci = 0;

        List<string> nazwy = new List<string>();

        while (enumerator.MoveNext())
        {
            string nazwa = enumerator.Current.ToString();

            if (!nazwy.Contains(nazwa))
            {
                nazwy.Add(nazwa);
                if (nazwa.Contains('\''))
                {
                    nazwa = nazwa.Replace("'", "&apos;");

                }
                XPathExpression queryPostaci =
                    navigator.Compile($"/x:produktyLecznicze/x:produktLeczniczy[@nazwaPowszechnieStosowana='{nazwa}']/@postac");
                queryPostaci.SetContext(manager);

                var e = navigator.Select(queryPostaci).GetEnumerator();
                List<string> postaciPreparatu = new List<string>();
                while (e.MoveNext())
                {
                    if (!postaciPreparatu.Contains(e.Current.ToString())) postaciPreparatu.Add(e.Current.ToString());
                }
                if (postaciPreparatu.Count >= 2) kilkaPostaci++;
            }
        }

        Console.WriteLine("Liczba produkt??w leczniczych w postaci kremu, kt??rych jedyn?? substancj?? czynn?? jest Mometasoni furoas {0}", count);
        Console.WriteLine("Preparatow o tej samej nazwie pod r????n?? postaci?? jest: " + kilkaPostaci);
    }

    public static void ReadPodmioty(string filepath)
    {
        XPathDocument document = new XPathDocument(filepath);
        XPathNavigator navigator = document.CreateNavigator();
        
        XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");
        
        XPathExpression queryKremy =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem']/@podmiotOdpowiedzialny");
        XPathExpression queryTabletki =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Tabletki']/@podmiotOdpowiedzialny");
        queryKremy.SetContext(manager);
        queryTabletki.SetContext(manager);
        var enumeratorKremow = navigator.Select(queryKremy).GetEnumerator();
        var enumeratorTabletek = navigator.Select(queryTabletki).GetEnumerator();
        Dictionary<string, int> iloscKremow = new Dictionary<string, int>();
        Dictionary<string, int> iloscTabletek = new Dictionary<string, int>();
        while (enumeratorKremow.MoveNext())
        {
            string podmiot = enumeratorKremow.Current.ToString();
            if (!iloscKremow.ContainsKey(podmiot)) iloscKremow[podmiot] = 1;
            else iloscKremow[podmiot]++;
        }
        while (enumeratorTabletek.MoveNext())
        {
            string podmiot = enumeratorTabletek.Current.ToString();
            if (!iloscTabletek.ContainsKey(podmiot)) iloscTabletek[podmiot] = 1;
            else iloscTabletek[podmiot]++;
        }

        var maxKremow= iloscKremow.OrderByDescending(x => x.Value).First();
        var maxTabletki= iloscTabletek.OrderByDescending(x => x.Value).First();
        
        Console.WriteLine("Podmiot produkuj??cy najwi??cej krem??w: " + maxKremow.Key + " w ilo??ci " + maxKremow.Value);
        Console.WriteLine("Podmiot produkuj??cy najwi??cej tabletek: " + maxTabletki.Key + " w ilo??ci " + maxTabletki.Value);
    }

    public static void ReadMostKremy(string filepath)
    {
        XPathDocument document = new XPathDocument(filepath);
        XPathNavigator navigator = document.CreateNavigator();

        XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
        manager.AddNamespace("x", "http://rejestrymedyczne.ezdrowie.gov.pl/rpl/eksport-danych-v1.0");

        XPathExpression queryKremy =
            navigator.Compile("/x:produktyLecznicze/x:produktLeczniczy[@postac='Krem']/@podmiotOdpowiedzialny");
        queryKremy.SetContext(manager);

        var enumeratorKremow = navigator.Select(queryKremy).GetEnumerator();

        Dictionary<string, int> iloscKremow = new Dictionary<string, int>();

        while (enumeratorKremow.MoveNext())
        {
            string podmiot = enumeratorKremow.Current.ToString();
            if (!iloscKremow.ContainsKey(podmiot)) iloscKremow[podmiot] = 1;
            else iloscKremow[podmiot]++;
        }


        var maxKremow = iloscKremow.OrderByDescending(x => x.Value);

        Console.WriteLine("3 najwi??kszych sprzedawc??w krem??w: ");
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine(i + ". " + maxKremow.ElementAt(i).Key + " " + maxKremow.ElementAt(i).Value);
        }
    }
}