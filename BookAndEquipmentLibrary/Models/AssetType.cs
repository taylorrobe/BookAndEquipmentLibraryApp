using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAndEquipmentLibrary.Models
{
    public class AssetType
    {
        public int AssetTypeId { get; set; }
        [Required]
        [StringLength(100,ErrorMessage = "{0} chars is max length")]
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        [Display(Name = "Asset Type")]
        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1} {2}", AssetTypeId, Name, Description);
            }
        }
        public virtual IEnumerable<Asset> Assets { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}