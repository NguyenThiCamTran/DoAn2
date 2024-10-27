using CuaHangTapHoas.Models;
using CuaHangTapHoas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangTapHoas.Controllers
{

    public class TrangChuController : Controller
    {
        CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();
        // GET: TrangChu
        
        private double getTotalIncome()
        {
            double res = (double)(from hd in kn.HoaDons
                                  join ct in kn.ChiTietHoaDons on hd.maHoaDon equals ct.maHoaDon
                                  join sp in kn.SanPhams on ct.maSanPham equals sp.maSanPham
                                  where hd.TrangThai.Trim() == "Đã thanh toán"
                                  select hd.tongTien).Sum();
            return res;
        }
        private double getTotalIncomeInMonth(int month)
        {
            double res = (double)(from hd in kn.HoaDons
                                  join ct in kn.ChiTietHoaDons on hd.maHoaDon equals ct.maHoaDon
                                  join sp in kn.SanPhams on ct.maSanPham equals sp.maSanPham
                                  where hd.TrangThai.Trim() == "Đã thanh toán" && hd.ngayLap.Month == month
                                  select hd.tongTien).Sum();
            return res;
        }
        private int getTotalProduct()
        {
            int res = kn.SanPhams.Count();
            return res;
        }

        private int getTotalProductInMonth(int month)
        {
            return kn.TaiKhoans.Where(ac => ac.ngayTao.Value.Month == month).Count();
        }
        private int getTotalOrder()
        {
            return kn.HoaDons.Count();
        }

        private int getTotalOrderInMonth(int month)
        {
            return kn.HoaDons.Where(o => o.ngayLap.Month == month).Count();
        }
        public ActionResult Index()
        {
            TrangChuViewModel viewModel = new TrangChuViewModel();

            viewModel.TotalIncome = getTotalIncome();
            viewModel.TotalIncomeInMonth = getTotalIncomeInMonth(DateTime.Now.Month);
            viewModel.TotalProduct = getTotalProduct();
            viewModel.TotalProductInMonth = getTotalProductInMonth(DateTime.Now.Month);
            viewModel.TotalOrder = getTotalOrder();
            viewModel.TotalOrderInMonth = getTotalOrderInMonth(DateTime.Now.Month);
            //var incomeChartBar = (from hd in kn.HoaDons
            //                      join ct in kn.ChiTietHoaDons on hd.maHoaDon equals ct.maHoaDon
            //                      join sp in kn.SanPhams on ct.maSanPham equals sp.maSanPham
            //                      where hd.TrangThai.Trim() == "Đã thanh toán"
            //                      group new { ct, sp } by hd.ngayLap.Year into g
            //                      select new
            //                      {
            //                          Year = g.Key,
            //                          TotalAmount = g.Sum(x => x.ct.giaBan )
            //                      }).ToList();
            
            return View(viewModel);
        }
    }
}