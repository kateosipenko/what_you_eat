using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WhatYouEatWP7.Resources.Buttons;
using WhatYouEatWP7.Resources.Common;

namespace WhatYouEatWP7.Resources
{
    public class LocalizationManager : INotifyPropertyChanged
    {
        #region Fields

        private CommonStrings commonStrings = new CommonStrings();
        private ButtonsStrings buttonStrings = new ButtonsStrings();

        #endregion Fields

        #region Properties

        public CommonStrings Common
        {
            get { return commonStrings; }
        }

        public ButtonsStrings Buttons
        {
            get { return buttonStrings; }
        }

        #endregion Properties

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
        }

        #endregion INotifyPropertyChanged

    }
}
