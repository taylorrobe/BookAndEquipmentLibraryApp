using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models.AssetViewModels
{
    public class EquipmentViewModel
    {
        [Key]
        public int AssetId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? Year { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        [Display(Name = "Asset Type")]
        public int AssetTypeId { get; set; }
        public virtual AssetType AssetType { get; set; }

        [Required]
        [Display(Name= "Status")]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}