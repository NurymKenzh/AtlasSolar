namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SPP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.SPP",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        Power = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.String(),
                        Startup = c.Int(nullable: false),
                        Link = c.String(),
                        Customer = c.String(),
                        Investor = c.String(),
                        Executor = c.String(),
                        Name = c.String(),
                        CapacityFactor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SPPStatusId = c.Int(nullable: false),
                        SPPPurposeId = c.Int(nullable: false),
                        PanelOrientationId = c.Int(nullable: false),
                        Photo = c.Binary(),
                        Coordinates = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.PanelOrientations", t => t.PanelOrientationId, cascadeDelete: true)
                .ForeignKey("public.SPPPurposes", t => t.SPPPurposeId, cascadeDelete: true)
                .ForeignKey("public.SPPStatuses", t => t.SPPStatusId, cascadeDelete: true)
                .Index(t => t.SPPStatusId)
                .Index(t => t.SPPPurposeId)
                .Index(t => t.PanelOrientationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.SPP", "SPPStatusId", "public.SPPStatuses");
            DropForeignKey("public.SPP", "SPPPurposeId", "public.SPPPurposes");
            DropForeignKey("public.SPP", "PanelOrientationId", "public.PanelOrientations");
            DropIndex("public.SPP", new[] { "PanelOrientationId" });
            DropIndex("public.SPP", new[] { "SPPPurposeId" });
            DropIndex("public.SPP", new[] { "SPPStatusId" });
            DropTable("public.SPP");
        }
    }
}
