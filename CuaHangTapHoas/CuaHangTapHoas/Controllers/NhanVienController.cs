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
    public class NhanVienController : Controller
    {
        CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();
        // GET: NhanVien
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContentNV(string searchString, int? page)
        {
            int pageSize = 10; // Số mục trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang 1 nếu không có tham số page

            var nhanVienQuery = (from nv in kn.NhanViens join tk in kn.TaiKhoans on nv.maTaiKhoan 
                                 equals tk.maTaiKhoan select new {nv, tk}).AsQueryable(); // Ensure to include TaiKhoan

            // Tìm kiếm nếu có chuỗi tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                nhanVienQuery = nhanVienQuery.Where(t => t.nv.tenNhanVien.Contains(searchString));
            }

            // Lấy danh sách nhân viên và chuyển đổi sang ThemNVViewModel
            var nhanVienList = nhanVienQuery
                .OrderByDescending(nv => nv.nv.maNhanVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(nv => new ThemNVViewModel
                {
                    maNhanVien = nv.nv.maNhanVien,
                    tenNhanVien = nv.nv.tenNhanVien,
                    soDienThoai = nv.nv.soDienThoai,
                    diaChi = nv.nv.diaChi,
                    email = nv.nv.email,
                    chucVu = nv.nv.chucVu,
                    //luong = nv.nv.luong,
                    loaiTaiKhoan = nv.tk.loaiTaiKhoan // Retrieve loaiTaiKhoan from TaiKhoan
                })
                .ToList();

            // Tính toán tổng số trang
            int totalRecords = nhanVienQuery.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.PageNumber = pageNumber; // Lưu trữ số trang hiện tại để sử dụng cho phân trang
            ViewBag.SearchString = searchString; // Lưu chuỗi tìm kiếm để hiển thị lại trong form tìm kiếm

            return View(nhanVienList);
        }


        public ActionResult ThemNV()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNV(ThemNVViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tạo mới một tài khoản
                var taiKhoan = new TaiKhoan
                {
                    tenDangNhap = model.tenDangNhap,
                    matKhau = model.matKhau,
                    //loaiTaiKhoan = model.loaiTaiKhoan,
                    soDienThoai = model.soDienThoai,
                    email = model.email,
                    loaiTaiKhoan = "Nhân Viên",
                    ngayTao = DateTime.Now // Gán ngày tạo hiện tại
                };

                // Thêm tài khoản vào cơ sở dữ liệu
                kn.TaiKhoans.Add(taiKhoan);
                kn.SaveChanges(); // Lưu thay đổi để lấy được maTaiKhoan

                // Tạo mới một nhân viên và liên kết với tài khoản vừa tạo
                var nhanVien = new NhanVien
                {
                    tenNhanVien = model.tenNhanVien,
                    soDienThoai = model.soDienThoai,
                    diaChi = model.diaChi,
                    email = model.email,
                    chucVu = model.chucVu,
                    luong = model.luong,
                    maTaiKhoan = taiKhoan.maTaiKhoan // Gán maTaiKhoan vừa được tạo
                };

                // Thêm nhân viên vào cơ sở dữ liệu
                kn.NhanViens.Add(nhanVien);
                kn.SaveChanges(); // Lưu thay đổi

                return RedirectToAction("ContentNV");
            }
            return View(model);
        }


        public ActionResult EditNV(int id)
        {
            var nhanVien = kn.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            // Lấy tài khoản tương ứng với mã tài khoản của nhân viên
            var taiKhoan = kn.TaiKhoans.Find(nhanVien.maTaiKhoan);

            // Chuyển dữ liệu từ NhanVien và TaiKhoan sang ThemNVViewModel
            var model = new ThemNVViewModel
            {
                maNhanVien = nhanVien.maNhanVien,
                tenNhanVien = nhanVien.tenNhanVien,
                soDienThoai = nhanVien.soDienThoai,
                diaChi = nhanVien.diaChi,
                email = nhanVien.email,
                chucVu = nhanVien.chucVu,
                luong = nhanVien.luong,
                
                //// Thông tin tài khoản
                //tenDangNhap = taiKhoan?.tenDangNhap,
                //matKhau = taiKhoan?.matKhau,
                loaiTaiKhoan = taiKhoan?.loaiTaiKhoan
            };
            ViewBag.ChucVuOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "user", Text = "Nhân Viên" },
                new SelectListItem { Value = "admin", Text = "Quản Lý" },
            };
            // Trả về view với model ThemNVViewModel
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNV(ThemNVViewModel model)
        {
            // Kiểm tra tính hợp lệ của Model
            if (ModelState.IsValid)
            {
                // Tìm nhân viên theo maNhanVien
                var nhanVien = kn.NhanViens.Find(model.maNhanVien);
                if (nhanVien == null)
                {
                    return HttpNotFound();
                }

                // Tìm tài khoản tương ứng
                var taiKhoan = kn.TaiKhoans.Find(nhanVien.maTaiKhoan);
                if (taiKhoan == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật dữ liệu từ ThemNVViewModel vào đối tượng NhanVien
                nhanVien.tenNhanVien = model.tenNhanVien;
                nhanVien.soDienThoai = model.soDienThoai;
                nhanVien.diaChi = model.diaChi;
                nhanVien.email = model.email;
                nhanVien.chucVu = (model.loaiTaiKhoan.Equals("user") ? "Nhân Viên" : "Quản Lý");
                nhanVien.luong = model.luong;

                //// Cập nhật dữ liệu từ ThemNVViewModel vào đối tượng TaiKhoan
                //taiKhoan.tenDangNhap = model.tenDangNhap;
                //taiKhoan.matKhau = model.matKhau;
                taiKhoan.loaiTaiKhoan = model.loaiTaiKhoan;

                // Lưu thay đổi vào cơ sở dữ liệu
                kn.SaveChanges();
                return RedirectToAction("ContentNV", "NhanVien");
            }
            ViewBag.ChucVuOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "user", Text = "Nhân Viên" },
                new SelectListItem { Value = "admin", Text = "Quản Lý" },
            };

            // Nếu model không hợp lệ, trả lại view với dữ liệu hiện có
            return View(model);
        }
        // In Controller or ViewModel

        public ActionResult DeleteNV(int id)
        {
            // Tìm nhân viên theo id
            var nhanVien = kn.NhanViens.Find(id);

            // Kiểm tra nếu nhân viên không tồn tại
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            // Tìm tài khoản liên quan đến nhân viên (nếu có)
            var taiKhoan = kn.TaiKhoans.Find(nhanVien.maTaiKhoan);

            // Tạo đối tượng ThemNVViewModel từ dữ liệu NhanVien
            ThemNVViewModel nhanVienViewModel = new ThemNVViewModel
            {
                maNhanVien = nhanVien.maNhanVien,
                tenNhanVien = nhanVien.tenNhanVien,
                soDienThoai = nhanVien.soDienThoai,
                diaChi = nhanVien.diaChi,
                email = nhanVien.email,
                chucVu = nhanVien.chucVu,
                luong = nhanVien.luong,
                tenDangNhap = taiKhoan?.tenDangNhap,  // Lấy thông tin tài khoản nếu có
                loaiTaiKhoan = taiKhoan?.loaiTaiKhoan
            };

            // Trả về view với model là ThemNVViewModel
            return View(nhanVienViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int maNhanVien)
        {
            // Tìm nhân viên theo id
            var nhanVien = kn.NhanViens.Find(maNhanVien);

            // Kiểm tra nếu nhân viên không tồn tại
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            // Tìm tài khoản liên quan đến nhân viên
            var taiKhoan = kn.TaiKhoans.Find(nhanVien.maTaiKhoan);

            // Xóa nhân viên khỏi cơ sở dữ liệu
            kn.NhanViens.Remove(nhanVien);

            // Nếu có tài khoản liên quan, cũng xóa tài khoản đó
            if (taiKhoan != null)
            {
                kn.TaiKhoans.Remove(taiKhoan);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            kn.SaveChanges();

            // Chuyển hướng về trang Index
            return RedirectToAction("ContentNV", "NhanVien");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int maNhanVien)
        //{
        //    Console.WriteLine("Xóa nhân viên với ID: " + maNhanVien); // Log ID nhân viên
        //    var nhanVien = kn.NhanViens.Find(maNhanVien);

        //    if (nhanVien == null)
        //    {
        //        return Json(new { success = false, message = "Nhân viên không tồn tại." });
        //    }

        //    // Xóa nhân viên
        //    kn.NhanViens.Remove(nhanVien);
        //    kn.SaveChanges();

        //    return Json(new { success = true });
        //}


        public ActionResult DetailsNV(int id)
        {
            // Tìm nhân viên theo id
            var nhanVien = kn.NhanViens.Find(id);

            // Kiểm tra nếu nhân viên không tồn tại
            if (nhanVien == null)
            {
                return HttpNotFound("Nhân viên không tồn tại.");
            }

            var nhanVienViewModel = new ThemNVViewModel
            {
                maNhanVien = nhanVien.maNhanVien,
                tenNhanVien = nhanVien.tenNhanVien,
                soDienThoai = nhanVien.soDienThoai,
                diaChi = nhanVien.diaChi,
                email = nhanVien.email,
                chucVu = nhanVien.chucVu,
                luong = nhanVien.luong,
                loaiTaiKhoan = nhanVien.TaiKhoan?.loaiTaiKhoan
            };
            // Trả về view với model là ThemNVViewModel
            return View(nhanVienViewModel);
        }

        public ActionResult ExportToExcel()
        {
            // Lấy danh sách nhân viên từ cơ sở dữ liệu và chuyển đổi sang ThemNVViewModel
            List<ThemNVViewModel> nhanVienViewModels = kn.NhanViens
                .Select(nv => new ThemNVViewModel
                {
                    maNhanVien = nv.maNhanVien,
                    tenNhanVien = nv.tenNhanVien,
                    soDienThoai = nv.soDienThoai,
                    diaChi = nv.diaChi,
                    email = nv.email,
                    chucVu = nv.chucVu,
                    luong = nv.luong
                })
                .ToList();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Nhân Viên");

                // Tiêu đề
                worksheet.Cell(1, 1).Value = "Mã Nhân viên";
                worksheet.Cell(1, 2).Value = "Tên Nhân viên";
                worksheet.Cell(1, 3).Value = "Số điện thoại";
                worksheet.Cell(1, 4).Value = "Địa chỉ";
                worksheet.Cell(1, 5).Value = "Email";
                worksheet.Cell(1, 6).Value = "Chức vụ";
                worksheet.Cell(1, 7).Value = "Lương";

                // Duyệt qua danh sách tài khoản và thêm dữ liệu vào worksheet
                for (int i = 0; i < nhanVienViewModels.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = nhanVienViewModels[i].maNhanVien;
                    worksheet.Cell(i + 2, 2).Value = nhanVienViewModels[i].tenNhanVien;
                    worksheet.Cell(i + 2, 3).Value = nhanVienViewModels[i].soDienThoai;
                    worksheet.Cell(i + 2, 4).Value = nhanVienViewModels[i].diaChi;
                    worksheet.Cell(i + 2, 5).Value = nhanVienViewModels[i].email;
                    worksheet.Cell(i + 2, 6).Value = nhanVienViewModels[i].chucVu;
                    worksheet.Cell(i + 2, 7).Value = nhanVienViewModels[i].luong;
                }

                // Lưu file Excel
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = "NhanVien.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }



    }
}