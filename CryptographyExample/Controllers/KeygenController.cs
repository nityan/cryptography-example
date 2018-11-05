using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CryptographyExample.Models.KeygenModels;
using Microsoft.AspNetCore.Mvc;

namespace CryptographyExample.Controllers
{
    public class KeygenController : Controller
    {
		public IActionResult Index()
		{
			var model = new KeygenModel();

			var hmacKey = new byte[64];

			using (var rngProvider  = new RNGCryptoServiceProvider())
			{
				rngProvider.GetBytes(hmacKey);
			}

			model.HmacKey = Convert.ToBase64String(hmacKey);

			// creates a new key and IV every time
			using (var aes = Aes.Create())
			{
				model.AesKey = Convert.ToBase64String(aes.Key);
				model.AesIv = Convert.ToBase64String(aes.IV);
			}

			using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
			{
				// export the parameters (private key)
				var parameterBlob = rsaCryptoServiceProvider.ExportCspBlob(true);

				model.AsymmetricPrivateKey = Convert.ToBase64String(parameterBlob);
			}

			return View(model);
        }
    }
}