using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptographyExample.Models.CreditCardModels
{
	/// <summary>
	/// Represents a create credit card model.
	/// </summary>
	public class CreateCreditCardModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateCreditCardModel"/> class.
		/// </summary>
		public CreateCreditCardModel()
		{
			
		}

		/// <summary>
		/// Gets or sets the credit card.
		/// </summary>
		/// <value>The credit card.</value>
		[Required]
		[DataType(DataType.CreditCard)]
		public ulong CreditCard { get; set; }
	}
}
