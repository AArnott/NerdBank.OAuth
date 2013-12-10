//-----------------------------------------------------------------------
// <copyright file="OAuth1Consumer.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Windows.Security.Cryptography;
	using Windows.Security.Cryptography.Core;

	/// <summary>
	/// A WinRT implementation of OAuth 1.0 Consumers.
	/// </summary>
	public class OAuth1Consumer : OAuth1ConsumerBase {
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
