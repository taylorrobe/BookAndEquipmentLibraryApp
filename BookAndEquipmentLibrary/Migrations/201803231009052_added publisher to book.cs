namespace BookAndEquipmentLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpublishertobook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "Publisher", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "Publisher");
        }
    }
}
