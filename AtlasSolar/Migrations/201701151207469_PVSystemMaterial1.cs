namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PVSystemMaterial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.PVSystemMaterials", "Efficiency", c => c.Decimal(nullable: false, precision: 29, scale: 19));
            AlterColumn("public.PVSystemMaterials", "ThermalPowerFactor", c => c.Decimal(nullable: false, precision: 29, scale: 19));
        }
        
        public override void Down()
        {
            AlterColumn("public.PVSystemMaterials", "ThermalPowerFactor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.PVSystemMaterials", "Efficiency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
