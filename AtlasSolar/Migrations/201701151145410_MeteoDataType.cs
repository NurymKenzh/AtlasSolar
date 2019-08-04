namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeteoDataType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.MeteoDataTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeteoDataSourceId = c.Int(nullable: false),
                        MeteoDataPeriodicityId = c.Int(nullable: false),
                        Code = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                        AdditionalEN = c.String(),
                        AdditionalKZ = c.String(),
                        AdditionalRU = c.String(),
                        GroupEN = c.String(),
                        GroupKZ = c.String(),
                        GroupRU = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.MeteoDataPeriodicities", t => t.MeteoDataPeriodicityId, cascadeDelete: true)
                .ForeignKey("public.MeteoDataSources", t => t.MeteoDataSourceId, cascadeDelete: true)
                .Index(t => t.MeteoDataSourceId)
                .Index(t => t.MeteoDataPeriodicityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.MeteoDataTypes", "MeteoDataSourceId", "public.MeteoDataSources");
            DropForeignKey("public.MeteoDataTypes", "MeteoDataPeriodicityId", "public.MeteoDataPeriodicities");
            DropIndex("public.MeteoDataTypes", new[] { "MeteoDataPeriodicityId" });
            DropIndex("public.MeteoDataTypes", new[] { "MeteoDataSourceId" });
            DropTable("public.MeteoDataTypes");
        }
    }
}
