namespace BookAndEquipmentLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeassetyeartonullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "Year", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "Year", c => c.Int(nullable: false));
        }
    }
}
