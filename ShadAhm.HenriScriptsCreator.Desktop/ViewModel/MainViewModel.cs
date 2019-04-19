using GalaSoft.MvvmLight;
using ShadAhm.HenriScriptsCreator.Desktop.Command;
using ShadAhm.HenriScriptsCreator.Desktop.Model;
using ShadAhm.HenriScriptsCreator.Service.Functions;
using ShadAhm.HenriScriptsCreator.Service.Models;
using ShadAhm.HenriScriptsCreator.Service.Models.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ShadAhm.HenriScriptsCreator.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private AssetAttributeUpdaterSafe _doService;
        private ErrorPrintService _errorPrintService;

        public ICommand BrowseCommand { get; set; }
        public ICommand BrowseSettingsCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public ICommand PrintErrorsCommand { get; set; }


        private ObservableCollection<ConsoleMessage> _successMessages;
        public ObservableCollection<ConsoleMessage> SuccessMessages
        {
            get => _successMessages;
            set { Set(ref _successMessages, value); }
        }

        private ObservableCollection<ConsoleMessage> _errorMessages;
        public ObservableCollection<ConsoleMessage> ErrorMessages
        {
            get => _errorMessages;
            set { Set(ref _errorMessages, value); }
        }

        private string _filePath = string.Empty;
        public string FilePath
        {
            get => _filePath;
            set => Set(ref _filePath, value);
        }

        private string _outputFolderPath = string.Empty;
        public string OutputFolderPath
        {
            get => _outputFolderPath;
            set => Set(ref _outputFolderPath, value);
        }

        private string _requesterUsername = string.Empty;
        public string RequesterUsername
        {
            get => _requesterUsername;
            set => Set(ref _requesterUsername, value);
        }

        private string _ticketNumber = string.Empty;
        public string TicketNumber
        {
            get => _ticketNumber;
            set => Set(ref _ticketNumber, value);
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get => _currentStatus;
            set => Set(ref _currentStatus, value);
        }

        private string _functionName = string.Empty;
        public string FunctionName
        {
            get => _functionName;
            set => Set(ref _functionName, value);
        }


        public void BrowseFile()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Excel or Json | *.xlsx; *.json";
            fileDialog.Multiselect = true; 

            var result = fileDialog.ShowDialog();

            if (result.Value)
            {
                this.FilePath = fileDialog.FileNames.Where(i => i.EndsWith(".xlsx")).FirstOrDefault();
                string settingsFile = fileDialog.FileNames.Where(i => i.EndsWith("appSettings.json", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(settingsFile))
                {
                    AppSettingsService serv = new AppSettingsService();
                    var o = serv.ReadAppSettings(fileDialog.FileName);

                    this.OutputFolderPath = o.OutputDir;
                    this.RequesterUsername = o.RequesterUsername;
                    this.TicketNumber = o.TicketNumber; 
                }
            }
        }

        public void BrowseSettings()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Json files (*.json)|*.json";

            var result = fileDialog.ShowDialog();

            if (result.Value)
            {
                AppSettingsService serv = new AppSettingsService();
                var o = serv.ReadAppSettings(fileDialog.FileName);

                this.OutputFolderPath = o.OutputDir;
                this.RequesterUsername = o.RequesterUsername;
                this.TicketNumber = o.TicketNumber; 
            }
        }

        public void Run()
        {
            SuccessMessages = new ObservableCollection<ConsoleMessage>();
            ErrorMessages = new ObservableCollection<ConsoleMessage>();

            if (string.IsNullOrWhiteSpace(_filePath))
            {
                ErrorMessages.Add(new ConsoleMessage(ConsoleMessageType.CriticalError, "Excel file not found."));
                return;
            }

            var bk = new BackgroundWorker();
            bk.DoWork += (s, e) =>
            {
                _doService = new AssetAttributeUpdaterSafe(this._filePath, this.OutputFolderPath, this.RequesterUsername, this.TicketNumber, true);

                bool exitLoop = false;
                foreach (ConsoleMessage statusMessage in _doService.Do())
                {
                    switch (statusMessage.Type)
                    {
                        case (ConsoleMessageType.CriticalError):
                            {
                                Application.Current.Dispatcher.BeginInvoke(
                                  DispatcherPriority.Background,
                                  new Action(() => ErrorMessages.Add(statusMessage)));
                                exitLoop = true;
                                break;
                            }
                        case (ConsoleMessageType.Okay):
                            Application.Current.Dispatcher.BeginInvoke(
                              DispatcherPriority.Background,
                              new Action(() => SuccessMessages.Add(statusMessage)));
                            break;
                        case (ConsoleMessageType.Error):
                            Application.Current.Dispatcher.BeginInvoke(
                              DispatcherPriority.Background,
                              new Action(() => ErrorMessages.Add(statusMessage)));
                            break;
                    }

                    if (exitLoop) break;
                }
            };
            bk.RunWorkerAsync();
        }

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _functionName = "AssetAttributeUpdaterSafe"; 

            this.BrowseCommand = new FileBrowseCommand(BrowseFile);
            this.BrowseSettingsCommand = new FileBrowseCommand(BrowseSettings); 
            this.PrintErrorsCommand = new PrintErrorsCommand(PrintErrors); 
            this.RunCommand = new RunCommand(Run);
        }

        public void PrintErrors()
        {
            _errorPrintService = new ErrorPrintService();

            IEnumerable<ConsoleMessage> obsCollection = ErrorMessages;

            _errorPrintService.Print(obsCollection);
        }
    }
}