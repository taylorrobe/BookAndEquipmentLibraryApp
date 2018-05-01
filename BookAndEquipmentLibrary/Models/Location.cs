using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAndEquipmentLibrary.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        [Display(Name = "Location")]
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        [Display(Name = "Location")]
        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1}, {2}", LocationId, Name, Description);
            }
        }

        public virtual IEnumerable<Asset> Assets { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}