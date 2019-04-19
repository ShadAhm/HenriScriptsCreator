using ShadAhm.HenriScriptsCreator.Service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadAhm.HenriScriptsCreator.Service.Models
{
    public class ConsoleMessage
    {
        public ConsoleMessageType Type { get; set; } 
        public string Text { get; set; }

        public ConsoleMessage(ConsoleMessageType type, string text)
        {
            Type = type;
            Text = text;
        }
    }
}
