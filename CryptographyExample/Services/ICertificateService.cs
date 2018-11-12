using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CryptographyExample.Models.DbModels;

namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a certificate service.
	/// </summary>
	public interface ICertificateService
	{
		/// <summary>
		/// Creates the certificate.
		/// </summary>
		/// <param name="holderName">Name of the holder.</param>
		/// <returns>Returns the created certificate.</returns>
		Task<Certificate> CreateCertificateAsync(string holderName);

		/// <summary>
		/// Gets the certificate.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Returns the certificate.</returns>
		Task<Certificate> GetCertificateAsync(Guid id);

		/// <summary>
		/// Finds a list of certificates which match the given expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="count">The count.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>Returns a list of certificates which match the given expression.</returns>
		Task<IEnumerable<Certificate>> QueryAsync(Expression<Func<Certificate, bool>> expression, int? count, int offset);


	}
}
