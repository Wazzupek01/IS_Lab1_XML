namespace IS_Lab1_XML
{
    class Program
    {
        public static void Main(String[] args)
        {
            string xmlpath = Path.Combine("Assets", "data.xml");
            xmlpath = "./Assets/data.xml";

            // odczyt danych z wykorzystaniem DOM
            Console.WriteLine("xml loaded by dom approach");
            XMLReadWithDOMApproach.Read(xmlpath);
            XMLReadWithDOMApproach.ReadPodmioty(xmlpath);
            XMLReadWithDOMApproach.ReadMostKremy(xmlpath);

            // 1.3.
            // odczyt danych z wykorzystaniem SAX
            Console.WriteLine("XML loaded by SAX Approach");
            XMLReadWithSAXApproach.Read(xmlpath);
            XMLReadWithSAXApproach.ReadPodmioty(xmlpath);
            XMLReadWithSAXApproach.ReadMostKremy(xmlpath);

            // 1.4.
            // odczyt danych z wykorzystaniem XPath i DOM
            Console.WriteLine("XML loaded with XPath");
            XMLReadWithXLSTDOM.Read(xmlpath);
            XMLReadWithXLSTDOM.ReadPodmioty(xmlpath);
            XMLReadWithXLSTDOM.ReadMostKremy(xmlpath);

            // 1.5.
            // Identyfikowanie leków jednoskładnikowych
            XMLReadWithDOMApproach.IdentifySingleComponentDrugs(xmlpath);
            XMLReadWithDOMApproach.IdentifyMultiComponentDrugs(xmlpath);
            Console.ReadLine();
        }
    }
}

