using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models
{
    public class Equipment : Asset
    {
        [Required]
        public int AssetTypeId { get; set; }
        public virtual AssetType AssetType { get; set; }
    }
}