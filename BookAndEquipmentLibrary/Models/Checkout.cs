using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models
{
    public class Checkout
    {
        public int CheckoutId { get; set; }

        [Required]
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        [Required]
        public int PatronId { get; set; }
        public virtual Patron Patron { get; set; }

        [Required]
        public DateTime CheckoutDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }
        public string Notes { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}