namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// A delegating HTTP handler that signs outgoing HTTP requests 
	/// with an HMAC-SHA1 signature.
	/// </summary>
	public class OAuth1HmacSha1HttpMessageHandler : OAuth1HmacSha1HttpMessageHandlerBase {
			/// <summary>
		/// Initializes a new instance of the <see cref="OAuth1HmacSha1HttpMessageHandler"/> class.
		/// </summary>
		public OAuth1HmacSha1HttpMessageHandler() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuth1HmacSha1HttpMessageHandler"/> class.
		/// </summary>
		/// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
		public OAuth1HmacSha1HttpMessageHandler(HttpMessageHandler innerHandler)
			: base(innerHandler) {
		}

		/// <inheritdoc/>
		protected override byte[] ComputeHmacSha1(byte[] data, byte[] key) {
			using (var hmac = HMAC.Create("HMACSHA1")) {
				hmac.Key = key;
				return hmac.ComputeHash(data);
			}
		}
	}
}
