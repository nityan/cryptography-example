using CryptographyExample.Models.CreditCardModels;
using CryptographyExample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace CryptographyExample.Controllers
{
	/// <summary>
	/// Represents a cryptography controller.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	public class CreditCardController : Controller
	{
		/// <summary>
		/// The credit card service.
		/// </summary>
		private readonly ICreditCardService creditCardService;

		/// <summary>
		/// The crypto service.
		/// </summary>
		private readonly ICryptoService cryptoService;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardController" /> class.
		/// </summary>
		/// <param name="cryptoService">The crypto service.</param>
		/// <param name="creditCardService">The credit card service.</param>
		public CreditCardController(ICryptoService cryptoService, ICreditCardService creditCardService)
		{
			this.cryptoService = cryptoService;
			this.creditCardService = creditCardService;
		}

		/// <summary>
		/// Displays the create view.
		/// </summary>
		/// <returns>Returns an action result.</returns>
		public ActionResult Create()
		{
			return View();
		}

		/// <summary>
		/// Creates a credit card.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns an action result.</returns>
		[HttpPost]
		[ActionName("Create")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync(CreateCreditCardModel model)
		{
			try
			{
				if (!this.ModelState.IsValid)
				{
					return View(model);
				}

				var creditCard = await this.creditCardService.CreateCreditCardAsync(model.CreditCard.ToString(), model.CvcCode.ToString());

				return RedirectToAction("Details", new { id = creditCard.Id });
			}
			catch (Exception e)
			{
				this.ModelState.AddModelError("", "Unable to create credit card");
				Trace.TraceError($"Unable to create credit card: {e}");

				return View();
			}
		}

		/// <summary>
		/// Displays the details view.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns an action result.</returns>
		[ActionName("Details")]
		public async Task<ActionResult> DetailsAsync(Guid id)
		{
			CreditCardViewModel model;

			try
			{
				var creditCard = await this.creditCardService.GetCreditCardAsync(id);

				model = new CreditCardViewModel(creditCard);

				// decrypt the credit card
				model.PlainTextCreditCard = this.cryptoService.DecryptContent(model.EncryptedCreditCard);
			}
			catch (Exception e)
			{
				Trace.TraceError($"Unable to retrieve credit card: {e}");
				return this.RedirectToAction("Index");
			}

			return this.View(model);
		}

		/// <summary>
		/// Displays the index view.
		/// </summary>
		/// <returns>Returns an action result.</returns>
		[HttpGet]
		[ActionName("Index")]
		public async Task<ActionResult> IndexAsync()
		{
			var results = new List<CreditCardViewModel>();

			try
			{
				var creditCards = await this.creditCardService.QueryAsync(c => true, null, 0);

				results.AddRange(creditCards.Select(c => new CreditCardViewModel(c)).OrderByDescending(c => c.CreationTime));
			}
			catch (Exception e)
			{
				Trace.TraceError($"Unable to retrieve credit cards: {e}");
			}

			return View(results);
		}
	}
}