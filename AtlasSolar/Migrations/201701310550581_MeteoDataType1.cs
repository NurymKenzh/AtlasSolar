namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MeteoDataType1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.MeteoDataTypes", "DescriptionEN", c => c.String());
            AddColumn("public.MeteoDataTypes", "DescriptionKZ", c => c.String());
            AddColumn("public.MeteoDataTypes", "DescriptionRU", c => c.String());
            DropColumn("public.MeteoDataTypes", "AdditionalKZ");
            DropColumn("public.MeteoDataTypes", "AdditionalRU");
        }
        
        public override void Down()
        {
            AddColumn("public.MeteoDataTypes", "AdditionalRU", c => c.String());
            AddColumn("public.MeteoDataTypes", "AdditionalKZ", c => c.String());
            DropColumn("public.MeteoDataTypes", "DescriptionRU");
            DropColumn("public.MeteoDataTypes", "DescriptionKZ");
            DropColumn("public.MeteoDataTypes", "DescriptionEN");
        }
    }
}
