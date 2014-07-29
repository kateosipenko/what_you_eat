using FatSecretApi.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace FatSecretApi
{
    internal class RequestSender
    {
        internal static long GetUtcNow()
        {
            return (DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / 10000000;
        }

        internal static void SendRequest(FatSecretRequest request)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, args) =>
            {
                HttpWebRequest webRequest = HttpWebRequest.CreateHttp(Constants.ServerUrl);
                webRequest.Method = "POST";
                webRequest.Accept = "application/json";
                request.WebRequest = webRequest;                
                webRequest.BeginGetRequestStream(OnRequestStreamGot, request);
            };
            worker.RunWorkerAsync();
        }

        internal static async Task<string> SendRequestAsync(FatSecretRequest request)
        {
            string responseContent = string.Empty;
            HttpWebRequest webRequest = HttpWebRequest.CreateHttp(Constants.ServerUrl);
            webRequest.Method = "POST";
            webRequest.Accept = "application/json";
            request.WebRequest = webRequest;
            var requestStream = await webRequest.GetRequestStreamAsync();
            request.RequestParameter.Timestamp = GetUtcNow();
            request.RequestParameter.Signature = request.RequestParameter.GetSignature();
            string requestContent = request.RequestParameter.GetParametersString();
            byte[] requestBuffer = System.Text.UTF8Encoding.UTF8.GetBytes(requestContent);
            requestStream.Write(requestBuffer, 0, requestBuffer.Length);
            HttpWebResponse webResponse = null;
            try
            {
                webResponse = (HttpWebResponse) await webRequest.GetResponseAsync();
            }
            catch (Exception exception)
            {
                var r = exception;
                //webResponse = (HttpWebResponse) exception.Response;
            }

            if (webResponse != null)
            {
                using (var stream = webResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[webResponse.ContentLength];
                    stream.Read(buffer, 0, buffer.Length);
                    responseContent = System.Text.UTF8Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                }

                if (!string.IsNullOrEmpty(responseContent))
                {
                }
            }

            return responseContent;

        }

        private static void OnRequestStreamGot(IAsyncResult asyncResult)
        {
            var request = (FatSecretRequest)asyncResult.AsyncState;
            using (var stream = request.WebRequest.EndGetRequestStream(asyncResult))
            {
                request.RequestParameter.Timestamp = GetUtcNow();
                request.RequestParameter.Signature = request.RequestParameter.GetSignature();
                string requestContent = request.RequestParameter.GetParametersString();
                byte[] requestBuffer = System.Text.UTF8Encoding.UTF8.GetBytes(requestContent);
                stream.Write(requestBuffer, 0, requestBuffer.Length);
            }

            request.WebRequest.BeginGetResponse(OnResponseGot, request);
        }

        private static void OnResponseGot(IAsyncResult asyncResult)
        {
            var request = (FatSecretRequest)asyncResult.AsyncState;
            HttpWebResponse webResponse = null;
            try
            {
                webResponse = (HttpWebResponse) request.WebRequest.EndGetResponse(asyncResult);
            }
            catch (WebException exception)
            {
                webResponse = (HttpWebResponse) exception.Response;
            }

            if (webResponse != null)
            {
                string responseContent = string.Empty;
                using (var stream = webResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[webResponse.ContentLength];
                    stream.Read(buffer, 0, buffer.Length);
                    responseContent = System.Text.UTF8Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                }

                if (!string.IsNullOrEmpty(responseContent))
                {
                }
            }
        }
    }
}
