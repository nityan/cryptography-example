using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a cryptography service.
	/// </summary>
	public interface ICryptoService
	{
		/// <summary>
		/// Decrypts the content.
		/// </summary>
		/// <param name="encryptedContent">Content of the encrypted.</param>
		/// <returns>Returns the decrypted content.</returns>
		string DecryptContent(string encryptedContent);

		/// <summary>
		/// Encrypts the content.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns>Returns the encrypted content.</returns>
		string EncryptContent(string content);

		/// <summary>
		/// Signs the content.
		/// </summary>
		/// <param name="encryptedContent">The encrypted content.</param>
		/// <returns>Returns the signed content.</returns>
		string SignContent(byte[] encryptedContent);

		/// <summary>
		/// Verifies the signed content.
		/// </summary>
		/// <param name="signedContent">The signed content.</param>
		/// <returns><c>true</c> if the computed hash and the stored hash match, <c>false</c> otherwise.</returns>
		bool VerifySignedContent(string signedContent);
	}
}
