//-----------------------------------------------------------------------
// <copyright file="SigningAlgorithm.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace NerdBank.OAuth {
	/// <summary>
	/// The signing algorithm to use.
	/// </summary>
	public enum SigningAlgorithm {
		/// <summary>
		/// The OAuth 1.0 PLAINTEXT algorithm.
		/// </summary>
		PlainText,

		/// <summary>
		/// The OAuth 1.0 HMAC-SHA1 algorithm.
		/// </summary>
		HmacSha1,
	}
}
