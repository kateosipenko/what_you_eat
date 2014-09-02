using Resources.Buttons;
using Resources.Citation;
using Resources.Common;
using Resources.Enums;
using Resources.Errors;
using Resources.Pages.HomePanorama;
using Resources.Pages.Profile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Resources
{
    public class LocalizationManager : INotifyPropertyChanged
    {
        #region Fields

        private CommonStrings commonStrings = new CommonStrings();
        private ButtonsStrings buttonStrings = new ButtonsStrings();
        private CitationStrings citationStrings = new CitationStrings();
        private EnumsStrings enumsStrings = new EnumsStrings();
        private ErrorsStrings errorsStrings = new ErrorsStrings();
        private HomePanoramaStrings homePanoramaStrings = new HomePanoramaStrings();
        private ProfileStrings profileStrings = new ProfileStrings();

        #endregion Fields

        #region Properties

        public CitationStrings Citations
        {
            get { return citationStrings; }
        }

        public CommonStrings Common
        {
            get { return commonStrings; }
        }

        public ButtonsStrings Buttons
        {
            get { return buttonStrings; }
        }

        public EnumsStrings Enums
        {
            get { return enumsStrings; }
        }

        public ErrorsStrings Errors
        {
            get { return errorsStrings; }
        }

        public HomePanoramaStrings HomePanorama
        {
            get { return homePanoramaStrings; }
        }

        public ProfileStrings Profile
        {
            get { return profileStrings; }
        }

        #endregion Properties

        public void RefreshLanguage()
        {
            CommonStrings.Culture = Thread.CurrentThread.CurrentUICulture;
            CitationStrings.Culture = Thread.CurrentThread.CurrentUICulture;
            ButtonsStrings.Culture = Thread.CurrentThread.CurrentUICulture;
            EnumsStrings.Culture = Thread.CurrentThread.CurrentUICulture;
            ErrorsStrings.Culture = Thread.CurrentThread.CurrentUICulture;
            HomePanoramaStrings.Culture = Thread.CurrentThread.CurrentUICulture;
            ProfileStrings.Culture = Thread.CurrentThread.CurrentUICulture;

            RaiseLocalizationChanged();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void RaiseLocalizationChanged()
        {
            RaisePropertyChanged("Common");
            RaisePropertyChanged("Buttons"); 
            RaisePropertyChanged("Citations");
            RaisePropertyChanged("Enums");
            RaisePropertyChanged("Errors");
            RaisePropertyChanged("HomePanorama");
            RaisePropertyChanged("Profile");
        }

        #endregion INotifyPropertyChanged

    }
}
