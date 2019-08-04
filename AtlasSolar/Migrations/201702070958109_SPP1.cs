namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SPP1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("public.SPP", "PanelOrientationId", "public.PanelOrientations");
            DropForeignKey("public.SPP", "SPPPurposeId", "public.SPPPurposes");
            DropForeignKey("public.SPP", "SPPStatusId", "public.SPPStatuses");
            DropIndex("public.SPP", new[] { "SPPStatusId" });
            DropIndex("public.SPP", new[] { "SPPPurposeId" });
            DropIndex("public.SPP", new[] { "PanelOrientationId" });
            AlterColumn("public.SPP", "Count", c => c.Int());
            AlterColumn("public.SPP", "Power", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.SPP", "Startup", c => c.Int());
            AlterColumn("public.SPP", "CapacityFactor", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("public.SPP", "SPPStatusId", c => c.Int());
            AlterColumn("public.SPP", "SPPPurposeId", c => c.Int());
            AlterColumn("public.SPP", "PanelOrientationId", c => c.Int());
            CreateIndex("public.SPP", "SPPStatusId");
            CreateIndex("public.SPP", "SPPPurposeId");
            CreateIndex("public.SPP", "PanelOrientationId");
            AddForeignKey("public.SPP", "PanelOrientationId", "public.PanelOrientations", "Id");
            AddForeignKey("public.SPP", "SPPPurposeId", "public.SPPPurposes", "Id");
            AddForeignKey("public.SPP", "SPPStatusId", "public.SPPStatuses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("public.SPP", "SPPStatusId", "public.SPPStatuses");
            DropForeignKey("public.SPP", "SPPPurposeId", "public.SPPPurposes");
            DropForeignKey("public.SPP", "PanelOrientationId", "public.PanelOrientations");
            DropIndex("public.SPP", new[] { "PanelOrientationId" });
            DropIndex("public.SPP", new[] { "SPPPurposeId" });
            DropIndex("public.SPP", new[] { "SPPStatusId" });
            AlterColumn("public.SPP", "PanelOrientationId", c => c.Int(nullable: false));
            AlterColumn("public.SPP", "SPPPurposeId", c => c.Int(nullable: false));
            AlterColumn("public.SPP", "SPPStatusId", c => c.Int(nullable: false));
            AlterColumn("public.SPP", "CapacityFactor", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.SPP", "Startup", c => c.Int(nullable: false));
            AlterColumn("public.SPP", "Power", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("public.SPP", "Count", c => c.Int(nullable: false));
            CreateIndex("public.SPP", "PanelOrientationId");
            CreateIndex("public.SPP", "SPPPurposeId");
            CreateIndex("public.SPP", "SPPStatusId");
            AddForeignKey("public.SPP", "SPPStatusId", "public.SPPStatuses", "Id", cascadeDelete: true);
            AddForeignKey("public.SPP", "SPPPurposeId", "public.SPPPurposes", "Id", cascadeDelete: true);
            AddForeignKey("public.SPP", "PanelOrientationId", "public.PanelOrientations", "Id", cascadeDelete: true);
        }
    }
}
