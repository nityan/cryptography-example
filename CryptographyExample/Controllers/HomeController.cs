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

			// generate the salt value
			// generate a salt value which we will use to add "randomness" to the hash
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}

			// write the salt value to the output
			Debug.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

			// derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
			// iterate 10,000 times to create a hash of the plain text password and the salt value
			// using the PBKDF2 algorithm
			var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2("some text password here", salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8));

			Debug.WriteLine(hashed);

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
