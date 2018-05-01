namespace BookAndEquipmentLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        AssetId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Year = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Author = c.String(maxLength: 100),
                        ISBN = c.String(maxLength: 100),
                        DeweyIndex = c.String(maxLength: 100),
                        AssetTypeId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Status_StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssetId)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.Status_StatusId, cascadeDelete: true)
                .ForeignKey("dbo.AssetTypes", t => t.AssetTypeId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.AssetTypeId)
                .Index(t => t.Status_StatusId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.AssetTypes",
                c => new
                    {
                        AssetTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.AssetTypeId);
            
            CreateTable(
                "dbo.CheckoutHistories",
                c => new
                    {
                        CheckoutHistoryId = c.Int(nullable: false, identity: true),
                        AssetId = c.Int(nullable: false),
                        PatronId = c.Int(nullable: false),
                        CheckedOutDate = c.DateTime(nullable: false),
                        CheckedInDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CheckoutHistoryId)
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .ForeignKey("dbo.Patrons", t => t.PatronId, cascadeDelete: true)
                .Index(t => t.AssetId)
                .Index(t => t.PatronId);
            
            CreateTable(
                "dbo.Patrons",
                c => new
                    {
                        PatronId = c.Int(nullable: false, identity: true),
                        Forename = c.String(nullable: false, maxLength: 100),
                        Surname = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(nullable: false, maxLength: 100),
                        EmailAddress = c.String(nullable: false, maxLength: 100),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PatronId);
            
            CreateTable(
                "dbo.Checkouts",
                c => new
                    {
                        CheckoutId = c.Int(nullable: false, identity: true),
                        AssetId = c.Int(nullable: false),
                        PatronId = c.Int(nullable: false),
                        Checkoutdate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CheckoutId)
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .ForeignKey("dbo.Patrons", t => t.PatronId, cascadeDelete: true)
                .Index(t => t.AssetId)
                .Index(t => t.PatronId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Checkouts", "PatronId", "dbo.Patrons");
            DropForeignKey("dbo.Checkouts", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.CheckoutHistories", "PatronId", "dbo.Patrons");
            DropForeignKey("dbo.CheckoutHistories", "AssetId", "dbo.Assets");
            DropForeignKey("dbo.Assets", "AssetTypeId", "dbo.AssetTypes");
            DropForeignKey("dbo.Assets", "Status_StatusId", "dbo.Status");
            DropForeignKey("dbo.Assets", "LocationId", "dbo.Locations");
            DropIndex("dbo.Checkouts", new[] { "PatronId" });
            DropIndex("dbo.Checkouts", new[] { "AssetId" });
            DropIndex("dbo.CheckoutHistories", new[] { "PatronId" });
            DropIndex("dbo.CheckoutHistories", new[] { "AssetId" });
            DropIndex("dbo.Assets", new[] { "Status_StatusId" });
            DropIndex("dbo.Assets", new[] { "AssetTypeId" });
            DropIndex("dbo.Assets", new[] { "LocationId" });
            DropTable("dbo.Checkouts");
            DropTable("dbo.Patrons");
            DropTable("dbo.CheckoutHistories");
            DropTable("dbo.AssetTypes");
            DropTable("dbo.Status");
            DropTable("dbo.Locations");
            DropTable("dbo.Assets");
        }
    }
}
