using FatSecretApi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace FatSecretApi.RequestParameters
{
    public class AuthorizationParameter : FatSecretRequestParameter
    {
        public AuthorizationParameter()
        {
            this.Method = Constants.ApiMethods.Authorization;
        }

        [JsonProperty(PropertyName = Constants.ApiFields.UserId)]
        public string UserId { get; set; }
    }
}
