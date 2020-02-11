namespace FraudDetect.WebApi.Services
{
    using System;
    using FraudDetect.Data;
    using FraudDetect.Interface;
    using FraudDetect.Interface.Model;
    using FraudDetect.Interface.Services;

    public class DbLog : IDbLog
    {
        public void LogAsync(string text, SourceType source)
        {
            LogAsync(text, source, DateTime.Now);
        }


        public async void LogAsync(string text, SourceType source, DateTime logDate)
        {
            await using var context = new FraudDetectDbContext();

            var log = new Log
            {
                LogDate = logDate,
                Source = source.ToString(),
                Message = text
            };

            context.Logs.Add(log);
            context.Save();
        }
    }
}