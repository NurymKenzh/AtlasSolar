namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statistic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        MeteoDataSourceId = c.Int(nullable: false),
                        MeteoDataPeriodicityId = c.Int(nullable: false),
                        MeteoDataTypesCount = c.Int(nullable: false),
                        DataCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.MeteoDataPeriodicities", t => t.MeteoDataPeriodicityId, cascadeDelete: true)
                .ForeignKey("public.MeteoDataSources", t => t.MeteoDataSourceId, cascadeDelete: true)
                .Index(t => t.MeteoDataSourceId)
                .Index(t => t.MeteoDataPeriodicityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Statistics", "MeteoDataSourceId", "public.MeteoDataSources");
            DropForeignKey("public.Statistics", "MeteoDataPeriodicityId", "public.MeteoDataPeriodicities");
            DropIndex("public.Statistics", new[] { "MeteoDataPeriodicityId" });
            DropIndex("public.Statistics", new[] { "MeteoDataSourceId" });
            DropTable("public.Statistics");
        }
    }
}
