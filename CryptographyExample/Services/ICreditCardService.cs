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
using CryptographyExample.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
		/// <param name="company">The company.</param>
		/// <param name="creditCard">The credit card.</param>
		/// <param name="cvcCode">The CVC code.</param>
		/// <returns>Returns the id of the created credit card.</returns>
		Task<CreditCard> CreateCreditCardAsync(string company, string creditCard, string cvcCode);

		/// <summary>
		/// Gets the credit card asynchronously.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns the credit card or null if no credit card record is  found.</returns>
		Task<CreditCard> GetCreditCardAsync(Guid id);

		/// <summary>
		/// Finds a list of credit cards which match the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="count">The count.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>Returns a list of credit cards which match the specified expression.</returns>
		Task<IEnumerable<CreditCard>> QueryAsync(Expression<Func<CreditCard, bool>> expression, int? count, int offset);
	}
}