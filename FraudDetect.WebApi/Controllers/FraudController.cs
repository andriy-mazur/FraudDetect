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
        public void TypeFormInput([FromBody] object payloadObject)
        {
            var payload = payloadObject.ToString();
            
            //TODO secure TypeForm  call
            //  https://developer.typeform.com/webhooks/secure-your-webhooks/

            var date = DateTime.Now;

            dbLog.LogAsync(payload, SourceType.TypeForm, date);

            var request = ParseAndGetRequest(payload, date);

            var bureauResult = bureauService.GetScoreAsync(request).Result;

            //TODO check if score == null
            emailSender.SendEmail("support@idfeeler.com", "andriy.mazur@gmail.com, russ@dwellson.com",  "idFeeler. New  response for Check-in Form", GetEmailText(request, bureauResult));
        }

        private string GetEmailText(Request request, Response response)
        {
            const string lowRiskColor = "#6AA84F";
            const string midRiskColor = "#3C78D8";
            const string highRiskColor = "#E06666";
            const string lowRiskText = "Low Risk ( Consider Approval) - 0 to 200";
            const string midRiskText = "Neutral Risk (Consider Manual Review) - 200 to 400";
            const string highRiskText = "High Risk (Send to Manual Review) - 400 to 500";

            string riskText;
            string riskColor;

            if (response.Score.HasValue && response.Score > 400)
            {
                riskText = highRiskText;
                riskColor = highRiskColor;
            }
            else if (response.Score.HasValue && response.Score > 200)
            {
                riskText = midRiskText;
                riskColor = midRiskColor;
            }
            else
            {
                riskText = lowRiskText;
                riskColor = lowRiskColor;
            }

            riskText = response.Score.HasValue ? response.Score.ToString() + " - " + riskText : string.Empty;

            var body = "<p>Your Check-in Form has a new report:</p><p><br />First name: <strong>{FirstName}</strong><br />Last name: <strong>{LastName}</strong><br />Cell phone: <strong>{Phone}</strong><br />Email address: <strong>{Email}</strong><br />Check-in: <strong>5 PM - 11.30 PM</strong><br />Check-out: <strong>Before 10 AM</strong><br />Identity Confirmation ID: <a href=\"http://aaa.com/1.jpg\">IMG_5444.jpg</a><br />Identity Confirmation Selfie&amp;ID: <a href=\"http://aaa.com/2.jpg\">IMG_5445.HEIC</a></p><h2><strong>Risk Score: <span style=\"color: {RiskColor};\">{RiskText}</span></strong></h2><p>Team IdFeeler, <br />Risk Intelligence<br /><sub>Confidentiality Note: This email may contain confidential and/or private information, communication privileged by law. If you received this e-mail in error, any review, use, dissemination, distribution, or copying of this e-mail is strictly prohibited. Please notify us immediately of the error by return e-mail and please delete this message from your system. Thank you in advance for your cooperation.</sub></p>";

            body = SetEmailParam(body, "FirstName", request.FirstName);
            body = SetEmailParam(body, "LastName", request.LastName);
            body = SetEmailParam(body, "Phone", request.Phone);
            body = SetEmailParam(body, "Email", request.Email);
            body = SetEmailParam(body, "RiskColor", riskColor);
            body = SetEmailParam(body, "RiskText", riskText);

            return body;
        }

        private string SetEmailParam(string body, string paramName, string paramValue)
        {
            return body.Replace("{" + paramName + "}", paramValue, StringComparison.InvariantCultureIgnoreCase);
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