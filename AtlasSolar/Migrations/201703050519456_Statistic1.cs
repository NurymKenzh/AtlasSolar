namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statistic1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("public.Statistics", "DataCount", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("public.Statistics", "DataCount", c => c.Int(nullable: false));
        }
    }
}
