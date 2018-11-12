using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptographyExample.Models.CreditCardModels
{
	/// <summary>
	/// Represents a search credit card model.
	/// </summary>
	public class SearchCreditCardModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SearchCreditCardModel"/> class.
		/// </summary>
		public SearchCreditCardModel()
		{

		}

		/// <summary>
		/// Gets or sets the company.
		/// </summary>
		/// <value>The company.</value>
		[Required]
		[StringLength(128)]
		public string Company { get; set; }
	}
}
