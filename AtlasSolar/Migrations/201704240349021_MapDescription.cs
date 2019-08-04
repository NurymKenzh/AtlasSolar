namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MapDescription : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.MapDescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LayersCode = c.String(),
                        DescriptionForUser = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                        AppointmentEN = c.String(),
                        AppointmentKZ = c.String(),
                        AppointmentRU = c.String(),
                        SourceEN = c.String(),
                        SourceKZ = c.String(),
                        SourceRU = c.String(),
                        AdditionalEN = c.String(),
                        AdditionalKZ = c.String(),
                        AdditionalRU = c.String(),
                        ResolutionEN = c.String(),
                        ResolutionKZ = c.String(),
                        ResolutionRU = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.MapDescriptions");
        }
    }
}
