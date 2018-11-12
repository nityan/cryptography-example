using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CryptographyExample.Data;
using CryptographyExample.Models.DbModels;

namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a certificate service.
	/// </summary>
	public class CertificateService : ICertificateService, IDisposable
	{
		/// <summary>
		/// The context.
		/// </summary>
		private readonly ApplicationDbContext context;

		/// <summary>
		/// Initializes a new instance of the <see cref="CertificateService"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public CertificateService(ApplicationDbContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Creates the certificate.
		/// </summary>
		/// <param name="holderName">Name of the holder.</param>
		/// <returns>Returns the created certificate.</returns>
		public async Task<Certificate> CreateCertificateAsync(string holderName)
		{
			var aes = Aes.Create();

			var certificate = new Certificate
			{
				Authority = "Nityan Khanna",
				CertificateHolderName = holderName,
				CertificateHolderPublicKey = Convert.ToBase64String(aes.Key),
				ExpirationDate = DateTime.Now.AddYears(5),
				InitializationVector = Convert.ToBase64String(aes.IV),
			};

			await this.context.Certificates.AddAsync(certificate);
			await this.context.SaveChangesAsync();

			return certificate;
		}

		/// <summary>
		/// Gets the certificate.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns the certificate.</returns>
		/// <exception cref="KeyNotFoundException">If the id is not found</exception>
		public async Task<Certificate> GetCertificateAsync(Guid id)
		{
			var certificate = await this.context.Certificates.FindAsync(id);

			if (certificate == null)
			{
				throw new KeyNotFoundException($"Unable to locate certificate using id: {id}");
			}

			return certificate;
		}

		/// <summary>
		/// Finds a list of certificates which match the given expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="count">The count.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>Returns a list of certificates which match the given expression.</returns>
		/// <exception cref="NotImplementedException"></exception>
		public Task<IEnumerable<Certificate>> QueryAsync(Expression<Func<Certificate, bool>> expression, int? count, int offset)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.context?.Dispose();
		}
	}
}
