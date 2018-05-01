using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models
{
    public class Asset
    {
        public int AssetId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Name { get; set; }

        [NotMapped]
        [Display(Name ="Asset")]
        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1}", AssetId, Name);
            }
        }

        public string Description { get; set; }

        public int? Year { get; set; }

        [Required]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [Required]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}