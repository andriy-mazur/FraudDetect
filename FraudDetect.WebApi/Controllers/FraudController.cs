namespace FraudDetect.WebApi.Controllers
{
    using System;
    using System.Configuration;
    using FraudDetect.Data;
    using FraudDetect.Interface;
    using FraudDetect.Interface.Model;
    using FraudDetect.Interface.TypeForm;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    [ApiController]
    [Route("[controller]")]
    public class FraudController : ControllerBase
    {
        //private readonly ILogger<FraudController> _logger;
        //private readonly IConfiguration _config;
        private readonly FraudDetectDbContext context;

        public FraudController(
            //ILogger<FraudController> logger, 
            //IConfiguration config,
            FraudDetectDbContext context)
        {
            //_logger = logger;
            //_config = config;
            this.context = context;
        }

        [HttpPost]
        public FraudResponse Get(FraudRequest request)
        {
            if(request == null) return new FraudResponse{IsFraud = true, Description = "Empty request"};
            
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
            var log = new Log
            {
                LogDate = DateTime.Now,
                Source = SourceType.TypeForm.ToString(),
                Message = payload
            };

            context.Logs.Add(log);
            context.Save();

            var request = JsonConvert.DeserializeObject<TypeFormData>(payload);
        }
    }
}