namespace IS_Lab1_XML
{
    class Program
    {
        public static void Main(String[] args)
        {
            string xmlpath = Path.Combine("Assets", "data.xml");
            
            // odczyt danych z wykorzystaniem DOM
            Console.WriteLine("XML loaded by DOM Approach");
            XMLReadWithDOMApproach.Read(xmlpath);
            XMLReadWithDOMApproach.ReadPodmioty(xmlpath);
            
            // odczyt danych z wykorzystaniem SAX
            Console.WriteLine("XML loaded by SAX Approach");
            XMLReadWithSAXApproach.Read(xmlpath);
            XMLReadWithSAXApproach.ReadPodmioty(xmlpath);
            
            // odczyt danych z wykorzystaniem XPath i DOM
            Console.WriteLine("XML loaded with XPath");
            XMLReadWithXLSTDOM.Read(xmlpath);
            XMLReadWithXLSTDOM.ReadPodmioty(xmlpath);
            
            Console.ReadLine();
        }
    }
}

