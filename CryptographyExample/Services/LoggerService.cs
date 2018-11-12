using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a logger service.
	/// </summary>
	/// <seealso cref="CryptographyExample.Services.ILoggerService" />
	public class LoggerService : ILoggerService
	{
		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger<LoggerService> logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggerService"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public LoggerService(ILogger<LoggerService> logger)
		{
			this.logger = logger;
		}

		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogError(string message)
		{
			this.logger.LogError(message);
		}

		/// <summary>
		/// Logs the information.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <exception cref="NotImplementedException"></exception>
		public void LogInformation(string message)
		{
			this.logger.LogInformation(message);
		}

		/// <summary>
		/// Logs the warning.
		/// </summary>
		/// <param name="message">The message.</param>
		public void LogWarning(string message)
		{
			this.logger.LogWarning(message);
		}
	}
}
