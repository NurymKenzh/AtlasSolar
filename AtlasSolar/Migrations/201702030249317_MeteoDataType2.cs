namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeteoDataType2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.MeteoDataTypes", "AdditionalKZ", c => c.String());
            AddColumn("public.MeteoDataTypes", "AdditionalRU", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.MeteoDataTypes", "AdditionalRU");
            DropColumn("public.MeteoDataTypes", "AdditionalKZ");
        }
    }
}
