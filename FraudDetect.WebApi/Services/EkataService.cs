namespace FraudDetect.WebApi.Services
{
    using System;
    using System.Threading.Tasks;
    using FraudDetect.Interface;
    using FraudDetect.Interface.Model;

    public class EkataService : IBureauService
    {
        public async Task<Response> GetScoreAsync(Request request)
        {
            throw new NotImplementedException();
        }
    }
}