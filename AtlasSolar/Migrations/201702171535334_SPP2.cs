namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SPP2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.SPP", "Power", c => c.Decimal(precision: 29, scale: 19));
            AlterColumn("public.SPP", "CapacityFactor", c => c.Decimal(precision: 29, scale: 19));
        }
        
        public override void Down()
        {
            AlterColumn("public.SPP", "CapacityFactor", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.SPP", "Power", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
