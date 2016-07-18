using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace YourProject.Models
{
    [Table("CurrentADUserContact")]
    public partial class CurrentADUserContact
    {
        [Key]
        [StringLength(100)]
        public string AliasId { get; set; }

        [StringLength(500)]
        public string DisplayName { get; set; }

        [StringLength(500)]
        public string DistinguishedName { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(50)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string CountryOrRegion { get; set; }

        [StringLength(200)]
        public string Company { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        [StringLength(255)]
        public string Building { get; set; }

        [StringLength(255)]
        public string BusinessEmail { get; set; }

        [StringLength(255)]
        public string BusinessPhone1 { get; set; }

        [StringLength(255)]
        public string BusinessPhone2 { get; set; }

        [StringLength(255)]
        public string HomePhone1 { get; set; }

        [StringLength(255)]
        public string HomePhone2 { get; set; }

        [StringLength(255)]
        public string MobilePhone { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        public string StreetAddress { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RowState { get; set; }
    }
}