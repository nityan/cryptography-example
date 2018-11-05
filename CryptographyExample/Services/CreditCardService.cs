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
using CryptographyExample.Data;
using CryptographyExample.Models.DbModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

		/// <summary>
		/// The crypto service.
		/// </summary>
		private readonly ICryptoService cryptoService;

		/// <summary>
		/// The data protector.
		/// </summary>
		private readonly IDataProtector dataProtector;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardService" /> class.
		/// </summary>
		/// <param name="cryptoService">The crypto service.</param>
		/// <param name="context">The context.</param>
		/// <param name="dataProtectionProvider">The data protection provider.</param>
		public CreditCardService(ICryptoService cryptoService, ApplicationDbContext context, IDataProtectionProvider dataProtectionProvider)
		{
			this.context = context;
			this.dataProtector = dataProtectionProvider.CreateProtector("CvcCodeProtector");
			this.cryptoService = cryptoService;
		}

		/// <summary>
		/// Creates the credit card asynchronously.
		/// </summary>
		/// <param name="creditCard">The credit card.</param>
		/// <param name="cvcCode">The CVC code.</param>
		/// <returns>Returns the id of the created credit card.</returns>
		public async Task<CreditCard> CreateCreditCardAsync(string creditCard, string cvcCode)
		{
			var encryptedContent = this.cryptoService.EncryptContent(creditCard);
			var signedContent = this.cryptoService.SignContent(Convert.FromBase64String(encryptedContent));

			var cc = new CreditCard(Guid.NewGuid(), DateTime.Now)
			{
				CvcCode = this.dataProtector.Protect(Encoding.UTF8.GetBytes(cvcCode)),
				EncryptedCreditCard = encryptedContent,
				SignedCreditCard = signedContent
			};

			this.context.CreditCards.Add(cc);
			await this.context.SaveChangesAsync();

			return cc;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.context?.Dispose();
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

			creditCard.CvcCode = this.dataProtector.Unprotect(creditCard.CvcCode);

			return creditCard;
		}

		/// <summary>
		/// Finds a list of credit cards which match the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="count">The count.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>Returns a list of credit cards which match the specified expression.</returns>
		public async Task<IEnumerable<CreditCard>> QueryAsync(Expression<Func<CreditCard, bool>> expression, int? count, int offset)
		{
			var creditCards = this.context.CreditCards.Where(expression);

			if (offset > 0)
			{
				creditCards = creditCards.Skip(offset);
			}

			creditCards = creditCards.Take(count ?? 25);

			foreach (var creditCard in creditCards)
			{
				creditCard.CvcCode = this.dataProtector.Unprotect(creditCard.CvcCode);
			}

			return await creditCards.ToListAsync();
		}
	}
}