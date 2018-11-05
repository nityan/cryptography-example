/*
 * Copyright 2016-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: nitya
 * Date: 2018-11-4
 */
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

		/// <summary>
		/// Gets or sets the CVC code.
		/// </summary>
		/// <value>The CVC code.</value>
		[Required]
		public byte[] CvcCode { get; set; }
	}
}
