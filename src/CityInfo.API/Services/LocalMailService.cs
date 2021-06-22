using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private readonly IConfiguration config;

        public LocalMailService(IConfiguration config)
        {
            this.config = config;
        }

        public void Send(string subject, string message)
        {
            // send fake email
            Debug.WriteLine(
                $"Email sent from {config["mailSettings:mailFromAddress"]} to {config["mailSettings:mailToAddress"]} from LocalMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
