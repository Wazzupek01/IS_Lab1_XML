using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desktopApp
{
    internal class Drug
    {
        private string Nazwa;
        private string Postac;
        private string Rodzaj;
        private List<string> Substancje;

        public Drug(string nazwa, string postac, string rodzaj, List<string> substancje)
        {
            this.Nazwa = nazwa;
            this.Postac = postac;
            this.Rodzaj = rodzaj;
            this.Substancje = substancje;
        }

        public override string ToString()
        {
            string toString = this.Nazwa + " " + this.Postac + " " + this.Rodzaj;
            foreach(string s in this.Substancje)
            {
                toString += " ";
                toString += s;
            }
            return toString;
        }

        public string GetNazwa()
        {
            return this.Nazwa;
        }

        public string GetRodzaj()
        {
            return this.Rodzaj;
        }

        public string GetPostac()
        {
            return this.Postac;
        }

        public List<string> GetSubstancje()
        {
            return this.Substancje;
        }
    }
}
