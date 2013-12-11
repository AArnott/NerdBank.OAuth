namespace NerdBank.OAuth {
	using System.Net.Http;
	using Windows.Security.Cryptography;
	using Windows.Security.Cryptography.Core;

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
			var algorithm = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
			var cryptoKey = algorithm.CreateKey(CryptographicBuffer.CreateFromByteArray(key));
			var signature = CryptographicEngine.Sign(cryptoKey, CryptographicBuffer.CreateFromByteArray(data));
			byte[] result;
			CryptographicBuffer.CopyToByteArray(signature, out result);
			return result;
		}
	}
}
