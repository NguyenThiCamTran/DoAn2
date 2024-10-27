namespace CuaHangTapHoas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuNhap")]
    public partial class ChiTietPhieuNhap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int maPhieuNhap { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int maSanPham { get; set; }

        public int soLuong { get; set; }

        public decimal giaNhap { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? thanhTien { get; set; }

        public virtual PhieuNhapHang PhieuNhapHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
