using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangTapHoas.ViewModels
{
    public class ThemTKViewModel
    {
        [Key]
        public int maTaiKhoan { get; set; }

        [DisplayName("Tên tài khoản")]
        [StringLength(50)]
        [Required(ErrorMessage = "Tên tài khoản không được bỏ trống")]
        public string tenDangNhap { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [StringLength(255)]
        public string matKhau { get; set; }

        [DisplayName("Email")]
        [StringLength(100)]
        //[EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string email { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(20)]
        public string soDienThoai { get; set; }

        [DisplayName("Ngày tạo")]
        [Column(TypeName = "date")]
        public DateTime? ngayTao { get; set; }

        [DisplayName("Loại tài khoản")]
        [Required(ErrorMessage = "Loại tài khoản không được bỏ trống")]
        public string loaiTaiKhoan { get; set; }
        public List<SelectListItem> LoaiTaiKhoanOptions { get; set; }
    }
}