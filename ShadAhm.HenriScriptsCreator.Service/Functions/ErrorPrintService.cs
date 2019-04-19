using ShadAhm.HenriScriptsCreator.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadAhm.HenriScriptsCreator.Service.Functions
{
    public class ErrorPrintService
    {
        public void Print(IEnumerable<ConsoleMessage> errorMessages)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\o\WriteLines2.txt"))
            {
                foreach (ConsoleMessage line in errorMessages)
                {
                    file.WriteLine(line.Text);
                }
            }
        }
    }
}
