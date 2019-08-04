namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Option : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Options",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        DescriptionKZ = c.String(),
                        DescriptionRU = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Options");
        }
    }
}
