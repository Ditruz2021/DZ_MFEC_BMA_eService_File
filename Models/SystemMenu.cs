using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_starter.Models
{
    [Table("SystemMenu")]
    public class SystemMenu
    {
        [Key]
        public int? MenuId { get; set; }

        public int? ParentId { get; set; } // อ้างอิงเมนูหลัก

        public int? Position { get; set; } // ลำดับของเมนู

        [MaxLength(450)]
        public string MenuName { get; set; } = string.Empty;

        [MaxLength(450)]
        public string PathUrl { get; set; } = string.Empty;

        [MaxLength(450)]
        public string Icon { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        [MaxLength(450)]
        public string CreateBy { get; set; } = string.Empty;

        [MaxLength(450)]
        public string UpdateBy { get; set; } = string.Empty;

        public bool? isShow { get; set; }

        public byte? Level { get; set; } // tinyint? = byte

        [MaxLength(10)]
        public string MenuCode { get; set; } = string.Empty;
    }
}


