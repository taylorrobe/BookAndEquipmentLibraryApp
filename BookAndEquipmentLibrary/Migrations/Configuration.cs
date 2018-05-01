namespace BookAndEquipmentLibrary.Migrations
{
    using BookAndEquipmentLibrary.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookAndEquipmentLibrary.Models.BookAndEquipmentLibraryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BookAndEquipmentLibrary.Models.BookAndEquipmentLibraryContext context)
        {
            context.Status.AddOrUpdate(x => x.StatusId,
                new Status() { StatusId = 1, Name = "CheckedOut", Description = "Asset is currently checked out" },
                new Status() { StatusId = 2, Name = "CheckedIn", Description = "Asset is currently checked in" },
                new Status() { StatusId = 3, Name = "Lost", Description = "Asset is currently lost" });
        }
    }
}
