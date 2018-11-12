namespace CryptographyExample.Services
{
	/// <summary>
	/// Represents a logger service.
	/// </summary>
	public interface ILoggerService
	{
		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <param name="message">The message.</param>
		void LogError(string message);

		/// <summary>
		/// Logs the information.
		/// </summary>
		/// <param name="message">The message.</param>
		void LogInformation(string message);

		/// <summary>
		/// Logs the warning.
		/// </summary>
		/// <param name="message">The message.</param>
		void LogWarning(string message);
	}
}