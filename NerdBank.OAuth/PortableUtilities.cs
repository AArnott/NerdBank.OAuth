namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;
	using Validation;

	internal static class PortableUtilities {
		/// <summary>
		/// The set of characters that are unreserved in RFC 2396 but are NOT unreserved in RFC 3986.
		/// </summary>
		private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };

		/// <summary>
		/// Gets a random number generator for use on the current thread only.
		/// </summary>
		internal static Random NonCryptoRandomDataGenerator {
			get { return ThreadSafeRandom.RandomNumberGenerator; }
		}

		/// <summary>
		/// Escapes a string according to the URI data string rules given in RFC 3986.
		/// </summary>
		/// <param name="value">The value to escape.</param>
		/// <returns>The escaped value.</returns>
		/// <remarks>
		/// The <see cref="Uri.EscapeDataString"/> method is <i>supposed</i> to take on
		/// RFC 3986 behavior if certain elements are present in a .config file.  Even if this
		/// actually worked (which in my experiments it <i>doesn't</i>), we can't rely on every
		/// host actually having this configuration element present.
		/// </remarks>
		internal static string EscapeUriDataStringRfc3986(string value) {
			Requires.NotNull(value, "value");

			// Start with RFC 2396 escaping by calling the .NET method to do the work.
			// This MAY sometimes exhibit RFC 3986 behavior (according to the documentation).
			// If it does, the escaping we do that follows it will be a no-op since the
			// characters we search for to replace can't possibly exist in the string.
			StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(value));

			// Upgrade the escaping to RFC 3986, if necessary.
			for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++) {
				escaped.Replace(UriRfc3986CharsToEscape[i], string.Format("%{0:X}", (int)UriRfc3986CharsToEscape[i][0]));
			}

			// Return the fully-RFC3986-escaped string.
			return escaped.ToString();
		}

		/// <summary>
		/// Adds a name-value pair to the end of a given URL
		/// as part of the querystring piece.  Prefixes a ? or &amp; before
		/// first element as necessary.
		/// </summary>
		/// <param name="builder">The UriBuilder to add arguments to.</param>
		/// <param name="name">The name of the parameter to add.</param>
		/// <param name="value">The value of the argument.</param>
		/// <remarks>
		/// If the parameters to add match names of parameters that already are defined
		/// in the query string, the existing ones are <i>not</i> replaced.
		/// </remarks>
		internal static void AppendQueryArgument(this UriBuilder builder, string name, string value) {
			AppendQueryArgs(builder, new[] { new KeyValuePair<string, string>(name, value) });
		}

		/// <summary>
		/// Adds a set of name-value pairs to the end of a given URL
		/// as part of the querystring piece.  Prefixes a ? or &amp; before
		/// first element as necessary.
		/// </summary>
		/// <param name="builder">The UriBuilder to add arguments to.</param>
		/// <param name="args">
		/// The arguments to add to the query.  
		/// If null, <paramref name="builder"/> is not changed.
		/// </param>
		/// <remarks>
		/// If the parameters to add match names of parameters that already are defined
		/// in the query string, the existing ones are <i>not</i> replaced.
		/// </remarks>
		internal static void AppendQueryArgs(this UriBuilder builder, IEnumerable<KeyValuePair<string, string>> args) {
			Requires.NotNull(builder, "builder");

			if (args != null && args.Count() > 0) {
				StringBuilder sb = new StringBuilder(50 + (args.Count() * 10));
				if (!string.IsNullOrEmpty(builder.Query)) {
					sb.Append(builder.Query.Substring(1));
					sb.Append('&');
				}

				sb.Append(CreateQueryString(args));

				builder.Query = sb.ToString();
			}
		}

		/// <summary>
		/// Concatenates a list of name-value pairs as key=value&amp;key=value,
		/// taking care to properly encode each key and value for URL
		/// transmission according to RFC 3986.  No ? is prefixed to the string.
		/// </summary>
		/// <param name="args">The dictionary of key/values to read from.</param>
		/// <returns>The formulated querystring style string.</returns>
		internal static string CreateQueryString(IEnumerable<KeyValuePair<string, string>> args) {
			Requires.NotNull(args, "args");

			if (!args.Any()) {
				return string.Empty;
			}

			StringBuilder sb = new StringBuilder(args.Count() * 10);

			foreach (var p in args) {
				Requires.Argument(!string.IsNullOrEmpty(p.Key), "args", "Unexpected null or empty key.");
				Requires.Argument(p.Value != null, "args", "Unexpected null value for key \"{0}\".", p.Key);
				sb.Append(EscapeUriDataStringRfc3986(p.Key));
				sb.Append('=');
				sb.Append(EscapeUriDataStringRfc3986(p.Value));
				sb.Append('&');
			}

			sb.Length--; // remove trailing &

			return sb.ToString();
		}

		/// <summary>
		/// Assembles the content of the HTTP Authorization or WWW-Authenticate header.
		/// </summary>
		/// <param name="fields">The fields to include.</param>
		/// <returns>
		/// A value prepared for an HTTP header.
		/// </returns>
		internal static string AssembleAuthorizationHeader(IEnumerable<KeyValuePair<string, string>> fields) {
			Requires.NotNull(fields, "fields");

			var authorization = new StringBuilder();
			foreach (var pair in fields) {
				string key = EscapeUriDataStringRfc3986(pair.Key);
				string value = EscapeUriDataStringRfc3986(pair.Value);
				authorization.Append(key);
				authorization.Append("=\"");
				authorization.Append(value);
				authorization.Append("\",");
			}

			authorization.Length--; // remove trailing comma
			return authorization.ToString();
		}

		/// <summary>
		/// Assembles the content of the HTTP Authorization or WWW-Authenticate header.
		/// </summary>
		/// <param name="scheme">The scheme.</param>
		/// <param name="fields">The fields to include.</param>
		/// <returns>A value prepared for an HTTP header.</returns>
		internal static string AssembleAuthorizationHeader(string scheme, IEnumerable<KeyValuePair<string, string>> fields) {
			Requires.NotNullOrEmpty(scheme, "scheme");
			Requires.NotNull(fields, "fields");

			var authorization = new StringBuilder();
			authorization.Append(scheme);
			authorization.Append(" ");
			authorization.Append(AssembleAuthorizationHeader(fields));
			return authorization.ToString();
		}

		/// <summary>
		/// Enumerates all members of the collection as key=value pairs.
		/// </summary>
		/// <param name="nvc">The collection to enumerate.</param>
		/// <returns>A sequence of pairs.</returns>
		internal static IEnumerable<KeyValuePair<string, string>> AsKeyValuePairs(this NameValueCollection nvc) {
			Requires.NotNull(nvc, "nvc");

			foreach (string key in nvc) {
				foreach (string value in nvc.GetValues(key)) {
					yield return new KeyValuePair<string, string>(key, value);
				}
			}
		}

		internal static NameValueCollection ParseUrlEncodedString(string queryString) {
			var result = new NameValueCollection();
			if (string.IsNullOrEmpty(queryString)) {
				return result;
			}

			foreach (var pair in queryString.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)) {
				var tuple = pair.Split('=');
				if (tuple.Length == 2) {
					// http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx
					result.Add(WebUtility.UrlDecode(tuple[0]), WebUtility.UrlDecode(tuple[1]));
				}
			}

			return result;
		}

		internal static NameValueCollection ParseQueryString(this Uri url) {
			return ParseUrlEncodedString(url.Query.Length > 1 ? url.Query.Substring(1) : null);
		}

		/// <summary>
		/// Gets a random string made up of a given set of allowable characters.
		/// </summary>
		/// <param name="length">The length of the desired random string.</param>
		/// <param name="allowableCharacters">The allowable characters.</param>
		/// <returns>A random string.</returns>
		internal static string GetRandomString(int length, string allowableCharacters) {
			Requires.Range(length >= 0, "length");
			Requires.That(allowableCharacters != null && allowableCharacters.Length >= 2, "allowableCharacters", "At least two allowable characters required.");

			char[] randomString = new char[length];
			var random = NonCryptoRandomDataGenerator;
			for (int i = 0; i < length; i++) {
				randomString[i] = allowableCharacters[random.Next(allowableCharacters.Length)];
			}

			return new string(randomString);
		}

		/// <summary>
		/// A thread-safe, non-crypto random number generator.
		/// </summary>
		private static class ThreadSafeRandom {
			/// <summary>
			/// The initializer of all new <see cref="Random"/> instances.
			/// </summary>
			private static readonly Random ThreadRandomInitializer = new Random();

			/// <summary>
			/// A thread-local instance of <see cref="Random"/>
			/// </summary>
			[ThreadStatic]
			private static Random threadRandom;

			/// <summary>
			/// Gets a random number generator for use on the current thread only.
			/// </summary>
			public static Random RandomNumberGenerator {
				get {
					if (threadRandom == null) {
						lock (ThreadRandomInitializer) {
							threadRandom = new Random(ThreadRandomInitializer.Next());
						}
					}

					return threadRandom;
				}
			}
		}
	}
}
