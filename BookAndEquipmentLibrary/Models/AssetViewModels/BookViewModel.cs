using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models.AssetViewModels
{
    public class BookViewModel
    {
        [Key]
        public int AssetId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Author { get; set; }

        [StringLength(200, ErrorMessage = "{0} chars is max length")]
        public string Publisher { get; set; }

        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        [Display(Name = "ISBN #")]
        public string ISBN { get; set; }

        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        [Display(Name = "DDC")]
        public string DeweyIndex { get; set; }

        public string Description { get; set; }

        public int? Year { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}