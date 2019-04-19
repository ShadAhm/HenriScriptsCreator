using ShadAhm.HenriScriptsCreator.Service.Models;
using ShadAhm.HenriScriptsCreator.Service.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadAhm.HenriScriptsCreator.Service.Interfaces
{
    public abstract class FunctionBaseAbstract
    {
        private string path;
        protected string outputDirPath;
        protected string requesterName;
        protected string ticketNo; 
        protected bool dryRun;
        
        protected ExcelUtil excelUtil;

        public FunctionBaseAbstract(string path, string outputDirPath, string requesterName, string ticketNo, bool dryRun)
        {
            this.path = path;
            this.dryRun = dryRun;
            this.outputDirPath = outputDirPath;
            this.requesterName = requesterName;
            this.ticketNo = ticketNo;

            this.excelUtil = new ExcelUtil(path);
        }

        abstract public IEnumerable<ConsoleMessage> Do();
    }
}
