using ClosedXML.Excel;
using CuaHangTapHoas.Models;
using CuaHangTapHoas.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangTapHoas.Controllers
{
    public class PhieuNhapHangController : Controller
    {
        CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();
        // GET: PhieuNhapHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContentPNH(string searchString, int? page)
        {
            int pageSize = 10;  // Number of items per page
            int pageNumber = (page ?? 1); // Current page

            // Query the PhieuNhapHang table, including related data (NhaCungCap, TaiKhoan, ChiTietPhieuNhap)
            var phieuNhapHangs = kn.PhieuNhapHangs
                .Include(pnh => pnh.NhaCungCap)
                .Include(pnh => pnh.TaiKhoan)
                .Include(pnh => pnh.ChiTietPhieuNhaps.Select(ct => ct.SanPham)) // Include SanPham in ChiTietPhieuNhap
                .AsQueryable();

            // Search by tenNhaCungCap if a search string is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                phieuNhapHangs = phieuNhapHangs.Where(pnh => pnh.NhaCungCap.tenNhaCungCap.Contains(searchString));
            }

            // Project the result into the ThemPNHViewModel
            var model = phieuNhapHangs
                .Select(pnh => new ThemPNHViewModel
                {
                    maPhieuNhap = pnh.maPhieuNhap,
                    maNhaCungCap = pnh.maNhaCungCap,
                    tenNhaCungCap = pnh.NhaCungCap.tenNhaCungCap,
                    ngayNhap = pnh.ngayNhap,
                    tongTien = pnh.tongTien,
                    maTaiKhoan = pnh.maTaiKhoan,
                    tenDangNhap = pnh.TaiKhoan.tenDangNhap,

                    // Map ChiTietPhieuNhap collection to the ViewModel
                    ChiTietPhieuNhaps = pnh.ChiTietPhieuNhaps.Select(ct => new ChiTietPhieuNhapViewModel
                    {
                        maSanPham = ct.maSanPham,
                        tenSanPham = ct.SanPham != null ? ct.SanPham.tenSanPham : "N/A", // Safe navigation in case SanPham is null
                        soLuong = ct.soLuong,
                        giaNhap = ct.giaNhap
                    }).ToList()
                })
                .OrderByDescending(pnh => pnh.maPhieuNhap) // Order by maPhieuNhap descending
                .ToList();

            // Calculate the total number of pages for pagination
            ViewBag.TotalPages = (int)Math.Ceiling((double)model.Count / pageSize);

            // Get items to display on the current page
            model = model.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Set current page and search string for use in the view
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchString = searchString;

            return View(model); // Pass the model to the view
        }


        public ActionResult ThemPNH()
        {
            ViewBag.NhaCungCapList = kn.NhaCungCaps.ToList();
            ViewBag.TaiKhoanList = kn.TaiKhoans.ToList();
            ViewBag.SanPhamList = kn.SanPhams.ToList();
            return View(new ThemPNHViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThemPNHViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tạo đối tượng PhieuNhapHang mới
                var phieuNhap = new PhieuNhapHang
                {
                    maNhaCungCap = model.maNhaCungCap,
                    ngayNhap = DateTime.Now,
                    tongTien = model.tongTien,
                    maTaiKhoan = model.maTaiKhoan
                };

                // Thêm chi tiết phiếu nhập
                foreach (var ct in model.ChiTietPhieuNhaps)
                {
                    var chiTiet = new ChiTietPhieuNhap
                    {
                        maSanPham = ct.maSanPham,
                        soLuong = ct.soLuong,
                        giaNhap = ct.giaNhap,
                        maPhieuNhap = phieuNhap.maPhieuNhap
                    };

                    // Tính thành tiền cho chi tiết
                    chiTiet.thanhTien = chiTiet.soLuong * chiTiet.giaNhap;

                    phieuNhap.ChiTietPhieuNhaps.Add(chiTiet);
                }

                // Lưu vào cơ sở dữ liệu
                kn.PhieuNhapHangs.Add(phieuNhap);
                kn.SaveChanges();

                return RedirectToAction("ContentPNH"); // Quay lại trang danh sách phiếu nhập hàng
            }

            // Nếu có lỗi, trả lại view cùng dữ liệu
            ViewBag.NhaCungCapList = kn.NhaCungCaps.ToList();
            ViewBag.TaiKhoanList = kn.TaiKhoans.ToList();
            ViewBag.SanPhamList = kn.SanPhams.ToList();
            return View(model);
        }

        //    // In case of invalid model, return with model data to show validation errors
        //    ViewBag.NhaCungCaps = new SelectList(kn.NhaCungCaps, "maNhaCungCap", "tenNhaCungCap", model.maNhaCungCap);
        //    ViewBag.SanPhams = kn.SanPhams.ToList(); // Get products again for the view

        //    return View(model);
        //}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Tìm phiếu nhập theo ID
            var phieuNhap = kn.PhieuNhapHangs.Include(p => p.ChiTietPhieuNhaps).FirstOrDefault(p => p.maPhieuNhap == id);

            if (phieuNhap == null)
            {
                return HttpNotFound();
            }

            // Chuẩn bị dữ liệu để hiển thị trong view
            var viewModel = new ThemPNHViewModel
            {
                maPhieuNhap = phieuNhap.maPhieuNhap,
                maNhaCungCap = phieuNhap.maNhaCungCap,
                ngayNhap = phieuNhap.ngayNhap,
                maTaiKhoan = phieuNhap.maTaiKhoan,
                tongTien = phieuNhap.tongTien,
                ChiTietPhieuNhaps = phieuNhap.ChiTietPhieuNhaps.Select(ct => new ChiTietPhieuNhapViewModel
                {
                    maSanPham = ct.maSanPham,
                    soLuong = ct.soLuong,
                    giaNhap = ct.giaNhap
                }).ToList()
            };

            // Truyền dữ liệu phụ (Danh sách sản phẩm, tài khoản, nhà cung cấp)
            ViewBag.NhaCungCapList = kn.NhaCungCaps.ToList();
            ViewBag.TaiKhoanList = kn.TaiKhoans.ToList();
            ViewBag.SanPhamList = kn.SanPhams.ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThemPNHViewModel model)
        {
            if (ModelState.IsValid)
            {
                var phieuNhap = kn.PhieuNhapHangs.Include(p => p.ChiTietPhieuNhaps).FirstOrDefault(p => p.maPhieuNhap == model.maPhieuNhap);

                if (phieuNhap == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin phiếu nhập
                phieuNhap.maNhaCungCap = model.maNhaCungCap;
                phieuNhap.ngayNhap = model.ngayNhap;
                phieuNhap.maTaiKhoan = model.maTaiKhoan;
                phieuNhap.tongTien = model.tongTien;

                // Cập nhật chi tiết phiếu nhập
                phieuNhap.ChiTietPhieuNhaps.Clear(); // Xóa chi tiết cũ
                foreach (var chiTiet in model.ChiTietPhieuNhaps)
                {
                    phieuNhap.ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap
                    {
                        maSanPham = chiTiet.maSanPham,
                        soLuong = chiTiet.soLuong,
                        giaNhap = chiTiet.giaNhap
                    });
                }

                // Lưu thay đổi
                kn.SaveChanges();
                return RedirectToAction("ContentPNH");
            }

            // Trường hợp ModelState không hợp lệ
            ViewBag.NhaCungCapList = kn.NhaCungCaps.ToList();
            ViewBag.TaiKhoanList = kn.TaiKhoans.ToList();
            ViewBag.SanPhamList = kn.SanPhams.ToList();

            return View(model);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            // Tìm phiếu nhập theo ID
            var phieuNhap = kn.PhieuNhapHangs.Include(p => p.ChiTietPhieuNhaps.Select(ct => ct.SanPham))
                                               .Include(p => p.NhaCungCap)
                                               .Include(p => p.TaiKhoan)
                                               .FirstOrDefault(p => p.maPhieuNhap == id);

            if (phieuNhap == null)
            {
                return HttpNotFound();
            }

            // Tạo ViewModel cho chi tiết phiếu nhập
            var viewModel = new ThemPNHViewModel
            {
                maPhieuNhap = phieuNhap.maPhieuNhap,
                tenNhaCungCap = phieuNhap.NhaCungCap.tenNhaCungCap,
                ngayNhap = phieuNhap.ngayNhap,
                tenDangNhap = phieuNhap.TaiKhoan.tenDangNhap,
                tongTien = phieuNhap.tongTien,
                ChiTietPhieuNhaps = phieuNhap.ChiTietPhieuNhaps.Select(ct => new ChiTietPhieuNhapViewModel
                {
                    tenSanPham = ct.SanPham.tenSanPham,
                    soLuong = ct.soLuong,
                    giaNhap = ct.giaNhap
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var phieuNhap = kn.PhieuNhapHangs.Find(id);
            if (phieuNhap == null)
            {
                return HttpNotFound();
            }

            // Xóa chi tiết phiếu nhập và phiếu nhập chính
            kn.ChiTietPhieuNhaps.RemoveRange(phieuNhap.ChiTietPhieuNhaps);
            kn.PhieuNhapHangs.Remove(phieuNhap);
            kn.SaveChanges();

            // Sau khi xóa, quay về trang danh sách
            return RedirectToAction("ContentPNH");
        }
        public ActionResult ExportToExcel()
        {
            // Giả sử bạn đã có danh sách phiếu nhập hàng trong Model
            var phieuNhapHangs = kn.PhieuNhapHangs.Include(p => p.NhaCungCap).Include(p => p.TaiKhoan).ToList();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Phiếu nhập hàng");

                // Tiêu đề
                worksheet.Cell(1, 1).Value = "Mã phiếu nhập";
                worksheet.Cell(1, 2).Value = "Tên nhà cung cấp";
                worksheet.Cell(1, 3).Value = "Ngày nhập";
                worksheet.Cell(1, 4).Value = "Tên đăng nhập";
                worksheet.Cell(1, 5).Value = "Tổng tiền";

                // Duyệt qua danh sách phiếu nhập hàng và thêm dữ liệu vào worksheet
                for (int i = 0; i < phieuNhapHangs.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = phieuNhapHangs[i].maPhieuNhap;
                    worksheet.Cell(i + 2, 2).Value = phieuNhapHangs[i].NhaCungCap.tenNhaCungCap; 
                    worksheet.Cell(i + 2, 3).Value = phieuNhapHangs[i].ngayNhap; 
                    worksheet.Cell(i + 2, 4).Value = phieuNhapHangs[i].TaiKhoan.tenDangNhap; 
                    worksheet.Cell(i + 2, 5).Value = phieuNhapHangs[i].tongTien; 
                }

                // Lưu file Excel
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = "PhieuNhapHang.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }



    }
}