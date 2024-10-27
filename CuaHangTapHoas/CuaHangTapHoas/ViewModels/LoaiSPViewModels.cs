using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuaHangTapHoas.ViewModels
{
    public class LoaiSPViewModels
    {
        [DisplayName("Mã loại")]
        public int maLoaiSanPham { get; set; }
        [DisplayName("Tên loại")]
        public string tenLoaiSanPham { get; set; }
        public bool IsSelected { get; internal set; }
    }
}