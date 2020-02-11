namespace FraudDetect.WebApi.Controllers
{
    using System;
    using System.Linq;
    using FraudDetect.Data;
    using FraudDetect.Interface;
    using FraudDetect.Interface.Model;
    using FraudDetect.Interface.Services;
    using FraudDetect.Interface.TypeForm;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [ApiController]
    [Route("[controller]")]
    public class FraudController : ControllerBase
    {
        private readonly IBureauService bureauService;
        private readonly IDbLog dbLog;
        private readonly IEmailSender emailSender;

        public FraudController(
            IBureauService bureauService,
            IDbLog dbLog,
            IEmailSender emailSender
        )
        {
            this.bureauService = bureauService;
            this.dbLog = dbLog;
            this.emailSender = emailSender;
        }

        [HttpPost("typeform")]
        public void TypeFormInput(string payload)
        {
            var date = DateTime.Now;

            dbLog.LogAsync(payload, SourceType.TypeForm, date);

            var request = ParseAndGetRequest(payload, date);

            var bureauResult = bureauService.GetScoreAsync(request).Result;

            //TODO check if score == null
            emailSender.SendEmail("support@idfeeler", "andriy.mazur@gmail.com",  "idFeeler. New  response for Check-in Form", $"Some  test text. Score: {bureauResult.Score ?? -1}");
        }

        private Request ParseAndGetRequest(string json, DateTime date)
        {
            TypeFormData typeFormData;
            string parseError = null;

            try
            {
                typeFormData = JsonConvert.DeserializeObject<TypeFormData>(json);
            }
            catch (Exception ex)
            {
                typeFormData = null;
                parseError = ex.Message;
            }

            var request = new Request
            {
                ExternalId = typeFormData?.EventId,
                RequestDate = date,
                Json = json,
                ParseError = parseError,
                FirstName = GetTypeFormField(typeFormData, "38da8cda11ee96bd"),
                LastName = GetTypeFormField(typeFormData, "992fd7d939ac16f6"),
                Phone = GetTypeFormField(typeFormData, "af64d6df-4899-47b5-bf0e-ae900d754dbf"),
                Email = GetTypeFormField(typeFormData, "de28403292510ddc")
            };

            using var context = new FraudDetectDbContext();
            context.Requests.Add(request);
            context.Save();

            return request;
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
    }
}