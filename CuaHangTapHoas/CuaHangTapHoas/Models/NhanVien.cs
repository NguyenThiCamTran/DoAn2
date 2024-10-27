namespace CuaHangTapHoas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [Key]
        public int maNhanVien { get; set; }

        [StringLength(255)]
        public string tenNhanVien { get; set; }

        [StringLength(20)]
        public string soDienThoai { get; set; }

        [StringLength(255)]
        public string diaChi { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        [StringLength(255)]
        public string chucVu { get; set; }

        public decimal? luong { get; set; }

        public int? maTaiKhoan { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
