using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAndEquipmentLibrary.Models
{
    public class Status
    {
        public int StatusId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        [Display(Name = "Status")]
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        [Display(Name = "Status")]
        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1} {2}", StatusId, Name, Description);
            }
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}