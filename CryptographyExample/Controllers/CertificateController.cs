using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographyExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptographyExample.Controllers
{
	/// <summary>
	/// Represents a certificate controller.
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	public class CertificateController : Controller
	{
		/// <summary>
		/// The certificate service.
		/// </summary>
		private readonly ICertificateService certificateService;

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateController" /> class.
		/// </summary>
		/// <param name="certificateService">The certificate service.</param>
		public CertificateController(ICertificateService certificateService)
		{
			this.certificateService = certificateService;
		}

        public IActionResult Index()
        {
            return View();
        }
    }
}