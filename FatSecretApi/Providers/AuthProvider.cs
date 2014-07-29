using FatSecretApi.Core;
using FatSecretApi.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatSecretApi.Providers
{
    public class AuthProvider
    {
        private static string accessToken = string.Empty;

        public static string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }

        #region Authorization

        public static async void Authorize(string userId, AsyncCallback callback = null)
        {
            
            FatSecretRequest request = new FatSecretRequest
            {
                RequestParameter = new AuthorizationParameter { UserId = userId },
                Callback = callback
            };

            //var result = await RequestSender.SendRequestAsync(request);
            await RequestSender.SendRequestAsync(request);
        }

        private static void OnAthorizationGot()
        {
        }

        #endregion Authorization
    }
}
