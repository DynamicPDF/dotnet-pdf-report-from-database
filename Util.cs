using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfReportFromDatabase
{
    class Util
    {
        static string resourcePath = System.IO.Path.Combine("..", "..", "Resources");

        internal static string GetResourcePath(string inputFileName)
        {
            return System.IO.Path.Combine(resourcePath, inputFileName);
        }
    }
}
