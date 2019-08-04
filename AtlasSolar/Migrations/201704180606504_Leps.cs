namespace AtlasSolar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Leps : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Lepinsttps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Lepmtrls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Leprnzons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Leptypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        NameEN = c.String(),
                        NameKZ = c.String(),
                        NameRU = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Leptypes");
            DropTable("public.Leprnzons");
            DropTable("public.Lepmtrls");
            DropTable("public.Lepinsttps");
        }
    }
}
