using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    public class NpgsqlContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public NpgsqlContext() : base("name=NpgsqlContext")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 0;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeteoData>().Property(m => m.Latitude).HasPrecision(29, 19);
            modelBuilder.Entity<MeteoData>().Property(m => m.Longitude).HasPrecision(29, 19);
            modelBuilder.Entity<MeteoData>().Property(m => m.Value).HasPrecision(29, 19);

            modelBuilder.Entity<PVSystemMaterial>().Property(m => m.Efficiency).HasPrecision(29, 19);
            modelBuilder.Entity<PVSystemMaterial>().Property(m => m.ThermalPowerFactor).HasPrecision(29, 19);

            modelBuilder.Entity<SPP>().Property(s => s.Power).HasPrecision(29, 19);
            modelBuilder.Entity<SPP>().Property(s => s.CapacityFactor).HasPrecision(29, 19);

            modelBuilder.Entity<Province>().Property(p => p.Max_longitude).HasPrecision(29, 19);
            modelBuilder.Entity<Province>().Property(p => p.Min_longitude).HasPrecision(29, 19);
            modelBuilder.Entity<Province>().Property(p => p.Max_latitude).HasPrecision(29, 19);
            modelBuilder.Entity<Province>().Property(p => p.Min_latitude).HasPrecision(29, 19);
        }

        public System.Data.Entity.DbSet<AtlasSolar.Models.MeteoDataPeriodicity> MeteoDataPeriodicities { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.MeteoDataSource> MeteoDataSources { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.MeteoDataType> MeteoDataTypes { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.MeteoData> MeteoDatas { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.ApplianceType> ApplianceTypes { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Appliance> Appliances { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Option> Options { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.PVSystemMaterial> PVSystemMaterials { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.SystemLog> SystemLogs { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.SPPStatus> SPPStatus { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.SPPPurpose> SPPPurposes { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.PanelOrientation> PanelOrientations { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.SPP> SPPs { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Statistic> Statistics { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Leprng> Leprngs { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Leptype> Leptypes { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Lepmtrl> Lepmtrls { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Lepinsttp> Lepinsttps { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Leprnzon> Leprnzons { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.MapDescription> MapDescriptions { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Province> Provinces { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Lesfondtype> Lesfondtypes { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.LesfondOrgup> LesfondOrgups { get; set; }

        public System.Data.Entity.DbSet<AtlasSolar.Models.Feedback> Feedbacks { get; set; }
    }
}
