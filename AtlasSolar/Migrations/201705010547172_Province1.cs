namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Province1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.Provinces", "Max_auto_dist", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_auto_dist", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_lep_dist", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_lep_dist", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_np_dist", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_np_dist", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_slope_srtm", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_slope_srtm", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_srtm", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_srtm", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_swvdwnyear", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_swvdwnyear", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("public.Provinces", "Min_swvdwnyear", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_swvdwnyear", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_srtm", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_srtm", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_slope_srtm", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_slope_srtm", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_np_dist", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_np_dist", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_lep_dist", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_lep_dist", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Min_auto_dist", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.Provinces", "Max_auto_dist", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
