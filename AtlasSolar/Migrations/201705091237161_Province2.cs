namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Province2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Provinces", "Max_longitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("public.Provinces", "Min_longitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("public.Provinces", "Max_latitude", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("public.Provinces", "Min_latitude", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("public.Provinces", "Min_latitude");
            DropColumn("public.Provinces", "Max_latitude");
            DropColumn("public.Provinces", "Min_longitude");
            DropColumn("public.Provinces", "Max_longitude");
        }
    }
}
