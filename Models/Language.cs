using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Models
{
    [DataContract]
    public class Language : RaisableObject
    {
        private string cultureCode;
        private string text;

        [DataMember]
        public string CultureCode
        {
            get { return cultureCode; }
            set
            {
                cultureCode = value;
                RaisePropertyChanged("CultureCode");
            }
        }

        [DataMember]
        public string Text
        {
            get { return this.text; }
            set
            {
                text = value;
                RaisePropertyChanged("Text");
            }
        }

        public override string ToString()
        {
            return Text != null && Text != string.Empty ? Text : base.ToString();
        }
    }
}
