using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models.CheckoutViewModels
{
    public class CheckoutAssetOrForPatron
    {
        public int CheckoutId { get; set; }

        [Required]
        
        public int AssetId { get; set; }
        [Display(Name = "Asset")]
        public virtual Asset Asset { get; set; }

        [Required]
        public int PatronId { get; set; }
        [Display(Name = "Patron")]
        public virtual Patron Patron { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckoutDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
        public string Notes { get; set; }

        public int? FixedAsset { get; set; }
        public int? FixedPatron { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}