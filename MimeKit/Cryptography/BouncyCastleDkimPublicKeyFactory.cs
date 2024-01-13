﻿//
// BouncyCastleDkimPublicKeyFactory.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2024 .NET Foundation and Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.IO;
using System.Text;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;

namespace MimeKit.Cryptography {
	/// <summary>
	/// A DKIM public key factory implemented using BouncyCastle.
	/// </summary>
	/// <remarks>
	/// A DKIM public key factory implemented using BouncyCastle.
	/// </remarks>
	public class BouncyCastleDkimPublicKeyFactory : IDkimPublicKeyFactory
	{
		/// <summary>
		/// Instantiates a new instance of the <see cref="BouncyCastleDkimPublicKeyFactory"/> class.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="BouncyCastleDkimPublicKeyFactory"/>.
		/// </remarks>
		public BouncyCastleDkimPublicKeyFactory ()
		{
		}

		/// <summary>
		/// Create a new <see cref="IDkimPublicKey"/> instance.
		/// </summary>
		/// <param name="key">The public key.</param>
		/// <returns>A new <see cref="IDkimPublicKey"/> based on the public key provided.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// <paramref name="key"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// <paramref name="key"/> is not a public key.
		/// </exception>
		public IDkimPublicKey Create (AsymmetricKeyParameter key)
		{
			return new BouncyCastleDkimPublicKey (key);
		}

		static AsymmetricKeyParameter LoadRsaKey (string keyData)
		{
			var data = "-----BEGIN PUBLIC KEY-----\r\n" + keyData + "\r\n-----END PUBLIC KEY-----\r\n";
			var rawData = Encoding.ASCII.GetBytes (data);

			using (var stream = new MemoryStream (rawData, false)) {
				using (var reader = new StreamReader (stream)) {
					var pem = new PemReader (reader);

					return pem.ReadObject () as AsymmetricKeyParameter;
				}
			}
		}

		/// <summary>
		/// Create a new instance of an <see cref="IDkimPublicKey"/>.
		/// </summary>
		/// <remarks>
		/// <para>Creates a new instance of an <see cref="IDkimPublicKey"/> based on the parameters provided.</para>
		/// <para>The <paramref name="algorithm"/> string should be the <c>k</c> parameter value from a DNS DKIM TXT
		/// record while the <paramref name="keyData"/> string should be the <c>p</c> parameter value which in general
		/// will be the base64 encoded key data.</para>
		/// </remarks>
		/// <param name="algorithm">The public key algorithm.</param>
		/// <param name="keyData">The base64 encoded public key data.</param>
		/// <returns>A new instance of an <see cref="IDkimPublicKey"/>.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// <para><paramref name="algorithm"/> is <c>null</c>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="keyData"/> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <paramref name="algorithm"/> is not supported.
		/// </exception>
		public IDkimPublicKey CreatePublicKey (string algorithm, string keyData)
		{
			AsymmetricKeyParameter pubkey = null;

			if (algorithm.Equals ("ed25519", StringComparison.OrdinalIgnoreCase)) {
				var decoded = Convert.FromBase64String (keyData);

				pubkey = new Ed25519PublicKeyParameters (decoded, 0);
			} else if (algorithm.Equals ("rsa", StringComparison.OrdinalIgnoreCase)) {
				pubkey = LoadRsaKey (keyData);
			}

			if (pubkey is null)
				throw new NotSupportedException ("The public key algorithm is not supported.");

			return new BouncyCastleDkimPublicKey (pubkey);
		}

		/// <summary>
		/// Create a new instance of an <see cref="IDkimPublicKey"/>.
		/// </summary>
		/// <remarks>
		/// <para>Creates a new instance of an <see cref="IDkimPublicKey"/> based on the parameters provided.</para>
		/// <para>The <paramref name="algorithm"/> string should be the <c>k</c> parameter value from a DNS DKIM TXT
		/// record while the <paramref name="keyData"/> string should be the <c>p</c> parameter value which in general
		/// will be the base64 encoded key data.</para>
		/// </remarks>
		/// <param name="algorithm">The public key algorithm.</param>
		/// <param name="keyData">The encoded content of the public key.</param>
		/// <returns>A new instance of an <see cref="IDkimPublicKey"/>.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// <para><paramref name="algorithm"/> is <c>null</c>.</para>
		/// <para>-or-</para>
		/// <para><paramref name="keyData"/> is <c>null</c>.</para>
		/// </exception>
		/// <exception cref="System.NotSupportedException">
		/// The <paramref name="algorithm"/> is not supported.
		/// </exception>
		public IDkimPublicKey CreatePublicKey (DkimPublicKeyAlgorithm algorithm, string keyData)
		{
			AsymmetricKeyParameter pubkey = null;

			switch (algorithm) {
			case DkimPublicKeyAlgorithm.Ed25519:
				var decoded = Convert.FromBase64String (keyData);

				pubkey = new Ed25519PublicKeyParameters (decoded, 0);
				break;
			case DkimPublicKeyAlgorithm.Rsa:
				pubkey = LoadRsaKey (keyData);
				break;
			}

			if (pubkey is null)
				throw new NotSupportedException ("The public key algorithm is not supported.");

			return new BouncyCastleDkimPublicKey (pubkey);
		}
	}
}
