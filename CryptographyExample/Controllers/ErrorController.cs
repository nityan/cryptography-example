using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CryptographyExample.Controllers
{
	/// <summary>
	/// Represents an error controller.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	public class ErrorController : Controller
	{
		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger<ErrorController> logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public ErrorController(ILogger<ErrorController> logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Displays the not found view.
		/// </summary>
		/// <returns>IActionResult.</returns>
		public IActionResult Index()
        {
			try
			{
				var exceptionHandlerFeature = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

				this.logger.LogError(exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
			}
			catch (Exception e)
			{
				this.logger.LogError($"Unable to log exception: {e}");
			}

            return View();
        }
    }
}