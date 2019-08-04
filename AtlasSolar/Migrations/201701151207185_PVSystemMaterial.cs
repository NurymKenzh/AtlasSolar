namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PVSystemMaterial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.PVSystemMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Efficiency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RatedOperatingTemperature = c.Int(nullable: false),
                        ThermalPowerFactor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.PVSystemMaterials");
        }
    }
}
