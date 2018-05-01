using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models
{
    public class BookAndEquipmentLibraryContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BookAndEquipmentLibraryContext() : base("name=BookAndEquipmentLibraryContext")
        {
        }

        public System.Data.Entity.DbSet<BookAndEquipmentLibrary.Models.Asset> Assets { get; set; }

        public System.Data.Entity.DbSet<BookAndEquipmentLibrary.Models.Location> Locations { get; set; }

        public System.Data.Entity.DbSet<BookAndEquipmentLibrary.Models.AssetType> AssetTypes { get; set; }

        public System.Data.Entity.DbSet<BookAndEquipmentLibrary.Models.Checkout> Checkouts { get; set; }

        public System.Data.Entity.DbSet<BookAndEquipmentLibrary.Models.Patron> Patrons { get; set; }

        public System.Data.Entity.DbSet<BookAndEquipmentLibrary.Models.CheckoutHistory> CheckoutHistories { get; set; }

        public System.Data.Entity.DbSet<BookAndEquipmentLibrary.Models.Status> Status { get; set; }
        
    }
}
