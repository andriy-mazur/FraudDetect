namespace FraudDetect.Data
{
    using FraudDetect.Interface.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Configuration;

    public class FraudDetectDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Log> Logs { get; set; }

        //public FraudDetectDbContext(DbContextOptions<FraudDetectDbContext> options) : base(options)
        //{

        //}

        //private string connectionString;

        //public FraudDetectDbContext(string connectionString) : base()
        //{
        //    this.connectionString = connectionString;
        //}

        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=frauddetectdbserver.database.windows.net;Database=fraud;User Id=andriy;Password=Mpg0*1Kwh07w;");
            //options.UseSqlServer("Server=127.0.0.1;Database=fraud;User Id=andriy;Password=Mpg0*1Kwh07w;");
            //options.UseSqlServer("Server =.; Database = Fraud; Trusted_Connection = True;");


            //var a = ConfigurationManager.ConnectionStrings["FraudDetect"];
            //var b = a.ConnectionString;
            //options.UseSqlServer(ConfigurationManager.ConnectionStrings["FraudDetect"].ConnectionString);
            //options.UseSqlServer(connectionString);
            //options.UseSqlServer("Server=127.0.0.1;Database=fraud;User Id=fd2020;Password=Mpg0*1Kwh07w;");
        }
    }
}