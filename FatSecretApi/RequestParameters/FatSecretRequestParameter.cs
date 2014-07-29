using FatSecretApi.Core;
using FatSecretApi.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace FatSecretApi.RequestParameters
{
    public class FatSecretRequestParameter
    {
        private long timeStamp = 0;

        #region CommonProperties

        [JsonProperty(PropertyName = Constants.ApiFields.OAuthConsumerKey)]
        public string ConsumerKey { get { return Constants.FatSeretConsumerKey; } }

        [JsonProperty(PropertyName = Constants.ApiFields.OAuthVersion)]
        public string Version { get { return Constants.LastApiVersion; } }

        [JsonProperty(PropertyName = Constants.ApiFields.OAuthSignatureMethod)]
        public string SignatureMethod { get { return Constants.SupportedSignatureMethod; } }

        [JsonProperty(PropertyName = Constants.ApiFields.OAuthTimestamp)]
        public long Timestamp
        {
            get { return timeStamp; }
            set
            {
                timeStamp = value;
                Nonce = timeStamp.ToString();
            }
        }

        [JsonProperty(PropertyName = Constants.ApiFields.OAuthNonce)]
        public string Nonce
        {
            get;
            set;
        }

        [JsonProperty(PropertyName = Constants.ApiFields.OAuthSignature)]
        public string Signature { get; set; }

        [JsonProperty(PropertyName = Constants.ApiFields.Method)]
        public string Method { get; set; }

        #endregion CommonProperties

        // Computes a keyed hash for a source file and creates a target file with the keyed hash
        // prepended to the contents of the source file. 
        public string GetSignature()
        {
            StringBuilder requestString = new StringBuilder(Constants.Post);
            requestString.Append(Constants.Umpersant);
            requestString.Append(HttpUtility.UrlEncode(Constants.ServerUrl));
            requestString.Append(Constants.Umpersant);
            requestString.Append(HttpUtility.UrlEncode(GetParametersString()));
            string key = string.Concat(Constants.FatSecretKey, Constants.Umpersant, AuthProvider.AccessToken);
            byte[] buffer = System.Text.UTF8Encoding.UTF8.GetBytes(requestString.ToString());
            string signature = string.Empty;

            var hash = HMACSHA1Helper.ComputeHash(UTF8Encoding.UTF8.GetBytes(key), buffer);
            signature = System.Text.UTF8Encoding.UTF8.GetString(hash, 0, hash.Length);

            //using (HMACSHA1 crypto = new HMACSHA1(System.Text.UTF8Encoding.UTF8.GetBytes(key)))
            //{
            //    byte[] hash = crypto.ComputeHash(buffer, 0, buffer.Length);
            //    signature = System.Text.UTF8Encoding.UTF8.GetString(hash, 0, hash.Length);
            //}

            return signature;
        }

        public string GetParametersString()
        {
            // get all request properties and sort them by alphabet
            Dictionary<string, object> properties = new Dictionary<string, object>();
            var jsonProperties = GetType().GetProperties().Where(property => property.GetCustomAttributes(typeof(JsonPropertyAttribute), false) != null);
            foreach (var property in jsonProperties)
            {
                var attribute = property.GetCustomAttributes(typeof(JsonPropertyAttribute), false).SingleOrDefault() as JsonPropertyAttribute;
                if (attribute != null)
                {
                    var value = property.GetValue(this, null);
                    if (value != null)
                        properties.Add(attribute.PropertyName, value);
                }
            }

            var orderedProperties = properties.OrderBy(item => item.Key);

            // concat all properties to string type a=1&b=asdasd&c=asdasd...
            return string.Join(Constants.Umpersant, orderedProperties.Select(item => item.Key + "=" + item.Value));
        }
    }
}
