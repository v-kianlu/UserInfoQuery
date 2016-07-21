using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Models
{
    [Table("ContactGroupMember")]
    public partial class ContactGroupMember
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(200)]
        public string GroupAliasId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string MemberAliasId { get; set; }

    }
}