using ClosedXML.Excel;
using CuaHangTapHoas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangTapHoas.Controllers
{
    public class LoaiSPController : Controller
    {
        CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();
        // GET: LoaiSP
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContentLoaiSP(string searchString, int? page)
        {           
            int pageSize = 10; // Số mục trên mỗi trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang 1 nếu không có tham số page

            var topics = kn.LoaiSanPhams.AsQueryable();

            // Tìm kiếm nếu có chuỗi tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                topics = topics.Where(t => t.tenLoaiSanPham.Contains(searchString));
            }

            var model = topics.OrderByDescending(tp => tp.maLoaiSanPham).ToList();

            // Lấy số lượng tổng cộng của các mục và tính toán số trang
            ViewBag.TotalPages = (int)Math.Ceiling((double)model.Count / pageSize);

            // Lấy chỉ mục của các mục hiển thị trên trang hiện tại
            model = model.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageNumber = pageNumber; // Lưu trữ số trang hiện tại để sử dụng cho phân trang

            ViewBag.SearchString = searchString; // Lưu chuỗi tìm kiếm để hiển thị lại trong form tìm kiếm

            return View(model);
        }
        // GET: LoaiSP/Create
        public ActionResult ThemLoaiSP()
        {
            return View();
        }

        // POST: LoaiSP/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemLoaiSP(LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                kn.LoaiSanPhams.Add(loaiSanPham);
                kn.SaveChanges();
                return RedirectToAction("ContentLoaiSP");
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSP/Edit/5
        public ActionResult EditLoaiSP(int id)
        {
            var loaiSanPham = kn.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: LoaiSP/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLoaiSP(LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                kn.Entry(loaiSanPham).State = EntityState.Modified;
                kn.SaveChanges();
                return RedirectToAction("ContentLoaiSP", "LoaiSP");
            }
            return View(loaiSanPham);
        }

        // GET: LoaiSP/Delete/5
        public ActionResult DeleteLoaiSP(int id)
        {
            //var loaiSanPham = kn.LoaiSanPhams.Find(id);
            //if (loaiSanPham == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(loaiSanPham);
            LoaiSanPham cs = new LoaiSanPham();
            cs = kn.LoaiSanPhams.SingleOrDefault(x => x.maLoaiSanPham == id);
            return View(cs);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int maLoaiSanPham)
        {
            var loaiSanPham = kn.LoaiSanPhams.Find(maLoaiSanPham);
            if (loaiSanPham != null)
            {
                kn.LoaiSanPhams.Remove(loaiSanPham);
                kn.SaveChanges();
            }
            return RedirectToAction("ContentLoaiSP", "LoaiSP");
        }



        // POST: LoaiSP/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int maLoaiSanPham)
        //{
        //    var loaiSanPham = kn.LoaiSanPhams.Find(maLoaiSanPham);
        //    if (loaiSanPham != null)
        //    {
        //        kn.LoaiSanPhams.Remove(loaiSanPham);
        //        kn.SaveChanges();
        //    }
        //    return RedirectToAction("ContentLoaiSP", "LoaiSP");
        //}



        // GET: LoaiSP/Details/5
        public ActionResult DetailsLoaiSP(int id)
        {
            var loaiSanPham = kn.LoaiSanPhams.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        public ActionResult ExportToExcel()
        {
            // Giả sử bạn đã có danh sách tài khoản trong Model
            List<LoaiSanPham> loaiSanPhams = kn.LoaiSanPhams.ToList(); 

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Loại sản phẩm");

                // Tiêu đề
                worksheet.Cell(1, 1).Value = "Mã Loại Sản Phẩm";
                worksheet.Cell(1, 2).Value = "Tên Loại Sản Phẩm";               

                // Duyệt qua danh sách tài khoản và thêm dữ liệu vào worksheet
                for (int i = 0; i < loaiSanPhams.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = loaiSanPhams[i].maLoaiSanPham;
                    worksheet.Cell(i + 2, 2).Value = loaiSanPhams[i].tenLoaiSanPham;
                }

                // Lưu file Excel
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = "LoaiSanPham.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}