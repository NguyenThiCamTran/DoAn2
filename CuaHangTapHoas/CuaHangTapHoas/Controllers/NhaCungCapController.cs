using ClosedXML.Excel;
using CuaHangTapHoas.Models;
using CuaHangTapHoas.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangTapHoas.Controllers
{
    public class NhaCungCapController : Controller
    {
        CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();
        // GET: NhaCungCap
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContentNCC(string searchString, int? page)
        {
            int pageSize = 10; 
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang 1 nếu không có tham số page

            var nhaCungCaps = kn.NhaCungCaps.AsQueryable();

            // Tìm kiếm nếu có chuỗi tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                nhaCungCaps = nhaCungCaps.Where(t => t.tenNhaCungCap.Contains(searchString));
            }

            var nhaCungCapList = nhaCungCaps.OrderByDescending(ac => ac.maNhaCungCap).ToList();

            // Chuyển đổi dữ liệu từ NhaCungCap sang NhaCungCapViewModel
            var model = nhaCungCapList.Select(ncc => new NhaCungCapViewModel
            {
                maNhaCungCap = ncc.maNhaCungCap,
                tenNhaCungCap = ncc.tenNhaCungCap,
                soDienThoai = ncc.soDienThoai,
                diaChi = ncc.diaChi,
                email = ncc.email
            }).ToList();

            // Lấy số lượng tổng cộng của các mục và tính toán số trang
            ViewBag.TotalPages = (int)Math.Ceiling((double)nhaCungCapList.Count / pageSize);

            // Lấy chỉ mục của các mục hiển thị trên trang hiện tại
            model = model.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageNumber = pageNumber; // Lưu trữ số trang hiện tại để sử dụng cho phân trang

            ViewBag.SearchString = searchString; // Lưu chuỗi tìm kiếm để hiển thị lại trong form tìm kiếm

            return View(model);
        }

        // Hiển thị form thêm nhà cung cấp
        public ActionResult ThemNCC()
        {
            return View();
        }

        // Xử lý dữ liệu từ form thêm nhà cung cấp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNCC(NhaCungCapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nhaCungCap = new NhaCungCap
                {
                    maNhaCungCap = model.maNhaCungCap,
                    tenNhaCungCap = model.tenNhaCungCap,
                    soDienThoai = model.soDienThoai,
                    diaChi = model.diaChi,
                    email = model.email
                };

                kn.NhaCungCaps.Add(nhaCungCap);
                kn.SaveChanges();
                return RedirectToAction("ContentNCC", "NhaCungCap"); 
            }

            return View(model); // Nếu có lỗi, trả lại view với thông tin lỗi
        }
        public ActionResult SuaNCC(int id)
        {
            var nhaCungCap = kn.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }

            var model = new NhaCungCapViewModel
            {
                maNhaCungCap = nhaCungCap.maNhaCungCap,
                tenNhaCungCap = nhaCungCap.tenNhaCungCap,
                soDienThoai = nhaCungCap.soDienThoai,
                diaChi = nhaCungCap.diaChi,
                email = nhaCungCap.email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaNCC(int id, NhaCungCapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nhaCungCap = kn.NhaCungCaps.Find(id);
                if (nhaCungCap == null)
                {
                    return HttpNotFound();
                }
                nhaCungCap.maNhaCungCap = model.maNhaCungCap;
                nhaCungCap.tenNhaCungCap = model.tenNhaCungCap;
                nhaCungCap.soDienThoai = model.soDienThoai;
                nhaCungCap.diaChi = model.diaChi;
                nhaCungCap.email = model.email;

                kn.SaveChanges();
                return RedirectToAction("ContentNCC", "NhaCungCap");
            }

            return View(model);
        }
        public ActionResult XoaNCC(int id)
        {
            var nhaCungCap = kn.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }

            // Tạo ViewModel từ entity NhaCungCap
            var nhaCungCapViewModel = new NhaCungCapViewModel
            {
                maNhaCungCap = nhaCungCap.maNhaCungCap,
                tenNhaCungCap = nhaCungCap.tenNhaCungCap,
                soDienThoai = nhaCungCap.soDienThoai,
                diaChi = nhaCungCap.diaChi,
                email = nhaCungCap.email
            };

            return View(nhaCungCapViewModel);
        }

        [HttpPost, ActionName("XoaNCC")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaNCCConfirmed(int id)
        {
            var nhaCungCap = kn.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }

            kn.NhaCungCaps.Remove(nhaCungCap);
            kn.SaveChanges();

            return RedirectToAction("ContentNCC", "NhaCungCap");
        }

        public ActionResult ChiTietNCC(int id)
        {
            var nhaCungCap = kn.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
            {
                return HttpNotFound();
            }

            // Chuyển dữ liệu từ NhaCungCap sang NhaCungCapViewModel
            var model = new NhaCungCapViewModel
            {
                maNhaCungCap = nhaCungCap.maNhaCungCap,
                tenNhaCungCap = nhaCungCap.tenNhaCungCap,
                soDienThoai = nhaCungCap.soDienThoai,
                diaChi = nhaCungCap.diaChi,
                email = nhaCungCap.email
            };

            return View(model);
        }
        public ActionResult ExportToExcel()
        {
            // Lấy danh sách nhân viên từ cơ sở dữ liệu và chuyển đổi sang ThemNVViewModel
            List<NhaCungCapViewModel> nhaCungCapViewModels = kn.NhaCungCaps
                .Select(ncc => new NhaCungCapViewModel
                {
                    maNhaCungCap = ncc.maNhaCungCap,
                    tenNhaCungCap = ncc.tenNhaCungCap,
                    soDienThoai = ncc.soDienThoai,
                    diaChi = ncc.diaChi,
                    email = ncc.email,
                })
                .ToList();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Nhà Cung Cấp");

                // Tiêu đề
                worksheet.Cell(1, 2).Value = "Tên nhà cung cấp";
                worksheet.Cell(1, 3).Value = "Số điện thoại";
                worksheet.Cell(1, 4).Value = "Địa chỉ";
                worksheet.Cell(1, 5).Value = "Email";

                // Duyệt qua danh sách tài khoản và thêm dữ liệu vào worksheet
                for (int i = 0; i < nhaCungCapViewModels.Count; i++)
                {
                    worksheet.Cell(i + 2, 2).Value = nhaCungCapViewModels[i].tenNhaCungCap;
                    worksheet.Cell(i + 2, 3).Value = nhaCungCapViewModels[i].soDienThoai;
                    worksheet.Cell(i + 2, 4).Value = nhaCungCapViewModels[i].diaChi;
                    worksheet.Cell(i + 2, 5).Value = nhaCungCapViewModels[i].email;
                }

                // Lưu file Excel
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = "NhaCungCap.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}