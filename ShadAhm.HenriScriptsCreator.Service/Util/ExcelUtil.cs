using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel; 

namespace ShadAhm.HenriScriptsCreator.Service.Util
{
    public class ExcelUtil
    {
        public Application App { get; set; }
        public Workbook Workbook { get; set; }

        public ExcelUtil(string filePath)
        {
            App = new Application();
            Workbooks workbooks = App.Workbooks;
            Workbook = workbooks.Open(filePath, ReadOnly: true);
        }

        public void Close()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Workbook.Close(Type.Missing, Type.Missing, Type.Missing);
            Marshal.FinalReleaseComObject(Workbook);

            App.Quit();
            Marshal.FinalReleaseComObject(App);
        }
    }
}
