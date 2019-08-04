namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.SystemLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        UserId = c.String(),
                        User = c.String(),
                        Operation = c.String(),
                        Action = c.String(),
                        Comment = c.String(),
                        Errors = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.SystemLogs");
        }
    }
}
