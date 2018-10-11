namespace EF.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBook : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(nullable: false),
                        ISBN = c.String(nullable: false),
                        Published = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IP = c.String(),
                        Url = c.String(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifyTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Books");
        }
    }
}
