namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Province3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.Provinces", "Max_longitude", c => c.Decimal(precision: 29, scale: 19));
            AlterColumn("public.Provinces", "Min_longitude", c => c.Decimal(precision: 29, scale: 19));
            AlterColumn("public.Provinces", "Max_latitude", c => c.Decimal(precision: 29, scale: 19));
            AlterColumn("public.Provinces", "Min_latitude", c => c.Decimal(precision: 29, scale: 19));
        }
        
        public override void Down()
        {
            AlterColumn("public.Provinces", "Min_latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_latitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_longitude", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_longitude", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
