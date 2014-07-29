using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FatSecretApi.Core
{
    internal class Constants
    {
        internal const string ServerUrl = "http://platform.fatsecret.com/rest/server.api";
        internal const string FatSeretConsumerKey = "20c9b67a9d6b48c18fac024add79a4e3";
        internal const string FatSecretKey = "29ad69aff0444e65959542420418d8ca";
        internal const string SupportedSignatureMethod = "HMAC-SHA1";
        internal const string LastApiVersion = "1.0";
        internal const string Post = "POST";
        internal const string Umpersant = "&";

        internal class ApiFields
        {
            #region OAuth

            internal const string OAuthConsumerKey = "oauth_consumer_key";
            internal const string OAuthSignatureMethod = "oauth_signature_method";
            internal const string OAuthVersion = "oauth_version";
            internal const string OAuthTimestamp = "oauth_timestamp";
            internal const string OAuthNonce = "oauth_nonce";
            internal const string OAuthSignature = "oauth_signature";
            internal const string Method = "method";

            #endregion OAuth

            internal const string UserId = "user_id";
        }

        internal class ApiMethods
        {
            internal const string Authorization = "profile.get_auth";
        }
    }
}
