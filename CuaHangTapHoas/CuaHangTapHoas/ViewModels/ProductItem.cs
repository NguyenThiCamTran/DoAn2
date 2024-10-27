using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangTapHoas.ViewModels
{
    public class ProductItem
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => GiaBan * SoLuong;
    }
}