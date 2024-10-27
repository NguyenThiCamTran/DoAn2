using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CuaHangTapHoas.ViewModels
{
    public class ThemPNHViewModel
    {
        public int maPhieuNhap { get; set; }
        public int? maNhaCungCap { get; set; }
        public string tenNhaCungCap { get; set; }
        public DateTime ngayNhap { get; set; }
        public decimal tongTien { get; set; }
        public int? maTaiKhoan { get; set; }
        public string tenDangNhap { get; set; }
        public List<ChiTietPhieuNhapViewModel> ChiTietPhieuNhaps { get; set; }
    }
}