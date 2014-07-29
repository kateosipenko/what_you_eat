using FatSecretApi.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FatSecretApi.Core
{
    public class FatSecretRequest
    {
        public HttpWebRequest WebRequest { get; set; }

        public FatSecretRequestParameter RequestParameter { get; set; }

        public AsyncCallback ProviderCallback { get; set; }

        public AsyncCallback Callback { get; set; }
    }
}
