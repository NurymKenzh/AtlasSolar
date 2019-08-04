namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Province : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Provinces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                        Max_auto_dist = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Min_auto_dist = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Max_lep_dist = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Min_lep_dist = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Max_np_dist = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Min_np_dist = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Max_slope_srtm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Min_slope_srtm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Max_srtm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Min_srtm = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Max_swvdwnyear = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Min_swvdwnyear = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Provinces");
        }
    }
}
