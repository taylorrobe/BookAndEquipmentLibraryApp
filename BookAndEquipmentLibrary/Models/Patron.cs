using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookAndEquipmentLibrary.Models
{
    public class Patron
    {
        public int PatronId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Forename { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Surname { get; set; }

        [NotMapped]
        [Display(Name ="Patron")]
        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1}, {2}", PatronId, Surname, Forename);
            }
        }

        [Required]
        [Phone]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "{0} chars is max length")]
        public string EmailAddress { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}