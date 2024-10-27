using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CuaHangTapHoas.ViewModels
{
    public class HoaDonViewModel
    {
        public int maHoaDon {  get; set; }
        public DateTime NgayLap { get; set; }
        public string tenDangNhap { get; set; }
        public int? MaTaiKhoan { get; set; }
        public decimal TongTien {  get; set; }
        public string TrangThai { get; set; }
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
       
    }
}