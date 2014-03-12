namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;

	internal static class TestUtilities {
		internal const string ConsumerKey = "some key";
		internal const string ConsumerSecret = "some secret";
		internal const string TempCredToken = "tempcred token+%";
		internal const string TempCredTokenSecret = "tempcred tokensecret+%";
		internal const string AccessToken = "ye old access token+%";
		internal const string AccessTokenSecret = "be my secret+%";
		internal static readonly Uri TemporaryCredentialsEndpoint = new Uri("http://localhost/tempcred");
		internal static readonly Uri AuthorizationEndpoint = new Uri("http://localhost/auth");
		internal static readonly Uri AccessTokenEndpoint = new Uri("http://localhost/accesstoken");
		internal static readonly Uri CallbackUri = new Uri("http://localhost/callback");

		internal static NameValueCollection ParseAuthorizationHeader(string headerParameter) {
			var result = new NameValueCollection();
			if (string.IsNullOrEmpty(headerParameter)) {
				return result;
			}

			foreach (var pair in headerParameter.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
				var tuple = pair.Split('=');
				if (tuple.Length == 2) {
					// http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx
					string key = WebUtility.UrlDecode(tuple[0]);
					string encodedValue = tuple[1];
					if (encodedValue.Length >= 2 && encodedValue[0] == '"' && encodedValue[encodedValue.Length - 1] == '"') {
						string value = WebUtility.UrlDecode(encodedValue.Substring(1, encodedValue.Length - 2));
						result.Add(key, value);
					}
				}
			}

			return result;
		}
	}
}
