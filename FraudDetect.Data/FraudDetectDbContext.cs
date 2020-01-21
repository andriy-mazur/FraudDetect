namespace FraudDetect.Data
{
    using FraudDetect.Interface.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Configuration;
    using System.Diagnostics;

    public class FraudDetectDbContext : DbContext, IFraudDetectDbContext
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Response> Logs { get; set; }

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var a = ConfigurationManager.ConnectionStrings["FraudDetect"].ConnectionString;
            Debug.Write(a);
            options.UseSqlServer(ConfigurationManager.ConnectionStrings["FraudDetect"].ConnectionString);
            //options.UseSqlServer("Server=.;Database=Fraud;Trusted_Connection=True;");
            //options.UseSqlServer("Server=127.0.0.1;Database=fraud;User Id=fd2020;Password=Mpg0*1Kwh07w;");
        }
    }
}