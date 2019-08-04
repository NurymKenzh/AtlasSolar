namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PanelOrientation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.PanelOrientations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.PanelOrientations");
        }
    }
}
