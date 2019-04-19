using ShadAhm.HenriScriptsCreator.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShadAhm.HenriScriptsCreator.Desktop.Command
{
    public class FileBrowseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action _execute; 

        public FileBrowseCommand(Action laleyoSayo)
        {
            _execute = laleyoSayo;
        }

        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(); 
        }
    }
}
