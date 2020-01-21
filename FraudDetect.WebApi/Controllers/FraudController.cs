namespace FraudDetect.WebApi.Controllers
{
    using System;
    using FraudDetect.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class FraudController : ControllerBase
    {
        private readonly ILogger<FraudController> _logger;

        public FraudController(ILogger<FraudController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public FraudResponse Get(FraudRequest request)
        {
            if (request.FirstName.Equals("Andriy", StringComparison.InvariantCultureIgnoreCase))
            {
                return new FraudResponse
                {
                    IsFraud = true,
                    Description = "First name is `Andriy` and we are always trust to Andriy!"
                };
            }

            return new FraudResponse
            {
                IsFraud = false,
                Description =
                    $"First name is {request.FirstName} and it is not `Andriy`. We are trusting only to Andriy!"
            };
        }

        [HttpPost("typeform")]
        public void TypeFormInput(string payload)
        {

        }
    }
}