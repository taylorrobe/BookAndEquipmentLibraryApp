namespace BookAndEquipmentLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNavigationForStatus : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Assets", name: "Status_StatusId", newName: "StatusId");
            RenameIndex(table: "dbo.Assets", name: "IX_Status_StatusId", newName: "IX_StatusId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Assets", name: "IX_StatusId", newName: "IX_Status_StatusId");
            RenameColumn(table: "dbo.Assets", name: "StatusId", newName: "Status_StatusId");
        }
    }
}
