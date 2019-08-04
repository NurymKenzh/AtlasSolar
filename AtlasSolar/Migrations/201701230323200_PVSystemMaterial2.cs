namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PVSystemMaterial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.PVSystemMaterials", "NameEN", c => c.String());
            AddColumn("public.PVSystemMaterials", "NameKZ", c => c.String());
            AddColumn("public.PVSystemMaterials", "NameRU", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.PVSystemMaterials", "NameRU");
            DropColumn("public.PVSystemMaterials", "NameKZ");
            DropColumn("public.PVSystemMaterials", "NameEN");
        }
    }
}
