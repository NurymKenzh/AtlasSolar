namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeteoData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.MeteoData",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MeteoDataTypeId = c.Int(nullable: false),
                        Year = c.Int(),
                        Month = c.Int(),
                        Day = c.Int(),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Value = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.MeteoDataTypes", t => t.MeteoDataTypeId, cascadeDelete: true)
                .Index(t => t.MeteoDataTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.MeteoData", "MeteoDataTypeId", "public.MeteoDataTypes");
            DropIndex("public.MeteoData", new[] { "MeteoDataTypeId" });
            DropTable("public.MeteoData");
        }
    }
}
