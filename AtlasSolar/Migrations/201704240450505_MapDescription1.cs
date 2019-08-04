namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapDescription1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.MapDescriptions", "DescriptionEN", c => c.String());
            AddColumn("public.MapDescriptions", "DescriptionKZ", c => c.String());
            AddColumn("public.MapDescriptions", "DescriptionRU", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.MapDescriptions", "DescriptionRU");
            DropColumn("public.MapDescriptions", "DescriptionKZ");
            DropColumn("public.MapDescriptions", "DescriptionEN");
        }
    }
}
