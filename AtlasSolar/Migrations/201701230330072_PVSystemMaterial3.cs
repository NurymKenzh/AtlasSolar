namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PVSystemMaterial3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("public.PVSystemMaterials", "Type");
        }
        
        public override void Down()
        {
            AddColumn("public.PVSystemMaterials", "Type", c => c.String());
        }
    }
}
