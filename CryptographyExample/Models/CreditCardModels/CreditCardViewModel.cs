using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptographyExample.Models.DbModels;

namespace CryptographyExample.Models.CreditCardModels
{
	/// <summary>
	/// Represents a credit card view model.
	/// </summary>
	public class CreditCardViewModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardViewModel" /> class.
		/// </summary>
		public CreditCardViewModel()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardViewModel"/> class.
		/// </summary>
		/// <param name="creditCard">The credit card.</param>
		public CreditCardViewModel(CreditCard creditCard)
		{
			this.Id = creditCard.Id;
			this.CreationTime = creditCard.CreationTime;
			this.CvcCode = Encoding.UTF8.GetString(creditCard.CvcCode);
			this.EncryptedCreditCard = creditCard.EncryptedCreditCard;
			this.SignedCreditCard = creditCard.SignedCreditCard;
		}

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the creation time.
		/// </summary>
		/// <value>The creation time.</value>
		[Display(Name = "Creation Time")]
		public DateTime CreationTime { get; set; }

		/// <summary>
		/// Gets or sets the CVC code.
		/// </summary>
		/// <value>The CVC code.</value>
		[Display(Name = "CVC Code")]
		public string CvcCode { get; set; }

		/// <summary>
		/// Gets or sets the plain text credit card.
		/// </summary>
		/// <value>The plain text credit card.</value>
		[Display(Name = "Plain Text Credit Card")]
		public string PlainTextCreditCard { get; set; }

		/// <summary>
		/// Gets or sets the encrypted credit card.
		/// </summary>
		/// <value>The encrypted credit card.</value>
		[Display(Name = "Encrypted Credit Card")]
		public string EncryptedCreditCard { get; set; }

		/// <summary>
		/// Gets or sets the signed credit card.
		/// </summary>
		/// <value>The signed credit card.</value>
		[Display(Name = "Signed Credit Card")]
		public string SignedCreditCard { get; set; }

	}
}
