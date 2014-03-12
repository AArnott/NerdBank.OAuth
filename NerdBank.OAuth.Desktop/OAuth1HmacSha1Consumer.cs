//-----------------------------------------------------------------------
// <copyright file="OAuth1HmacSha1Consumer.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace NerdBank.OAuth {
	using System.Net.Http;

	/// <summary>
	/// A .NET Framework implementation of OAuth 1.0 Consumers that signs requests
	/// using the HMAC-SHA1 algorithm.
	/// </summary>
	public class OAuth1HmacSha1Consumer : OAuth1Consumer {
		/// <summary>
		/// Initializes a new instance of the <see cref="OAuth1HmacSha1Consumer"/> class.
		/// </summary>
		public OAuth1HmacSha1Consumer() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuth1HmacSha1Consumer"/> class.
		/// </summary>
		/// <param name="consumerKey">The consumer key previously obtained from the service provider.</param>
		/// <param name="consumerSecret">The consumer secret previously obtained from the service provider.</param>
		public OAuth1HmacSha1Consumer(string consumerKey, string consumerSecret)
			: base(consumerKey, consumerSecret) {
		}

		/// <inheritdoc/>
		protected override OAuth1HttpMessageHandlerBase CreateOAuthMessageHandlerCore(HttpMessageHandler innerHandler) {
			return new OAuth1HmacSha1HttpMessageHandler(innerHandler);
		}
	}
}
