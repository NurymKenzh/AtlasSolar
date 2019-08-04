namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Appliance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Appliances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplianceTypeId = c.Int(nullable: false),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                        Power = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.ApplianceTypes", t => t.ApplianceTypeId, cascadeDelete: true)
                .Index(t => t.ApplianceTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Appliances", "ApplianceTypeId", "public.ApplianceTypes");
            DropIndex("public.Appliances", new[] { "ApplianceTypeId" });
            DropTable("public.Appliances");
        }
    }
}
