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

		/// <summary>
		/// Gets or sets the CVC code.
		/// </summary>
		/// <value>The CVC code.</value>
		[Required]
		[Range(000, 999)]
		public uint CvcCode { get; set; }
	}
}
