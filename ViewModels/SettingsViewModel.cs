using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewModels.Helpers;

namespace ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public SettingsViewModel()
        {
            SaveCommand = new RelayCommand(SaveExecute);
        }

        #region CurrentLanguage

        private Language currentLanguage;

        public Language CurrentLanguage
        {
            get { return this.currentLanguage; }
            set
            {
                currentLanguage = value;
                RaisePropertyChanged("CurrentLanguage");
            }
        }

        #endregion CurrentLanguage

        #region AllLanguages

        private List<Language> allLanguages = new List<Language>();

        public List<Language> AllLanguages
        {
            get { return allLanguages; }
            set
            {
                allLanguages = value;
                RaisePropertyChanged("AllLanguages");
            }
        }

        #endregion AllLanguages

        #region SaveCommand

        public RelayCommand SaveCommand { get; private set; }

        private void SaveExecute()
        {
            SettingsManager.Instance.SetCurrentLanguage(currentLanguage);
        }

        #endregion SaveCommand

        protected override void InitializeExecute()
        {
            base.InitializeExecute();
            this.CurrentLanguage = SettingsManager.Instance.CurrentLanguage;
            this.AllLanguages = SettingsManager.Instance.AllLanguages;
        }

        #region Cleanup

        public override void Cleanup()
        {
            this.CurrentLanguage = null;
            base.Cleanup();
        }

        #endregion Cleanup
    }
}
