using System;

namespace Ligric.Service.CryptoApisService.Infrastructure.Persistence.Configurations
{
	public class InvalidCommandException : Exception
	{
		public string Details { get; }
		public InvalidCommandException(string message, string details) : base(message)
		{
			Details = details;
		}
	}
}