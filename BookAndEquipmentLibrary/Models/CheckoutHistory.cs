using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models
{
    public class CheckoutHistory
    {
        public int CheckoutHistoryId { get; set; }

        [Required]
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        [Required]
        public int PatronId { get; set; }
        public virtual Patron Patron { get; set; }

        [Required]
        public DateTime CheckedOutDate { get; set; }

        [Required]
        public DateTime CheckedInDate { get; set; }
        public string Notes { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}