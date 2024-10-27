using ClosedXML.Excel;
using CuaHangTapHoas.Models;
using CuaHangTapHoas.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static CuaHangTapHoas.ViewModels.ThemSPViewModel;

namespace CuaHangTapHoas.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();

        // GET: SanPham
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContentSP(string searchString, int? page)
        {
            int pageSize = 10; // Số mục hiển thị trên mỗi trang
            int pageNumber = (page ?? 1); // Mặc định là trang đầu tiên nếu không có số trang được chỉ định

            var sanPhams = (from sp in kn.SanPhams
                            join lsp in kn.LoaiSanPhams on sp.maLoaiSanPham equals lsp.maLoaiSanPham
                            select new ThemSPViewModel
                            {
                                maSanPham = sp.maSanPham,
                                tenSanPham = sp.tenSanPham,
                                maLoaiSanPham = lsp.maLoaiSanPham, 
                                tenLoaiSanPham = lsp.tenLoaiSanPham,
                                giaBan = sp.giaBan,
                                soLuong = sp.soLuong,
                                ngayNhap = sp.ngayNhap,
                                moTa = sp.moTa,
                                hinhAnh = sp.hinhAnh
                            });

            if (!string.IsNullOrEmpty(searchString))
            {
                sanPhams = sanPhams.Where(sp => sp.tenSanPham.Contains(searchString));
            }

            sanPhams = sanPhams.OrderByDescending(sp => sp.maSanPham);
            var paginatedSanPhams = sanPhams.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)sanPhams.Count() / pageSize);
            ViewBag.SearchString = searchString;

            return View(paginatedSanPhams);
        }


        private List<LoaiSPViewModels> getListLoaiSPViewModel()
        {
            List<LoaiSanPham> list = kn.LoaiSanPhams.ToList();
            List<LoaiSPViewModels> listViewModel = new List<LoaiSPViewModels>();
            foreach (var item in list)
            {
                listViewModel.Add(new LoaiSPViewModels { maLoaiSanPham = item.maLoaiSanPham, tenLoaiSanPham = item.tenLoaiSanPham });
            }
            return listViewModel;
        }

        public ActionResult ThemSP()
        {
            var model = new ThemSPViewModel();
            model.SelectedCategories = getListLoaiSPViewModel(); // Load danh sách loại sản phẩm vào ViewModel
            return View(model);
        }

        [HttpPost]
        public ActionResult ThemSP(ThemSPViewModel model)
        {
            if (ModelState.IsValid)
            {
                SanPham sp = new SanPham
                {
                    tenSanPham = model.tenSanPham,
                    giaBan = model.giaBan,
                    soLuong = model.soLuong,
                    ngayNhap = DateTime.Now,
                    moTa = model.moTa,
                    maLoaiSanPham = model.maLoaiSanPham
                };

                // Handle image upload or conversion from URL
                if (model.hinhAnhFile != null && model.hinhAnhFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(model.hinhAnhFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Assets/Images"), fileName);
                    model.hinhAnhFile.SaveAs(path);
                    sp.hinhAnh = fileName;
                }
                else if (!string.IsNullOrEmpty(model.hinhAnh))
                {
                    try
                    {
                        var httpPostedFile = ConvertImageUrlToHttpPostedFileBase(model.hinhAnh);
                        string fileName = Path.GetFileName(model.hinhAnh);
                        string path = Path.Combine(Server.MapPath("~/Assets/Images"), fileName);
                        httpPostedFile.SaveAs(path);
                        sp.hinhAnh = fileName;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error converting image URL: {ex.Message}");
                        ModelState.AddModelError("", "Invalid image URL.");
                    }
                }

                try
                {
                    kn.SanPhams.Add(sp);
                    kn.SaveChanges();
                    return RedirectToAction("ContentSP");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving changes: {ex.Message}");
                    ModelState.AddModelError("", "Unable to save changes. Try again.");
                }
            }

            model.SelectedCategories = getListLoaiSPViewModel();
            return View(model);
        }


        public HttpPostedFileBase ConvertImageUrlToHttpPostedFileBase(string imageUrl)
        {
            // Combine the server path to the image URL
            string fullPath = Path.Combine(Server.MapPath("~/Assets/Images"), imageUrl);

            // Check if the file exists
            if (!System.IO.File.Exists(fullPath))
            {
                throw new FileNotFoundException("Image file not found.", fullPath);
            }

            // Read the image file into a byte array
            byte[] imageBytes = System.IO.File.ReadAllBytes(fullPath);

            // Create a memory stream from the byte array
            MemoryStream stream = new MemoryStream(imageBytes);

            // Determine the content type (you can adjust based on your needs)
            string contentType = MimeMapping.GetMimeMapping(imageUrl); // Automatically get the MIME type based on file extension
            string fileName = Path.GetFileName(imageUrl);

            // Return a new CustomHttpPostedFileBase instance
            return new CustomHttpPostedFileBase(stream, contentType, fileName);
        }

        public ActionResult EditSP(int id)
        {
            var sanPham = kn.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            var model = new ThemSPViewModel
            {
                maSanPham = sanPham.maSanPham,
                tenSanPham = sanPham.tenSanPham,
                giaBan = sanPham.giaBan,
                soLuong = sanPham.soLuong,
                moTa = sanPham.moTa,
                maLoaiSanPham = (int)sanPham.maLoaiSanPham,
                tenLoaiSanPham = sanPham.LoaiSanPham.tenLoaiSanPham,
                ngayNhap = DateTime.Now,
                hinhAnh = sanPham.hinhAnh, // Keep existing image file name
                SelectedCategories = getListLoaiSPViewModel()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditSP(int id, ThemSPViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sanPham = kn.SanPhams.Find(id);
                if (sanPham == null)
                {
                    return HttpNotFound();
                }

                sanPham.tenSanPham = model.tenSanPham;
                sanPham.giaBan = model.giaBan;
                sanPham.soLuong = model.soLuong;
                sanPham.moTa = model.moTa;
                sanPham.maLoaiSanPham = model.maLoaiSanPham;

                // Check if a new image file is uploaded
                if (model.hinhAnhFile != null && model.hinhAnhFile.ContentLength > 0)
                {
                    // Save the new image and update the product's image path
                    sanPham.hinhAnh = SaveImage(model.hinhAnhFile);
                }

                try
                {
                    kn.SaveChanges(); // Attempt to save changes to the database
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error saving changes: {ex.Message}");
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

                return RedirectToAction("ContentSP", "SanPham");
            }

            model.SelectedCategories = getListLoaiSPViewModel();
            return View(model);
        }


        // Method to save the uploaded image
        private string SaveImage(HttpPostedFileBase file)
        {
            // Get the file name and create a path to save it
            string fileName = Path.GetFileName(file.FileName);
            string path = Path.Combine(Server.MapPath("~/Assets/Images"), fileName);

            // Save the file to the server
            file.SaveAs(path);

            // Return the file name or path to store in the database
            return fileName; // You may also return the full path if needed
        }

        public ActionResult DeleteSP(int id)
        {
            var sanPham = kn.SanPhams.Include(s => s.LoaiSanPham).FirstOrDefault(s => s.maSanPham == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            // Create an instance of the view model and populate it with data from sanPham
            var viewModel = new ThemSPViewModel
            {
                maSanPham = sanPham.maSanPham,
                tenSanPham = sanPham.tenSanPham,
                maLoaiSanPham = (int)sanPham.maLoaiSanPham,
                tenLoaiSanPham = sanPham.LoaiSanPham?.tenLoaiSanPham, // Accessing the category name safely
                giaBan = sanPham.giaBan,
                soLuong = sanPham.soLuong,
                ngayNhap = sanPham.ngayNhap,
                moTa = sanPham.moTa,
                hinhAnh = sanPham.hinhAnh,
                // hinhAnhFile should not be populated here as it's not needed for deletion
                SelectedCategories = new List<LoaiSPViewModels>() // Populate this if needed
            };

            return View(viewModel);
        }

        //[HttpPost, ActionName("DeleteSP")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    var sanPham = kn.SanPhams.Find(id);
        //    if (sanPham == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    kn.SanPhams.Remove(sanPham);
        //    kn.SaveChanges();
        //    return RedirectToAction("ContentSP", "SanPham");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sanPham = kn.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            kn.SanPhams.Remove(sanPham);
            kn.SaveChanges();
            return RedirectToAction("ContentSP");
        }


        public ActionResult DetailSP(int id)
        {
            // Include the LoaiSanPham when fetching the product
            var sanPham = kn.SanPhams.Include(s => s.LoaiSanPham).FirstOrDefault(s => s.maSanPham == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            // Create an instance of the view model and populate it with data from sanPham
            var viewModel = new ThemSPViewModel
            {
                maSanPham = sanPham.maSanPham,
                tenSanPham = sanPham.tenSanPham,
                maLoaiSanPham = sanPham.LoaiSanPham?.maLoaiSanPham ?? 0, // Set to 0 or another default if null
                tenLoaiSanPham = sanPham.LoaiSanPham?.tenLoaiSanPham,
                giaBan = sanPham.giaBan,
                soLuong = sanPham.soLuong,
                ngayNhap = sanPham.ngayNhap,
                moTa = sanPham.moTa,
                hinhAnh = sanPham.hinhAnh,
                hinhAnhFile = null, 
                SelectedCategories = GetSelectedCategories(sanPham.maLoaiSanPham) 
            };

            return View(viewModel);
        }

        // Example method to populate SelectedCategories
        private List<LoaiSPViewModels> GetSelectedCategories(int? maLoaiSanPham)
        {
            var categories = kn.LoaiSanPhams
                .Select(c => new LoaiSPViewModels
                {
                 
                    maLoaiSanPham = c.maLoaiSanPham,
                    tenLoaiSanPham = c.tenLoaiSanPham,
                    IsSelected = c.maLoaiSanPham == maLoaiSanPham 
                }).ToList();

            return categories;
        }


        public ActionResult ExportToExcel()
        {
            // Get the list of products and map them to the export view model
            var sanPhams = kn.SanPhams
                .Select(sp => new ThemSPViewModel
                {
                    maSanPham = sp.maSanPham,
                    tenSanPham = sp.tenSanPham,
                    tenLoaiSanPham = sp.LoaiSanPham.tenLoaiSanPham,
                    giaBan = sp.giaBan,
                    soLuong = sp.soLuong,
                    ngayNhap = sp.ngayNhap,
                }).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sản phẩm");
                worksheet.Cell(1, 1).Value = "Mã sản phẩm";
                worksheet.Cell(1, 2).Value = "Tên sản phẩm";
                worksheet.Cell(1, 3).Value = "Loại sản phẩm";
                worksheet.Cell(1, 4).Value = "Giá bán";
                worksheet.Cell(1, 5).Value = "Số lượng";
                worksheet.Cell(1, 6).Value = "Ngày nhập";

                int row = 2;
                foreach (var sp in sanPhams)
                {
                    worksheet.Cell(row, 1).Value = sp.maSanPham;
                    worksheet.Cell(row, 2).Value = sp.tenSanPham;
                    worksheet.Cell(row, 3).Value = sp.tenLoaiSanPham;
                    worksheet.Cell(row, 4).Value = sp.giaBan;
                    worksheet.Cell(row, 5).Value = sp.soLuong;
                    worksheet.Cell(row, 6).Value = sp.ngayNhap;
                    row++;
                }

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;
                string fileName = "SanPham.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

    }
}
