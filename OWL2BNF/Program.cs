using System;

namespace OWL2BNF
{
    class Program
    {
        static void Main(string[] args)
        {
            Grammar grammar = new Grammar(OWLParser.GetClasses(@"C:\Users\Max\Desktop\KPI\1.xml"));
            grammar.WriteToFile(@"C:\Users\Max\Desktop\KPI\1.txt");
        }
    }
}
