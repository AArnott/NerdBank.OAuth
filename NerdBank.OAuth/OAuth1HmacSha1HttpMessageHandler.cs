﻿//-----------------------------------------------------------------------
// <copyright file="OAuth1HmacSha1HttpMessageHandler.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;
	using PCLCrypto;
	using Validation;

	/// <summary>
	/// A delegating HTTP handler that signs outgoing HTTP requests 
	/// with an HMAC-SHA1 signature.
	/// </summary>
	public class OAuth1HmacSha1HttpMessageHandler : OAuth1HttpMessageHandlerBase {
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

		/// <summary>
		/// Gets the signature method to include in the oauth_signature_method parameter.
		/// </summary>
		/// <value>
		/// The signature method.
		/// </value>
		protected override string SignatureMethod {
			get { return "HMAC-SHA1"; }
		}

		/// <summary>
		/// Computes the message authentication code for the specified data and key using the HMAC-SHA1 algorithm.
		/// </summary>
		/// <param name="data">The data to be signed.</param>
		/// <param name="key">The key used for computing the authentication code.</param>
		/// <returns>The message authentication code.</returns>
		protected byte[] ComputeHmacSha1(byte[] data, byte[] key) {
			var algorithm = WinRTCrypto.MacAlgorithmProvider.OpenAlgorithm(MacAlgorithm.HmacSha1);
			using (var hasher = algorithm.CreateHash(key)) {
				hasher.Append(data);
				return hasher.GetValueAndReset();
			}
		}

		/// <summary>
		/// Calculates the signature for the specified buffer.
		/// </summary>
		/// <param name="signedPayload">The payload to calculate the signature for.</param>
		/// <returns>
		/// The signature.
		/// </returns>
		protected override byte[] Sign(byte[] signedPayload) {
			Requires.NotNull(signedPayload, "signedPayload");

			byte[] key = Encoding.UTF8.GetBytes(this.GetConsumerAndTokenSecretString());
			return this.ComputeHmacSha1(signedPayload, key);
		}
	}
}
