namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Feedbacks");
        }
    }
}
