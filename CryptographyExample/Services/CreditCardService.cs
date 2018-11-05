using CryptographyExample.Data;
using CryptographyExample.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a cryptography service.
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	/// <seealso cref="CryptographyExample.Services.ICreditCardService" />
	public class CreditCardService : ICreditCardService, IDisposable
	{
		/// <summary>
		/// The configuration.
		/// </summary>
		private readonly IConfiguration configuration;

		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

		/// <summary>
		/// The AES key.
		/// </summary>
		private readonly string aesKey;

		/// <summary>
		/// The AES IV.
		/// </summary>
		private readonly string aesIv;

		/// <summary>
		/// The HMAC key.
		/// </summary>
		private readonly string hmacKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardService" /> class.
		/// </summary>
		public CreditCardService(IConfiguration configuration, ApplicationDbContext context)
		{
			this.configuration = configuration;
			this.context = context;
			this.aesKey = this.configuration.GetValue<string>("AesKey");
			this.aesIv = this.configuration.GetValue<string>("AesIv");
			this.hmacKey = this.configuration.GetValue<string>("HmacKey");
		}

		/// <summary>
		/// Creates the credit card asynchronously.
		/// </summary>
		/// <param name="creditCard">The credit card.</param>
		/// <returns>Returns the id of the created credit card.</returns>
		public async Task<CreditCard> CreateCreditCardAsync(string creditCard)
		{
			var encryptedContent = this.EncryptCreditCard(creditCard, this.aesKey, this.aesIv);
			var signedContent = this.SignCreditCard(Convert.FromBase64String(encryptedContent), this.hmacKey);

			//if (!this.VerifySignedCreditCard(this.hmacKey, signedContent))
			//{
			//	throw new InvalidOperationException($"Unable to persist credit card, the HMAC signature does not match the signed message");
			//}

			var cc = new CreditCard(Guid.NewGuid(), DateTime.Now)
			{
				EncryptedCreditCard = encryptedContent,
				SignedCreditCard = signedContent
			};

			this.context.CreditCards.Add(cc);
			await this.context.SaveChangesAsync();

			return cc;
		}

		/// <summary>
		/// Decrypts the credit card.
		/// </summary>
		/// <param name="encryptedCreditCard">The encrypted credit card.</param>
		/// <param name="key">The key.</param>
		/// <param name="iv">The iv.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="ArgumentNullException">
		/// encryptedCreditCard - Value cannot be null
		/// or
		/// key - Value cannot be null
		/// or
		/// iv - Value cannot be null
		/// </exception>
		public string DecryptCreditCard(string encryptedCreditCard, string key, string iv)
		{
			if (encryptedCreditCard == null)
			{
				throw new ArgumentNullException(nameof(encryptedCreditCard), "Value cannot be null");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException(nameof(key), "Value cannot be null");
			}

			if (string.IsNullOrEmpty(iv))
			{
				throw new ArgumentNullException(nameof(iv), "Value cannot be null");
			}

			string content;

			using (var aes = Aes.Create())
			{
				aes.Key = Convert.FromBase64String(key);
				aes.IV = Convert.FromBase64String(iv);

				var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				var input = Convert.FromBase64String(encryptedCreditCard);

				content = Encoding.UTF8.GetString(decryptor.TransformFinalBlock(input, 0, input.Length));
			}

			return content;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.context?.Dispose();
		}

		/// <summary>
		/// Encrypts the credit card.
		/// </summary>
		/// <param name="creditCard">The credit card.</param>
		/// <param name="key">The key.</param>
		/// <param name="iv">The iv.</param>
		/// <returns>Returns the encrypted credit card.</returns>
		/// <exception cref="ArgumentNullException">
		/// creditCard - Value cannot be null
		/// or
		/// key - Value cannot be null
		/// or
		/// iv - Value cannot be null
		/// </exception>
		public string EncryptCreditCard(string creditCard, string key, string iv)
		{
			if (string.IsNullOrEmpty(creditCard))
			{
				throw new ArgumentNullException(nameof(creditCard), "Value cannot be null");
			}

			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException(nameof(key), "Value cannot be null");
			}

			if (string.IsNullOrEmpty(iv))
			{
				throw new ArgumentNullException(nameof(iv), "Value cannot be null");
			}

			string content;

			using (var aes = Aes.Create())
			{
				aes.Key = Convert.FromBase64String(key);
				aes.IV = Convert.FromBase64String(iv);

				var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				var input = Encoding.UTF8.GetBytes(creditCard);

				content = Convert.ToBase64String(encryptor.TransformFinalBlock(input, 0, input.Length));
			}

			return content;
		}

		/// <summary>
		/// Gets the credit card asynchronously.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns the credit card or null if no credit card record is  found.</returns>
		public async Task<CreditCard> GetCreditCardAsync(Guid id)
		{
			var creditCard = await this.context.CreditCards.FindAsync(id);

			if (creditCard == null)
			{
				throw new KeyNotFoundException($"Unable to find credit card using id: {id}");
			}

			return creditCard;
		}

		/// <summary>
		/// Computes a keyed hash for content and returns the signed content.
		/// </summary>
		/// <param name="encryptedContent">Content of the encrypted.</param>
		/// <param name="key">The key.</param>
		/// <returns>Returns the signed content.</returns>
		public string SignCreditCard(byte[] encryptedContent, string key)
		{
			string content;

			// Initialize the keyed hash object.
			using (var hmac = new HMACSHA512(Convert.FromBase64String(key)))
			{
				var inStream = new MemoryStream(encryptedContent);

				var hashValue = hmac.ComputeHash(inStream);

				var outStream = new MemoryStream(hashValue);

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

			return content;
		}

		/// <summary>
		/// Verifies the content.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="signedCreditCard">The signed credit card.</param>
		/// <returns><c>true</c> if the content is verified successfully, <c>false</c> otherwise.</returns>
		public bool VerifySignedCreditCard(string key, string signedCreditCard)
		{
			var error = false;
			var status = false;

			var sourceContent = Convert.FromBase64String(signedCreditCard);

			// Initialize the keyed hash object.
			using (var hmac = new HMACSHA512(Convert.FromBase64String(key)))
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