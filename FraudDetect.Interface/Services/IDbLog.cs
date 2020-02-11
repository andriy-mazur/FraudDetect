namespace FraudDetect.Interface.Services
{
    using System;

    public interface IDbLog
    {
        void LogAsync(string text, SourceType source);


        void LogAsync(string text, SourceType source, DateTime logDate);
    }
}