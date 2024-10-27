using ClosedXML.Excel;
using CuaHangTapHoas.Models;
using CuaHangTapHoas.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace CuaHangTapHoas.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }
        private CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();

        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm người dùng có tên đăng nhập và mật khẩu trùng khớp
                var user = (from t in kn.TaiKhoans join nv in kn.NhanViens on t.maTaiKhoan equals nv.maTaiKhoan
                            where t.tenDangNhap == model.TenDangNhap && t.matKhau == model.MatKhau select new {t, nv}).FirstOrDefault();

                if (user != null)
                {
                    // Lưu thông tin người dùng vào Session khi đăng nhập thành công
                    Session["TenDangNhap"] = user.t.tenDangNhap;
                    Session["LoaiTaiKhoan"] = user.t.loaiTaiKhoan;
                    Session["Name"] = user.nv.tenNhanVien;
                    return RedirectToAction("Index", "TrangChu");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult ContentTK(string searchString, int? page)
        {
            int pageSize = 10; // Number of items per page
            int pageNumber = (page ?? 1); // Current page, defaults to 1

            // Get all accounts as a queryable
            var accountQuery = kn.TaiKhoans.AsQueryable();

            // Search if there's a search string
            if (!string.IsNullOrEmpty(searchString))
            {
                accountQuery = accountQuery.Where(t => t.tenDangNhap.Contains(searchString));
            }

            // Create a list of ThemTKViewModel
            var model = accountQuery
                .OrderByDescending(ac => ac.maTaiKhoan)
                .Select(ac => new ThemTKViewModel
                {
                    maTaiKhoan = ac.maTaiKhoan,
                    tenDangNhap = ac.tenDangNhap,
                    matKhau = ac.matKhau,
                    email = ac.email,
                    soDienThoai = ac.soDienThoai,
                    ngayTao = ac.ngayTao,
                    loaiTaiKhoan = ac.loaiTaiKhoan
                })
                .ToList();

            // Calculate total pages
            ViewBag.TotalPages = (int)Math.Ceiling((double)model.Count / pageSize);

            // Get the items to display on the current page
            model = model.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Store current page and search string in ViewBag
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchString = searchString;

            return View(model); // Return the list of ThemTKViewModel
        }


        // GET: TaiKhoan/Edit/5
        public ActionResult EditTK(int id)
        {
            var account = kn.TaiKhoans.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            var model = new ThemTKViewModel
            {
                maTaiKhoan = account.maTaiKhoan,
                tenDangNhap = account.tenDangNhap,
                matKhau = account.matKhau,
                email = account.email,
                soDienThoai = account.soDienThoai,
                ngayTao = account.ngayTao,
                loaiTaiKhoan = account.loaiTaiKhoan
            };
            model.LoaiTaiKhoanOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "user", Text = "Nhân Viên" },
                new SelectListItem { Value = "admin", Text = "Quản Lý" },
            };
            return View(model);
        }

        // POST: TaiKhoan/Edit/5
        [HttpPost]
        public ActionResult EditTK(ThemTKViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = kn.TaiKhoans.Find(model.maTaiKhoan);
                if (account == null)
                {
                    return HttpNotFound();
                }

                account.tenDangNhap = model.tenDangNhap;
                account.matKhau = model.matKhau; // You may want to hash this in a real application
                account.email = model.email;
                account.soDienThoai = model.soDienThoai;
                account.loaiTaiKhoan = model.loaiTaiKhoan;

                kn.SaveChanges();
                return RedirectToAction("ContentTK","TaiKhoan");
            }
            model.LoaiTaiKhoanOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "user", Text = "Nhân Viên" },
                new SelectListItem { Value = "admin", Text = "Quản Lý" },
            };
            return View(model);
        }

        // GET: TaiKhoan/Delete/5
        public ActionResult DeleteTK(int id)
        {
            // Tìm tài khoản theo id
            var taiKhoan = kn.TaiKhoans.Find(id);

            // Kiểm tra nếu tài khoản không tồn tại
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            // Tạo đối tượng ThemTKViewModel từ dữ liệu TaiKhoan
            var taiKhoanViewModel = new ThemTKViewModel
            {
                maTaiKhoan = taiKhoan.maTaiKhoan,
                tenDangNhap = taiKhoan.tenDangNhap,
                email = taiKhoan.email,
                soDienThoai = taiKhoan.soDienThoai,
                ngayTao = taiKhoan.ngayTao,
                loaiTaiKhoan = taiKhoan.loaiTaiKhoan
            };

            // Trả về view với model là ThemTKViewModel
            return View(taiKhoanViewModel);
        }
        // POST: TaiKhoan/Delete/5
        [HttpPost, ActionName("DeleteTK")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Tìm tài khoản theo id
            var taiKhoan = kn.TaiKhoans.Find(id);

            // Kiểm tra nếu tài khoản không tồn tại
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            // Xóa tài khoản khỏi cơ sở dữ liệu
            kn.TaiKhoans.Remove(taiKhoan);

            // Lưu thay đổi vào cơ sở dữ liệu
            kn.SaveChanges();

            // Chuyển hướng về trang Index
            return RedirectToAction("ContentTK", "TaiKhoan");
        }


        // GET: TaiKhoan/ChiTietTK/5
        public ActionResult DetailsTK(int id)
        {
            // Find the TaiKhoan by id
            TaiKhoan taiKhoan = kn.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }

            // Map TaiKhoan to ThemTKViewModel
            var taiKhoanViewModel = new ThemTKViewModel
            {
                maTaiKhoan = taiKhoan.maTaiKhoan,
                tenDangNhap = taiKhoan.tenDangNhap,
                matKhau = taiKhoan.matKhau,
                email = taiKhoan.email,
                soDienThoai = taiKhoan.soDienThoai,
                ngayTao = taiKhoan.ngayTao,
                loaiTaiKhoan = taiKhoan.loaiTaiKhoan
            };

            // Return the view with the ThemTKViewModel
            return View(taiKhoanViewModel);
        }


        public ActionResult ExportToExcel()
        {
            // Giả sử bạn đã có danh sách tài khoản trong Model
            List<TaiKhoan> taiKhoans = kn.TaiKhoans.ToList(); // Thay thế bằng phương thức lấy dữ liệu của bạn

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Tài Khoản");

                // Tiêu đề
                worksheet.Cell(1, 1).Value = "Mã Tài Khoản";
                worksheet.Cell(1, 2).Value = "Tên Đăng Nhập";
                worksheet.Cell(1, 3).Value = "Số điện thoại";
                worksheet.Cell(1, 4).Value = "Mật Khẩu";
                worksheet.Cell(1, 5).Value = "Loại Tài Khoản";

                // Duyệt qua danh sách tài khoản và thêm dữ liệu vào worksheet
                for (int i = 0; i < taiKhoans.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = taiKhoans[i].maTaiKhoan;
                    worksheet.Cell(i + 2, 2).Value = taiKhoans[i].tenDangNhap;
                    worksheet.Cell(i + 2, 3).Value = taiKhoans[i].soDienThoai;
                    worksheet.Cell(i + 2, 4).Value = taiKhoans[i].matKhau;
                    worksheet.Cell(i + 2, 5).Value = taiKhoans[i].loaiTaiKhoan;
                }

                // Lưu file Excel
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = "TaiKhoan.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}