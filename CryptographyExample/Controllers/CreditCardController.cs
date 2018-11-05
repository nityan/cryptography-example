using CryptographyExample.Models.CreditCardModels;
using CryptographyExample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
		/// The configuration.
		/// </summary>
		private readonly IConfiguration configuration;

		/// <summary>
		/// The credit card service.
		/// </summary>
		private readonly ICreditCardService creditCardService;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreditCardController" /> class.
		/// </summary>
		/// <param name="creditCardService">The credit card service.</param>
		public CreditCardController(IConfiguration configuration, ICreditCardService creditCardService)
		{
			this.configuration = configuration;
			this.creditCardService = creditCardService;
		}

		// GET: Cryptography/Create
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

				var creditCard = await this.creditCardService.CreateCreditCardAsync(model.CreditCard.ToString());

				return RedirectToAction("Details", new { id = creditCard.Id });
			}
			catch
			{
				return View();
			}
		}

		/// <summary>
		/// Displays the delete view.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns an action result.</returns>
		public ActionResult Delete(int id)
		{
			return View();
		}

		/// <summary>
		/// Deletes a credit card.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="collection">The collection.</param>
		/// <returns>Returns an action result.</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction(nameof(Index));
			}
			catch
			{
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
				model.PlainTextCreditCard = this.creditCardService.DecryptCreditCard(model.EncryptedCreditCard, this.configuration.GetValue<string>("AesKey"), this.configuration.GetValue<string>("AesIv"));
			}
			catch (Exception e)
			{
				Trace.TraceError($"Unable to retrieve credit card: {e}");
				return this.RedirectToAction(nameof(Index));
			}

			return this.View(model);
		}

		// GET: Cryptography
		public ActionResult Index()
		{
			var results = new List<CreditCardViewModel>();

			return View(results);
		}
	}
}