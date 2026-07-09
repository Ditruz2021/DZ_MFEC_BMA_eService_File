using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_starter.Models
{
    [Table("DataUsers")]
    public class DataUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserId { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(256)]
        public string NormalizedUserName { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(256)]
        public string NormalizedEmail { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string SecurityStamp { get; set; } = string.Empty;

        public string ConcurrencyStamp { get; set; } = string.Empty;

        public int? AccessFailedCount { get; set; }

        public bool IsAD { get; set; }

        public DateTime LastestLogin { get; set; }

        public int? PersonalTypeId { get; set; }

        [MaxLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        public int? TitleId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FirstNameEN { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastNameEN { get; set; } = string.Empty;

        public int? OrgStructureId { get; set; }

        public int? OrgSectionId { get; set; }

        public int? PositionId { get; set; }

        public int? ManagerId { get; set; }

        [MaxLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PhoneExtension { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        [MaxLength(200)]
        public string PersonPathImageSignature { get; set; } = string.Empty;

        public int? OrgGroupId { get; set; }

        [MaxLength(50)]
        public string LineID { get; set; } = string.Empty;
    }
}
