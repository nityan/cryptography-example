using CryptographyExample.Models.DbModels;
using System;
using System.Threading.Tasks;

namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a credit card service.
	/// </summary>
	public interface ICreditCardService
	{
		/// <summary>
		/// Creates the credit card asynchronously.
		/// </summary>
		/// <param name="creditCard">The credit card.</param>
		/// <returns>Returns the id of the created credit card.</returns>
		Task<CreditCard> CreateCreditCardAsync(string creditCard);

		/// <summary>
		/// Decrypts the credit card.
		/// </summary>
		/// <param name="encryptedCreditCard">The encrypted credit card.</param>
		/// <param name="key">The key.</param>
		/// <param name="iv">The iv.</param>
		/// <returns>System.String.</returns>
		string DecryptCreditCard(string encryptedCreditCard, string key, string iv);

		/// <summary>
		/// Encrypts the credit card.
		/// </summary>
		/// <param name="creditCard">The credit card.</param>
		/// <param name="key">The key.</param>
		/// <param name="iv">The iv.</param>
		/// <returns>Returns the encrypted credit card.</returns>
		string EncryptCreditCard(string creditCard, string key, string iv);

		/// <summary>
		/// Gets the credit card asynchronously.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns the credit card or null if no credit card record is  found.</returns>
		Task<CreditCard> GetCreditCardAsync(Guid id);

		/// <summary>
		/// Signs the credit card.
		/// </summary>
		/// <param name="encryptedContent">Content of the encrypted.</param>
		/// <param name="key">The key.</param>
		/// <returns>Returns the signed credit card.</returns>
		string SignCreditCard(byte[] encryptedContent, string key);

		/// <summary>
		/// Verifies the signed credit card.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="signedCreditCard">The signed credit card.</param>
		/// <returns><c>true</c> if unable to verify the signed credit card, <c>false</c> otherwise.</returns>
		bool VerifySignedCreditCard(string key, string signedCreditCard);
	}
}