namespace IS_Lab1_XML
{
    class Program
    {
        public static void Main(String[] args)
        {
            string xmlpath = Path.Combine("Assets", "data.xml");

            // odczyt danych z wykorzystaniem DOM
            //Console.WriteLine("XML loaded by DOM Approach");
            //XMLReadWithDOMApproach.Read(xmlpath);
            //XMLReadWithDOMApproach.ReadPodmioty(xmlpath);
            //XMLReadWithDOMApproach.ReadMostKremy(xmlpath);

            //// odczyt danych z wykorzystaniem SAX
            //Console.WriteLine("XML loaded by SAX Approach");
            //XMLReadWithSAXApproach.Read(xmlpath);
            //XMLReadWithSAXApproach.ReadPodmioty(xmlpath);
            //XMLReadWithSAXApproach.ReadMostKremy(xmlpath);

            //// odczyt danych z wykorzystaniem XPath i DOM
            //Console.WriteLine("XML loaded with XPath");
            //XMLReadWithXLSTDOM.Read(xmlpath);
            //XMLReadWithXLSTDOM.ReadPodmioty(xmlpath);
            //XMLReadWithXLSTDOM.ReadMostKremy(xmlpath);
            XMLReadWithXLSTDOM.Read(xmlpath);
            Console.ReadLine();
        }
    }
}

