namespace Signatory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Repositories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccessToken = c.String(),
                        RequireCla = c.Boolean(nullable: false),
                        LicenseText = c.String(),
                        Name = c.String(),
                        Owner = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Signatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RepositoryId = c.Int(nullable: false),
                        Address = c.String(),
                        Country = c.String(),
                        DateSigned = c.DateTime(nullable: false),
                        Email = c.String(),
                        FullName = c.String(),
                        SignatureImage = c.String(),
                        TelephoneNumber = c.String(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Repositories", t => t.RepositoryId, cascadeDelete: true)
                .Index(t => t.RepositoryId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Signatures", new[] { "RepositoryId" });
            DropForeignKey("dbo.Signatures", "RepositoryId", "dbo.Repositories");
            DropTable("dbo.Signatures");
            DropTable("dbo.Repositories");
        }
    }
}
