namespace FraudDetect.Data
{
    using FraudDetect.Interface.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Configuration;
    using System.Linq;

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
            TruncateStringForChangedEntities();
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //TODO Get from settings
            options.UseSqlServer("Server=frauddetectdbserver.database.windows.net;Database=fraud;User Id=andriy;Password=Mpg0*1Kwh07w;");
            //options.UseSqlServer("Server=127.0.0.1;Database=fraud;User Id=andriy;Password=Mpg0*1Kwh07w;");
            //options.UseSqlServer("Server =.; Database = Fraud; Trusted_Connection = True;");


            //var a = ConfigurationManager.ConnectionStrings["FraudDetect"];
            //var b = a.ConnectionString;
            //options.UseSqlServer(ConfigurationManager.ConnectionStrings["FraudDetect"].ConnectionString);
            //options.UseSqlServer(connectionString);
            //options.UseSqlServer("Server=127.0.0.1;Database=fraud;User Id=fd2020;Password=Mpg0*1Kwh07w;");
        }

        private void TruncateStringForChangedEntities()
        {
            var stringPropertiesWithLengthLimitations = Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string))
                .Select(z => new
                {
                    StringLength = z.GetMaxLength(),
                    ParentName = z.DeclaringEntityType.Name,
                    PropertyName = z.Name
                })
                .Where(d => d.StringLength.HasValue);


            var editedEntitiesInTheDbContextGraph = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(x => x.Entity);


            foreach (var entity in editedEntitiesInTheDbContextGraph)
            {
                var entityFields = stringPropertiesWithLengthLimitations.Where(d => d.ParentName == entity.GetType().FullName);

                foreach (var property in entityFields)
                {
                    var prop = entity.GetType().GetProperty(property.PropertyName);

                    if (prop == null) continue;

                    var originalValue = prop.GetValue(entity) as string;
                    if (originalValue == null) continue;

                    if (originalValue.Length > property.StringLength)
                    {
                        prop.SetValue(entity, originalValue.Substring(0, property.StringLength.Value));
                    }
                }
            }
        }
    }
}