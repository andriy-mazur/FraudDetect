namespace FraudDetect.WebApi.Services
{
    using System;
    using FraudDetect.Data;
    using FraudDetect.Interface;
    using FraudDetect.Interface.Model;

    public static class DbLog
    {
        public static void LogAsync(string json, SourceType source)
        {
            LogAsync(json, source, DateTime.Now);
        }


        public static async void LogAsync(string json, SourceType source, DateTime date)
        {
            await using var context = new FraudDetectDbContext();

            var log = new Log
            {
                LogDate = date,
                Source = source.ToString(),
                Message = json
            };

            context.Logs.Add(log);
            context.Save();
        }
    }
}