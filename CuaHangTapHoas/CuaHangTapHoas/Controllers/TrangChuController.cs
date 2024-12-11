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
            var res = from hd in kn.HoaDons
                      join ct in kn.ChiTietHoaDons on hd.maHoaDon equals ct.maHoaDon
                      join sp in kn.SanPhams on ct.maSanPham equals sp.maSanPham
                      where hd.TrangThai.Trim() == "Đã thanh toán" && hd.ngayLap.Month == month
                      select (double?)hd.tongTien;

            return res.Sum() ?? 0;
        }

        private int getTotalProduct()
        {
            int res = kn.SanPhams.Count();
            return res;
        }

        private int getTotalProductInMonth(int month)
        {
            return kn.SanPhams.Where(ac => ac.ngayNhap.Value.Month == month).Count();
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

            var incomeChartBar = (from hd in kn.HoaDons
                                  join ct in kn.ChiTietHoaDons on hd.maHoaDon equals ct.maHoaDon
                                  join sp in kn.SanPhams on ct.maSanPham equals sp.maSanPham
                                  where hd.TrangThai.Trim() == "Đã thanh toán"
                                  group new { ct, sp, hd } by hd.ngayLap.Year into g
                                  select new
                                  {
                                      Year = g.Key,
                                      TotalAmount = g.Sum(x => x.hd.tongTien)
                                  }).ToList();

            var loaiSanPhamData = (from lsp in kn.LoaiSanPhams
                                   join sp in kn.SanPhams on lsp.maLoaiSanPham equals sp.maLoaiSanPham
                                   group sp by lsp.tenLoaiSanPham into g
                                   select new
                                   {
                                       TenLoaiSanPham = g.Key,
                                       TongSoLuong = g.Sum(sp => sp.soLuong) // Assuming 'soLuong' is a quantity property of SanPham
                                   }).ToList();

            // Initialize Charts with two ChartViewModel objects
            viewModel.Charts = new List<ChartViewModel>
    {
        new ChartViewModel { Data = new List<double>(), Labels = new List<string>() },
        new ChartViewModel { Data = new List<double>(), Labels = new List<string>() }
    };

            // Assign income chart data
            foreach (var item in incomeChartBar)
            {
                viewModel.Charts[0].Labels.Add(item.Year.ToString());
                viewModel.Charts[0].Data.Add((double)item.TotalAmount);
            }

            // Assign loaiSanPhamData chart data
            foreach (var item in loaiSanPhamData)
            {
                viewModel.Charts[1].Labels.Add(item.TenLoaiSanPham.ToString());
                viewModel.Charts[1].Data.Add((double)item.TongSoLuong);
            }

            return View(viewModel);
        }
    }
}