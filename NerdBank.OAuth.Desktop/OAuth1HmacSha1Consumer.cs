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
		/// <inheritdoc/>
		protected override OAuth1HttpMessageHandlerBase CreateOAuthMessageHandlerCore(HttpMessageHandler innerHandler) {
			return new OAuth1HmacSha1HttpMessageHandler(innerHandler);
		}
	}
}
