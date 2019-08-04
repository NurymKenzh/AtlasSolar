namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SPPStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.SPPStatuses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.SPPStatuses");
        }
    }
}
