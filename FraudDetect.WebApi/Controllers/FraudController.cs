namespace FraudDetect.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
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

            //    var arr = new List<Request>
            //    {
            //        new Request { FirstName = "Laura", LastName = "Kelly", Email = "C14laura.kelly@yahoo.com", Phone = "9257195164"},
            //    new Request { FirstName = "Terry", LastName = "Fields", Email = "tfields100@aol.com", Phone = "3039180056" },
            //    new Request { FirstName = "Marissa", LastName = "Ballesteros", Email = "Marissa1103@hotmail.com", Phone = "9732209262" },
            //    new Request { FirstName = "Deanna", LastName = "Shaw", Email = "deanna_shaw@hotmail.com", Phone = "4086038871" },
            //    new Request { FirstName = "Ruiting", LastName = "Ye", Email = "aliceyrt@hotmail.com", Phone = "5105015230" },
            //    new Request { FirstName = "Alena", LastName = "Gromova", Email = "Algromova@yahoo.com", Phone = "6502081634" },
            //    new Request { FirstName = "eric", LastName = "moler", Email = "emoler@aol.com", Phone = "7708833570" },
            //    new Request { FirstName = "Philippe", LastName = "Retornaz", Email = "philippe.retornaz@gmail.com", Phone = "6282369153" },
            //    new Request { FirstName = "Marquasia", LastName = "Hopkins", Email = "Marquasia@rocketmail.com", Phone = "8562462191" },
            //    new Request { FirstName = "Marquasia", LastName = "Hopkins", Email = "marquasia@rocketmail.com", Phone = "8562462191" },
            //    new Request { FirstName = "Kelsey", LastName = "Torres", Email = "Kelsey@fitplanapp.com", Phone = "6233376280" },
            //    new Request { FirstName = "Joshua", LastName = "Kohan", Email = "Joshuakohan1@gmail.com", Phone = "9178046203" },
            //    new Request { FirstName = "Lauren", LastName = "Bond", Email = "Bondlaurenkelley@gmail.com", Phone = "9512953918" },
            //    new Request { FirstName = "Ann", LastName = "Morrow", Email = "Morrowann@outlook.com", Phone = "3107703024" },
            //    new Request { FirstName = "Cristal", LastName = "Gamez", Email = "cggamez@cpp.edu", Phone = "3234569389" },
            //    new Request { FirstName = "Jestin", LastName = "Woods", Email = "jestin.woods@gmail.com", Phone = "9714099612" },
            //    new Request { FirstName = "Ricardo", LastName = "Montiel Argumedo", Email = "Ricardo.montiela@gmail.com", Phone = "525534332955" },
            //    new Request { FirstName = "Robert", LastName = "Kang", Email = "wrkang@gmail.com", Phone = "2024094826" },
            //    new Request { FirstName = "Trung", LastName = "Nguyen", Email = "ndstyles@gmail.com", Phone = "8326335373" },
            //    new Request { FirstName = "Brian", LastName = "Abarquez", Email = "babarquez727@gmail.com", Phone = "9083073294" },
            //    new Request { FirstName = "Jason", LastName = "James", Email = "Jasonajames@live.com", Phone = "9512819986" },
            //    new Request { FirstName = "Maricris", LastName = "Baronia", Email = "Maricris.baronia@gmail.com", Phone = "9096786187" },
            //    new Request { FirstName = "Mark", LastName = "Lakerd", Email = "mlakers@af-advisors.com", Phone = "4026708265" },
            //    new Request { FirstName = "Fey", LastName = "Keng", Email = "Feykeng@gmail.com", Phone = "8325633127" },
            //    new Request { FirstName = "Alissa", LastName = "Cooper", Email = "Alissacooper423@gmail.com", Phone = "7329865100" },
            //    new Request { FirstName = "andrea", LastName = "stanley", Email = "studio.andreastanley@gmail.com", Phone = "9178067434" },
            //    new Request { FirstName = "Kenda", LastName = "Stewart", Email = "Kenda246@gmail.com", Phone = "6178942349" },
            //    new Request { FirstName = "Parker", LastName = "Smith", Email = "laxthunder43@gmail.com", Phone = "8014048078" },
            //    new Request { FirstName = "kirsten", LastName = "getzelman", Email = "Kirstengetzelman@me.com", Phone = "3107178050" },
            //    new Request { FirstName = "Andrew", LastName = "Pearce", Email = "andrewpearce@axisstudiosgroup.com", Phone = "447946348661" },
            //    new Request { FirstName = "Adolfo", LastName = "Deli", Email = "Aldeli6405@gmail.com", Phone = "4168070042" },
            //    new Request { FirstName = "Andrew", LastName = "Fisher", Email = "Fisherandrew4343@yahoo.com", Phone = "3218633235" },
            //    new Request { FirstName = "Yasser", LastName = "Wassef", Email = "Yasser@retailrealm.com", Phone = "7039096976" },
            //    new Request { FirstName = "Abrar", LastName = "Zafar", Email = "Yonis02@gmail.com", Phone = "8583809449" },
            //    new Request { FirstName = "Noushin", LastName = "Ahdoot", Email = "Noushinahdoot@gmail.com", Phone = "7148011430" },
            //    new Request { FirstName = "Meher", LastName = "Irani", Email = "Mirani@sgu.edu", Phone = "9143254868" },
            //    new Request { FirstName = "Ka Chon", LastName = "Wong", Email = "junwong333@gmail.com", Phone = "62019300" },
            //    new Request { FirstName = "Tai", LastName = "Nguyen", Email = "taitn22@gmail.com", Phone = "4088928500" },
            //    new Request { FirstName = "Matthew", LastName = "Bradshaw", Email = "matthew.d.bradshaw@bath.edu", Phone = "7881252527" },
            //    new Request { FirstName = "Cristal", LastName = "Vazquez", Email = "cristalv05@gmail.com", Phone = "8327486636" },
            //    new Request { FirstName = "Janet", LastName = "Sample", Email = "jlynnsample64@gmail.com", Phone = "5099102910" },
            //    new Request { FirstName = "Karan", LastName = "Sharma", Email = "Karansharma20@hotmail.com", Phone = "6478714010" },
            //    new Request { FirstName = "Daniel", LastName = "Giltner", Email = "giltner.daniel@gmail.com", Phone = "3176254829" },
            //    new Request { FirstName = "Nikita", LastName = "Reva", Email = "nikita.reva@gmail.com", Phone = "4243910376" },
            //    new Request { FirstName = "David", LastName = "Berg", Email = "airberg600@gmail.com", Phone = "3016510387" },
            //    new Request { FirstName = "Stephen", LastName = "Oakley", Email = "Herstleyoak@gmail.com", Phone = "447463169623" },
            //    new Request { FirstName = "Justine", LastName = "LaMont", Email = "justine.lamont@nbcuni.com", Phone = "7608856466" },
            //    new Request { FirstName = "Maria", LastName = "Karlsson", Email = "ewa@angsgarden.se", Phone = "46708146699" },
            //    new Request { FirstName = "Cheryl", LastName = "Mahoney", Email = "Cmahoney1031@gmail.com", Phone = "5857335137" },
            //    new Request { FirstName = "Johannes", LastName = "Zeller", Email = "johannes@johanneszeller.ch", Phone = "41793780277" },
            //    new Request { FirstName = "Olan", LastName = "Andrews", Email = "Olan.andrews@gmail.com", Phone = "3344773302" },
            //    new Request { FirstName = "Pascal", LastName = "Turbing", Email = "pascal@turbing.de", Phone = "4915116227196" },
            //    new Request { FirstName = "Jonathan", LastName = "Gregory", Email = "jonathang@jrgregory.com", Phone = "4239674278" },
            //    new Request { FirstName = "William", LastName = "Curtis", Email = "Whereswill25@yahoo.com", Phone = "9517961088" },
            //    new Request { FirstName = "Trisha", LastName = "Callella", Email = "tcallella@digitalproise.org", Phone = "7144758076" },
            //    new Request { FirstName = "Shawn", LastName = "Depew", Email = "elisempls@gmail.com", Phone = "6126444207" },
            //    new Request { FirstName = "Guanlian Gordon", LastName = "Gn", Email = "ggn@hksinc.com", Phone = "4155277090" },
            //    new Request { FirstName = "Monique", LastName = "Williams", Email = "Dmpccleaning@gmail.com", Phone = "4434739127" },
            //    new Request { FirstName = "John", LastName = "Sheedy", Email = "john@buffalomedia.com.au", Phone = "61403211972" },
            //    new Request { FirstName = "Colin", LastName = "Yip", Email = "Colinyyk@yahoo.com", Phone = "6594237177" },
            //    new Request { FirstName = "Zach", LastName = "Shiverdaker", Email = "Shiverdakerzach@gmail.com", Phone = "4085292303" },
            //    new Request { FirstName = "Manuela", LastName = "Reimann", Email = "reimann.manuela@googlemail.com", Phone = "491622384589" }
            //};

            //    foreach(var r in arr)
            //    {
            //        var bureauResult = bureauService.GetScoreAsync(r).Result;

            //        //TODO check if score == null
            //        emailSender.SendEmail("support@idfeeler.com", "andriy.mazur@gmail.com, russ@dwellson.com", "idFeeler. New  response for Check-in Form", GetEmailText(r, bureauResult));
            //    }


            var request = ParseAndGetRequest(payload, date);
            var bureauResult = bureauService.GetScoreAsync(request).Result;

            //TODO check if score == null
            emailSender.SendEmail("support@idfeeler.com", "andriy.mazur@gmail.com, russ@dwellson.com", "idFeeler. New  response for Check-in Form", GetEmailText(request, bureauResult));
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