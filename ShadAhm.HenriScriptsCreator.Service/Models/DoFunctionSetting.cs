using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadAhm.HenriScriptsCreator.Service.Models
{
    public class DoFunctionSetting
    {
        public string Path; 
        public string OutputDirPath;
        public string RequesterName;
        public string TicketNo;
        public IEnumerable<string> SheetNames; 
        public bool DryRun;
    }
}
