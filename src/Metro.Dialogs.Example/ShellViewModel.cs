﻿using System;
using System.ComponentModel.Composition;
using System.Reactive.Linq;
using Caliburn.Micro;
using Metro.Dialogs;//.Controls;

namespace Metro.Dialogs.Example
{
    [Export(typeof (IShell))]
    public class ShellViewModel : Screen, IShell
    {
        private readonly IWindowsDialogs _windowsDialogs;
        private bool _isBusy;
        private double _busyProgress;

        [ImportingConstructor]
        public ShellViewModel(IWindowsDialogs windowsDialogs)
        {
            _windowsDialogs = windowsDialogs;
            InitBreadcrumbs();
        }

       

        public void OpenSplashScreen()
        {
            _windowsDialogs.OpenSplashScreen("Metro.Dialogs", "0.1.2.34", "About box\nSplash screen\netc...",
                                             "Organization ltd.", "all right reserved", "licened to ");
        }

        public void OpenAboutSimple()
        {
            _windowsDialogs.ShowAboutBox("Metro Dialogs", "0.1.2.345", "About program, bla-bla-bla...");
        }

        public void OpenAboutWithActions()
        {
            _windowsDialogs.ShowAboutBox("Metro Dialogs", "0.1.2.345", "About program, bla-bla-bla...",
                                         new ActionViewModel("DefaultAction", "key1", true),
                                         new ActionViewModel("CancelAction", "key2", false, true),
                                         new ActionViewModel("CustomAction", "key3"));
        }

        public void OpenAbout()
        {
            OpenAboutWithActions();
        }

        public void SimpleError()
        {
            _windowsDialogs.ShowError("Caption", "Error\n bla-bla-bla");
        }

        public void SimpleWarning()
        {
            _windowsDialogs.ShowWarning("Caption", "Warning\n bla-bla-bla");
        }

        public void SimpleInfo()
        {
            _windowsDialogs.ShowInfo("Caption", "Warning\n bla-bla-bla");
        }

        public void SimpleYesNo()
        {
            _windowsDialogs.AskUserYesNo("Header", "All right?");
        }

        public void AskWithInfo()
        {
            _windowsDialogs.ShowMessageBox("Header", "Click top button 'info'", MessageBoxIconType.Error,
                                           new MessageBoxHelpButton("Info",
                                                                    () => _windowsDialogs.ShowExceptionInfoDialog("Inforamtion about exception", new Exception("Custom exception info"))), new MessageBoxButton("Ok"));
        }

        public void CustomButtons()
        {
            var res = _windowsDialogs.ShowMessageBox("Header", "Click top button 'info'", MessageBoxIconType.Error, new MessageBoxButton<int>("Action 1",1,true), new MessageBoxButton<int>("Action 2",2,false), new MessageBoxButton<int>("Action 3",3,false));
            _windowsDialogs.ShowInfo("Selected item","Select "+res);
        }


        public void SaveFile()
        {
            _windowsDialogs.ShowSaveFileDialog("ShowSaveFileDialog", "txt", "Text Files (.txt)|*.txt|All Files (*.*)|*.*", "C:\\");
        }
        public void OpenFile()
        {
            _windowsDialogs.ShowOpenFileDialog("ShowSaveFileDialog", "Text Files (.txt)|*.txt|All Files (*.*)|*.*", "C:\\");
        }
        public void SelectFolder()
        {
            _windowsDialogs.ShowSelectFolderDialog("ShowSelectFolderDialog", "C:\\");
        }

        public void SelectOne()
        {
            _windowsDialogs.SelectItemDialog("Caption", "Message", true,
                new SelectItem("DisplayName 1", "Description 1", 1,_=>_ == "1"),
                new SelectItem("DisplayName 2", "Description 2", 2, _ => _ == "2"));
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public double BusyProgress
        {
            get { return _busyProgress; }
            set
            {
                if (value.Equals(_busyProgress)) return;
                _busyProgress = value;
                NotifyOfPropertyChange(() => BusyProgress);
            }
        }

        public async void SetBusy()
        {
            BusyProgress = 0;
            IsBusy = true;
            for (int i = 0; i < 5; i++)
            {
                await Observable.Timer(TimeSpan.FromSeconds(1));
                BusyProgress += 20;    
            }
            await Observable.Timer(TimeSpan.FromSeconds(1));
            IsBusy = false;
        }



        #region Breadcrumb

        private void InitBreadcrumbs()
        {
            Breadcrumbs = new BindableCollection<BreadcrumbActionViewModel>
                              {
                                  new BreadcrumbActionViewModel(() => _windowsDialogs.ShowInfo("Click", "Food"))
                                      {CanExecute = true, DisplayName = "Food"},
                                  new BreadcrumbActionViewModel(() => _windowsDialogs.ShowInfo("Click", "Fruit"))
                                      {CanExecute = true, DisplayName = "Fruit"}
                              };
        }

        public BindableCollection<BreadcrumbActionViewModel> Breadcrumbs { get; set; }

        #endregion
    }

    public interface IShell
    {
    }
}