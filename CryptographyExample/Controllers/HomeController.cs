using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CryptographyExample.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CryptographyExample.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			// generate a 128-bit salt using a secure PRNG
			var salt = new byte[128 / 8];

			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}

			Debug.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

			// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
			var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2("some text password here", salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8));

			return View();
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
