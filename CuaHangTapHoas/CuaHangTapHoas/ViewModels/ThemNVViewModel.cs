using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangTapHoas.ViewModels
{
    public class ThemNVViewModel
    {
        [DisplayName("Mã nhân viên")]
        public int maNhanVien { get; set; }
        [DisplayName("Tên Nhân Viên")]
        [Required]
        [StringLength(255)]
        public string tenNhanVien { get; set; }
        [DisplayName("Tên tài khoản")]
        public string tenDangNhap { get; set; }
        [DisplayName("Mật khẩu")]
        public string matKhau { get; set; }
        [DisplayName("Số điện thoại")]
        public string soDienThoai { get; set; }
        [DisplayName("Địa chỉ")]
        public string diaChi { get; set; }
        [DisplayName("Email")]
        public string email { get; set; }
        [DisplayName("Chức vụ")]
        public string chucVu { get; set; }
        [DisplayName("Lương")]
        public decimal? luong { get; set; }
        [DisplayName("Ngày tạo")]
        [Column(TypeName = "date")]
        public DateTime? ngayTao { get; set; }     
        [DisplayName("Loại tài khoản")]
        [StringLength(50)]
        public string loaiTaiKhoan { get; set; }
        public List<LoaiSPViewModels> SelectedCategories { get; set; }
        public List<SelectListItem> ChucVuOptions { get; set; }
    }
}