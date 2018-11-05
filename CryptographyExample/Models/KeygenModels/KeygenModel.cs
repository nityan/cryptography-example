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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptographyExample.Models.KeygenModels
{
	/// <summary>
	/// Represents a keygen model.
	/// </summary>
	public class KeygenModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeygenModel"/> class.
		/// </summary>
		public KeygenModel()
		{
			
		}

		/// <summary>
		/// Gets or sets the HMAC key.
		/// </summary>
		/// <value>The HMAC key.</value>
		[Display(Name = "HMAC Key")]
		public string HmacKey { get; set; }

		/// <summary>
		/// Gets or sets the AES key.
		/// </summary>
		/// <value>The AES key.</value>
		[Display(Name = "AES Key")]
		public string AesKey { get; set; }

		/// <summary>
		/// Gets or sets the AES IV.
		/// </summary>
		/// <value>The AES IV.</value>
		[Display(Name = "AES IV")]
		public string AesIv { get; set; }

		/// <summary>
		/// Gets or sets the asymmetric private key.
		/// </summary>
		/// <value>The asymmetric private key.</value>
		[Display(Name = "Asymmetric Private Key")]
		public string AsymmetricPrivateKey { get; set; }
	}
}
