using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CryptographyExample.Models.DbModels
{
	/// <summary>
	/// Represents a credit card.
	/// </summary>
	public class CreditCard
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCard"/> class.
		/// </summary>
		public CreditCard() : this(Guid.NewGuid(), DateTime.Now)
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCard" /> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="creationTime">The creation time.</param>
		public CreditCard(Guid id, DateTime creationTime)
		{
			this.Id = id;
			this.CreationTime = creationTime;
		}

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the creation time.
		/// </summary>
		/// <value>The creation time.</value>
		[Required]
		public DateTime CreationTime { get; set; }

		/// <summary>
		/// Gets or sets the plain text credit card.
		/// </summary>
		/// <value>The plain text credit card.</value>
		[NotMapped]
		public string PlainTextCreditCard { get; set; }

		/// <summary>
		/// Gets or sets the encrypted credit card.
		/// </summary>
		/// <value>The encrypted credit card.</value>
		[Required]
		public string EncryptedCreditCard { get; set; }

		/// <summary>
		/// Gets or sets the signed credit card.
		/// </summary>
		/// <value>The signed credit card.</value>
		[Required]
		public string SignedCreditCard { get; set; }
	}
}
