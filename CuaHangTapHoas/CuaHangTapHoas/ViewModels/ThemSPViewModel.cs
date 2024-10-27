using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CuaHangTapHoas.ViewModels
{
    public class ThemSPViewModel
    {
        public int maSanPham { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống tên sản phẩm")]
        [DisplayName("Tên sản phẩm")]
        public string tenSanPham { get; set; }
        public int maLoaiSanPham { get; set; }
        
        [DisplayName("Tên loại sản phẩm")]
        public string tenLoaiSanPham { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống đơn giá")]
        [DisplayName("Đơn giá")]
        public decimal giaBan { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống số lượng")]
        [DisplayName("Số lượng")]
        public int soLuong { get; set; }
        [DisplayName("Ngày lập")]
        [Column(TypeName = "date")]
        public DateTime? ngayNhap { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống miêu tả")]
        [DisplayName("Mô tả")]
        [Column(TypeName = "text")]
        public string moTa { get; set; }
        [DisplayName("Hình sản phẩm")]
        public string hinhAnh { get; set; }
        [DisplayName("Upload hình sản phẩm")]
        public HttpPostedFileBase hinhAnhFile { get; set; }
        public List<LoaiSPViewModels> SelectedCategories { get; set; }
    }
  
}