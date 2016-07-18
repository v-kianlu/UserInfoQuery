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

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RowState { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(401)]
        public string UniqueName { get; set; }
    }
}