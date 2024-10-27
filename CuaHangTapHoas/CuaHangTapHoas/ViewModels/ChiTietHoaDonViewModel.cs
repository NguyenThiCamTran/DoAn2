using System.Collections.Generic;

namespace CuaHangTapHoas.ViewModels
{
    public class ChiTietHoaDonViewModel
    {
        public HoaDonViewModel HoaDon { get; set; }
        public List<HoaDonViewModel> ChiTietHoaDons { get; set; }
    }
}