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
 * Date: 2018-11-5
 */
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