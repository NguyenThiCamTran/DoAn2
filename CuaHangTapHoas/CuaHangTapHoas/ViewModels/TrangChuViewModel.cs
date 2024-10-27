using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangTapHoas.ViewModels
{
    public class TrangChuViewModel
    {
        public double TotalIncome { get; set; }
        public double TotalIncomeInMonth { get; set; }
        public int TotalProduct { get; set; }
        public int TotalProductInMonth { get; set; }
        public int TotalOrder { get; set; }
        public int TotalOrderInMonth { get; set; }
        public List<ChartViewModel> Charts { get; set; }
    }
}