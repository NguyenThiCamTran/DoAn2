namespace CuaHangTapHoas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            HoaDons = new HashSet<HoaDon>();
            NhanViens = new HashSet<NhanVien>();
            PhieuNhapHangs = new HashSet<PhieuNhapHang>();
        }

        [Key]
        public int maTaiKhoan { get; set; }

        [Required]
        [StringLength(50)]
        public string tenDangNhap { get; set; }

        [StringLength(255)]
        public string matKhau { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(20)]
        public string soDienThoai { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngayTao { get; set; }

        [StringLength(50)]
        public string loaiTaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhanVien> NhanViens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuNhapHang> PhieuNhapHangs { get; set; }
    }
}
