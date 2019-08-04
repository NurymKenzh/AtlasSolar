namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeteoData1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.MeteoData", "Longitude", c => c.Decimal(nullable: false, precision: 29, scale: 19));
            AlterColumn("public.MeteoData", "Latitude", c => c.Decimal(nullable: false, precision: 29, scale: 19));
            AlterColumn("public.MeteoData", "Value", c => c.Decimal(precision: 29, scale: 19));
        }
        
        public override void Down()
        {
            AlterColumn("public.MeteoData", "Value", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.MeteoData", "Latitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.MeteoData", "Longitude", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
