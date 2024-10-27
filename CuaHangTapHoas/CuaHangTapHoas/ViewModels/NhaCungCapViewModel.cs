using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuaHangTapHoas.ViewModels
{
    public class NhaCungCapViewModel
    {
        [DisplayName("Mã nhà cung cấp")]
        public int maNhaCungCap { get; set; }
        [DisplayName("Tên nhà cung cấp")]
        public string tenNhaCungCap { get; set; }
        [DisplayName("Số điện thoại")]
        public string soDienThoai { get; set; }

        [DisplayName("Địa chỉ")]
        public string diaChi { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }
    }
}