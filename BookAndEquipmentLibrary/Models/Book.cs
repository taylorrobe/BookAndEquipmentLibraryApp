using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models
{
    public class Book : Asset
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Author { get; set; }

        [StringLength(200, ErrorMessage = "{0} chars is max length")]
        public string Publisher { get; set; }

        [NotMapped]
        public new string DisplayName
        {
            get
            {
                return string.Format("{0} - {1} {2} {3}", AssetId, Name, Author, Year);
            }
        }

        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        [Display(Name = "ISBN #")]
        public string ISBN { get; set; }

        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        [Display(Name = "DDC")]
        public string DeweyIndex { get; set; }
    }
}