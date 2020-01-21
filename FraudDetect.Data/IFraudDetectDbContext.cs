namespace FraudDetect.Data
{
    using FraudDetect.Interface.Model;
    using Microsoft.EntityFrameworkCore;

    public interface IFraudDetectDbContext
    {
        DbSet<Request> Requests { get; set; }

        DbSet<Response> Responses { get; set; }

        DbSet<Response> Logs { get; set; }

        void Save();
    }
}