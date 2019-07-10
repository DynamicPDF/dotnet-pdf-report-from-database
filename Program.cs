using System;
using PdfReportFromDatabase.Examples;

namespace PdfReportFromDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = string.Empty;
            bool exit = false;
            Console.WriteLine("This example shows options for handling bookmarks when merging PDFs together using DynamicPDF Merger for .NET");
            Console.WriteLine();
            while (!exit)
            {
                Console.WriteLine(" A : Simple Report with 1 Query (SimpleReportExample)");
                Console.WriteLine(" B : Report with a Sub Report (ReportWithSubReportExample)");
                Console.WriteLine(" C : Multiple reports in the same document (MultipleReportsExample)");

                Console.WriteLine();
                Console.Write("Please press the corresponding key to run an example. Press any other key to quit: ");

                ConsoleKeyInfo runKey = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();

                switch (runKey.KeyChar)
                {
                    case 'a': goto case 'A';
                    case 'A':
                        fileName = "SimpleReportExample.pdf";
                        SimpleReportExample.Run(fileName);
                        break;
                    case 'b': goto case 'B';
                    case 'B':
                        fileName = "ReportWithSubReportExample.pdf";
                        ReportWithSubReportExample.Run(fileName);
                        break;
                    case 'c': goto case 'C';
                    case 'C':
                        fileName = "MultipleReportsExample.pdf";
                        MultipleReportsExample.Run(fileName);
                        break;
                    default:
                        exit = true;
                        break;
                }
                if (!exit)
                {
                    Console.WriteLine("Press 'A' to open the PDF. Press any other key to continue.");
                    ConsoleKeyInfo openKey = Console.ReadKey(true);
                    Console.WriteLine();
                    if (openKey.KeyChar == 'a' || openKey.KeyChar == 'A')
                    {
                        System.Diagnostics.Process.Start(fileName);
                        Console.WriteLine("Please be sure to close the PDF before running the example again, or an error will occur.");
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
