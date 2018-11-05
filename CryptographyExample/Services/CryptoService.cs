using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CryptographyExample.Data;
using CryptographyExample.Models.DbModels;
using Microsoft.Extensions.Configuration;

namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a cryptography service.
	/// </summary>
	/// <seealso cref="CryptographyExample.Services.ICryptoService" />
	public class CryptoService : ICryptoService
	{
		/// <summary>
		/// The configuration.
		/// </summary>
		private readonly IConfiguration configuration;

		/// <summary>
		/// The AES key.
		/// </summary>
		private readonly string aesKey;

		/// <summary>
		/// The AES IV.
		/// </summary>
		private readonly string aesIv;

		/// <summary>
		/// The asymmetric private key.
		/// </summary>
		private readonly string asymmetricPrivateKey;

		/// <summary>
		/// The HMAC key.
		/// </summary>
		private readonly string hmacKey;

		/// <summary>
		/// The use asymmetric encryption.
		/// </summary>
		private readonly bool useAsymmetricEncryption;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardService" /> class.
		/// </summary>
		public CryptoService(IConfiguration configuration)
		{
			this.configuration = configuration;
			this.aesKey = this.configuration.GetValue<string>("AesKey");
			this.aesIv = this.configuration.GetValue<string>("AesIv");
			this.hmacKey = this.configuration.GetValue<string>("HmacKey");
			this.useAsymmetricEncryption = this.configuration.GetValue<bool>("UseAsymmetricEncryption");
			this.asymmetricPrivateKey = this.configuration.GetValue<string>("AsymmetricPrivateKey");
		}

		/// <summary>
		/// Decrypts the content.
		/// </summary>
		/// <param name="encryptedContent">Content of the encrypted.</param>
		/// <returns>Returns the decrypted content.</returns>
		public string DecryptContent(string encryptedContent)
		{
			if (encryptedContent == null)
			{
				throw new ArgumentNullException(nameof(encryptedContent), "Value cannot be null");
			}

			string result;

			if (!this.useAsymmetricEncryption)
			{
				using (var aes = Aes.Create())
				{
					aes.Key = Convert.FromBase64String(this.aesKey);
					aes.IV = Convert.FromBase64String(this.aesIv);

					var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

					var input = Convert.FromBase64String(encryptedContent);

					result = Encoding.UTF8.GetString(decryptor.TransformFinalBlock(input, 0, input.Length));
				}
			}
			else
			{
				using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
				{
					// import the private key
					rsaCryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(asymmetricPrivateKey));

					var dataToEncrypt = Convert.FromBase64String(encryptedContent);
					var decryptedData = rsaCryptoServiceProvider.Decrypt(dataToEncrypt, true);

					result = Encoding.UTF8.GetString(decryptedData);
				}
			}

			return result;
		}

		/// <summary>
		/// Encrypts the content.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns>Returns the encrypted content.</returns>
		public string EncryptContent(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				throw new ArgumentNullException(nameof(content), "Value cannot be null");
			}

			string result;

			if (!this.useAsymmetricEncryption)
			{
				using (var aes = Aes.Create())
				{
					aes.Key = Convert.FromBase64String(this.aesKey);
					aes.IV = Convert.FromBase64String(this.aesIv);

					var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

					var input = Encoding.UTF8.GetBytes(content);

					result = Convert.ToBase64String(encryptor.TransformFinalBlock(input, 0, input.Length));
				}
			}
			else
			{
				using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
				{
					rsaCryptoServiceProvider.ImportCspBlob(Convert.FromBase64String(asymmetricPrivateKey));

					var dataToEncrypt = Encoding.UTF8.GetBytes(content);
					var encryptedData = rsaCryptoServiceProvider.Encrypt(dataToEncrypt, true);

					result = Convert.ToBase64String(encryptedData);
				}
			}

			return result;
		}

		/// <summary>
		/// Signs the content.
		/// </summary>
		/// <param name="encryptedContent">The encrypted content.</param>
		/// <returns>Returns the signed content.</returns>
		public string SignContent(byte[] encryptedContent)
		{
			string content;

			// Initialize the keyed hash object.
			using (var hmac = new HMACSHA512(Convert.FromBase64String(this.hmacKey)))
			{
				var inStream = new MemoryStream(encryptedContent);

				var hashValue = hmac.ComputeHash(inStream);

				var outStream = new MemoryStream();

				// MUST RE-POSITION the stream
				inStream.Position = 0;

				outStream.Write(hashValue, 0, hashValue.Length);

				int bytesRead;

				// read 1K at a time
				var buffer = new byte[1024];

				do
				{
					// Read from the wrapping CryptoStream.
					bytesRead = inStream.Read(buffer, 0, 1024);
					outStream.Write(buffer, 0, bytesRead);
				} while (bytesRead > 0);

				content = Convert.ToBase64String(outStream.ToArray());
			}

			if (!this.VerifySignedContent(content))
			{
				throw new InvalidOperationException("Unable to verify HMAC signature");
			}

			return content;
		}

		/// <summary>
		/// Verifies the signed content.
		/// </summary>
		/// <param name="signedContent">The signed content.</param>
		/// <returns><c>true</c> if the computed hash and the stored hash match, <c>false</c> otherwise.</returns>
		public bool VerifySignedContent(string signedContent)
		{
			var error = false;
			var status = false;

			var sourceContent = Convert.FromBase64String(signedContent);

			// Initialize the keyed hash object.
			using (var hmac = new HMACSHA512(Convert.FromBase64String(this.hmacKey)))
			using (var inStream = new MemoryStream(sourceContent))
			{
				// Create an array to hold the keyed hash value read from the content.
				var storedHash = new byte[hmac.HashSize / 8];

				// Read in the storedHash.
				inStream.Read(storedHash, 0, storedHash.Length);

				// Compute the hash of the remaining contents.
				// The stream is properly positioned at the beginning of the content, immediately after the stored hash value.
				var computedHash = hmac.ComputeHash(inStream);

				// compare the computed hash with the stored value
				for (var i = 0; i < storedHash.Length; i++)
				{
					if (computedHash[i] != storedHash[i])
					{
						error = true;
					}
				}
			}

			if (error)
			{
				Trace.TraceWarning("Hash values differ! Signed content has been tampered with!");
			}
			else
			{
				Trace.TraceWarning("Hash values are equal -- no tampering occurred.");
				status = true;
			}

			return status;
		}
	}
}
