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
        protected IEnumerable<string> sheetNames; 
        
        protected ExcelUtil excelUtil;

        public FunctionBaseAbstract(DoFunctionSetting options)
        {
            this.path = options.Path;
            this.dryRun = options.DryRun;
            this.outputDirPath = options.OutputDirPath;
            this.requesterName = options.RequesterName;
            this.ticketNo = options.TicketNo;
            this.sheetNames = options.SheetNames; 

            this.excelUtil = new ExcelUtil(path);
        }

        abstract public IEnumerable<ConsoleMessage> Do();
    }
}
