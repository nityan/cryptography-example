using System;
using System.ComponentModel.DataAnnotations;

namespace CryptographyExample.Models.DbModels
{
	/// <summary>
	/// Represents a certificate.
	/// </summary>
	public class Certificate
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Certificate"/> class.
		/// </summary>
		public Certificate() : this(Guid.NewGuid(), DateTime.Now)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Certificate"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="creationTime">The creation time.</param>
		public Certificate(Guid id, DateTime creationTime)
		{
			this.Id = id;
			this.CreationTime = creationTime;
		}

		/// <summary>
		/// Gets or sets the authority.
		/// </summary>
		/// <value>The authority.</value>
		[StringLength(255)]
		public string Authority { get; set; }

		/// <summary>
		/// Gets or sets the name of the certificate holder.
		/// </summary>
		/// <value>The name of the certificate holder.</value>
		[Required]
		[StringLength(255)]
		public string CertificateHolderName { get; set; }

		/// <summary>
		/// Gets or sets the certificate holder public key.
		/// </summary>
		/// <value>The certificate holder public key.</value>
		public string CertificateHolderPublicKey { get; set; }

		/// <summary>
		/// Gets or sets the creation time.
		/// </summary>
		/// <value>The creation time.</value>
		[Required]
		public DateTime CreationTime { get; set; }

		/// <summary>
		/// Gets or sets the expiration date.
		/// </summary>
		/// <value>The expiration date.</value>
		public DateTime? ExpirationDate { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the initialization vector.
		/// </summary>
		/// <value>The initialization vector.</value>
		public string InitializationVector { get; set; }

		/// <summary>
		/// Gets or sets the signature.
		/// </summary>
		/// <value>The signature.</value>
		public string Signature { get; set; }
	}
}