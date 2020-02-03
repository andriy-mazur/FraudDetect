namespace FraudDetect.WebApi.Controllers
{
    using System;
    using System.Configuration;
    using System.Linq;
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
        //private readonly FraudDetectDbContext context;
        private readonly IBureauService bureauService;

        public FraudController(
            IBureauService bureauService
            )
        {
            this.bureauService = bureauService;
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
            using var context = new FraudDetectDbContext();

            var date = DateTime.Now;

            //var log = new Log
            //{
            //    LogDate = date,
            //    Source = SourceType.TypeForm.ToString(),
            //    Message = payload
            //};

            //context.Logs.Add(log);
            //context.Save();

            TypeFormData typeFormData;
            string parseError = null;

            try
            {
                typeFormData = JsonConvert.DeserializeObject<TypeFormData>(payload);
            }
            catch(Exception ex)
            {
                typeFormData = null;
                parseError = ex.Message;
            }

            var request = new Request
            {
                ExternalId = typeFormData?.EventId,
                RequestDate = date,
                Json = payload,
                ParseError = parseError,
                FirstName = GetTypeFormField(typeFormData, "38da8cda11ee96bd"),
                LastName = GetTypeFormField(typeFormData, "992fd7d939ac16f6"),
                Phone = GetTypeFormField(typeFormData, "af64d6df-4899-47b5-bf0e-ae900d754dbf"),
                Email = GetTypeFormField(typeFormData, "de28403292510ddc"),
            };

            context.Requests.Add(request);
            context.Save();
        }

        private string GetTypeFormField(TypeFormData data, string questionRef)
        {
            try
            {
                if (data.FormResponse?.Answers == null) return null;

                var answer = data.FormResponse.Answers.FirstOrDefault(a =>
                    a.Field.Ref.Equals(questionRef, StringComparison.InvariantCultureIgnoreCase));

                return answer?.FieldValue;
            }
            catch
            {
                return null;
            }
        }

        //private int GetFieldPos(TypeFormData data, string questionRef)
        //{
        //    var index = 0;

        //    foreach (var field in data.FormResponse.Definition.Fields)
        //    {
        //        if (field.Ref.Equals(questionRef, StringComparison.InvariantCultureIgnoreCase)) return index;

        //        index++;
        //    }

        //    return -1;
        //}
    }
}