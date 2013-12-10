//-----------------------------------------------------------------------
// <copyright file="OAuth1Consumer.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// A WinRT implementation of OAuth 1.0 Consumers.
	/// </summary>
	public class OAuth1Consumer : OAuth1ConsumerBase {
		/// <inheritdoc/>
		protected override byte[] ComputeHmacSha1(byte[] data, byte[] key) {
			using (var hmac = new HMACSHA1(key)) {
				return hmac.ComputeHash(data);
			}
		}
	}
}
