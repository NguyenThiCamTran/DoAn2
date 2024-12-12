using ClosedXML.Excel;
using CuaHangTapHoas.Models;
using CuaHangTapHoas.ViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;

namespace CuaHangTapHoas.Controllers
{
    public class HoaDonController : Controller
    {
        CuaHangTapHoaDbContext kn = new CuaHangTapHoaDbContext();
        // GET: HoaDon
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContentHD(string searchString, int? page)
        {
            int pageSize = 10; // Number of items per page
            int pageNumber = (page ?? 1); // Current page, default to page 1 if page parameter is not provided

            // Query the invoice list and calculate the total amount for each invoice
            var hoaDons = from hd in kn.HoaDons
                          join tk in kn.TaiKhoans on hd.maTaiKhoan equals tk.maTaiKhoan
                          join ct in kn.ChiTietHoaDons on hd.maHoaDon equals ct.maHoaDon into details
                          from ct in details.DefaultIfEmpty() // Allow for invoices without details
                          join sp in kn.SanPhams on ct.maSanPham equals sp.maSanPham into products
                          from sp in products.DefaultIfEmpty() // Allow for details without products
                          group new { ct, sp } by new
                          {
                              hd.maHoaDon,
                              hd.ngayLap,
                              tk.tenDangNhap,
                              hd.TrangThai // Include status here
                          } into groupedHoaDon
                          select new HoaDonViewModel
                          {
                              maHoaDon = groupedHoaDon.Key.maHoaDon,
                              NgayLap = groupedHoaDon.Key.ngayLap,
                              tenDangNhap = groupedHoaDon.Key.tenDangNhap,
                              TrangThai = groupedHoaDon.Key.TrangThai, // Map the status to the ViewModel
                              TongTien = groupedHoaDon.Sum(g => g.ct != null ? (g.ct.soLuong * (g.sp != null ? g.sp.giaBan : 0)) : 0) // Calculate total amount for the invoice
                          };

            // Search if there is a search string
            if (!string.IsNullOrEmpty(searchString))
            {
                hoaDons = hoaDons.Where(t => t.tenDangNhap.Contains(searchString) || t.TrangThai.Contains(searchString));
            }

            // Count total items
            int totalItems = hoaDons.Count();

            // Calculate total pages
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Get data for the current page
            var model = hoaDons.OrderByDescending(ac => ac.maHoaDon)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchString = searchString;

            return View(model);
        }


        public ActionResult Create()
        {
            var model = new HoaDonViewModel
            {
                Products = kn.SanPhams.Select(p => new ProductItem
                {
                    MaSanPham = p.maSanPham,
                    TenSanPham = p.tenSanPham,
                    GiaBan = p.giaBan,
                    SoLuong = 0
                }).ToList()
            };

            ViewBag.TaiKhoans = kn.TaiKhoans.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HoaDonViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new HoaDon object
                var hoaDon = new HoaDon
                {
                    ngayLap = DateTime.Now,
                    maTaiKhoan = model.MaTaiKhoan,
                    TrangThai = "Chưa thanh toán", // Default status
                    tongTien = model.Products.Sum(p => p.SoLuong > 0 ? p.SoLuong * p.GiaBan : 0) // Calculate total price
                };

                kn.HoaDons.Add(hoaDon);
                kn.SaveChanges(); // Save HoaDon first to get maHoaDon

                // Add details for each product selected
                foreach (var product in model.Products)
                {
                    if (product.SoLuong > 0)
                    {
                        var chiTiet = new ChiTietHoaDon
                        {
                            maHoaDon = hoaDon.maHoaDon,
                            maSanPham = product.MaSanPham,
                            soLuong = product.SoLuong,
                            giaBan = product.GiaBan,
                            thanhTien = product.SoLuong * product.GiaBan // Calculate thanhTien
                        };
                        kn.ChiTietHoaDons.Add(chiTiet);
                    }
                }

                kn.SaveChanges(); // Save all ChiTietHoaDons
                return RedirectToAction("ContentHD");
            }

            // Reload Products and TaiKhoans if ModelState is invalid
            model.Products = kn.SanPhams.Select(p => new ProductItem
            {
                MaSanPham = p.maSanPham,
                TenSanPham = p.tenSanPham,
                GiaBan = p.giaBan,
                SoLuong = 0
            }).ToList();

            ViewBag.TaiKhoans = kn.TaiKhoans.ToList();
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            // Load the invoice from the database
            var hoaDon = kn.HoaDons.Include("ChiTietHoaDons").FirstOrDefault(h => h.maHoaDon == id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            var model = new HoaDonViewModel
            {
                maHoaDon = hoaDon.maHoaDon,
                NgayLap = hoaDon.ngayLap,
                MaTaiKhoan = hoaDon.maTaiKhoan,
                TrangThai = hoaDon.TrangThai,
                Products = hoaDon.ChiTietHoaDons.Select(c => new ProductItem
                {
                    MaSanPham = c.maSanPham,
                    TenSanPham = kn.SanPhams.Find(c.maSanPham).tenSanPham, // Fetch the product name
                    GiaBan = c.giaBan,
                    SoLuong = c.soLuong
                }).ToList()
                
            };
            var sanpham = kn.SanPhams.AsQueryable();
            foreach(var item in sanpham)
            {
                if (model.Products.Any(x => x.MaSanPham == item.maSanPham)) continue;
                model.Products.Add(new ProductItem
                {
                    MaSanPham = item.maSanPham,
                    TenSanPham = item.tenSanPham, // Fetch the product name
                    GiaBan = item.giaBan,
                    SoLuong = 0
                });
            }
            ViewBag.TaiKhoans = kn.TaiKhoans.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HoaDonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hoaDon = kn.HoaDons.Include("ChiTietHoaDons").FirstOrDefault(h => h.maHoaDon == model.maHoaDon);
                if (hoaDon == null)
                {
                    return HttpNotFound();
                }

                // Update the invoice details
                hoaDon.ngayLap = model.NgayLap;
                hoaDon.maTaiKhoan = model.MaTaiKhoan;

                // Clear existing product details
                hoaDon.ChiTietHoaDons.Clear();

                // Add updated details for each product
                foreach (var product in model.Products)
                {
                    if (product.SoLuong > 0)
                    {
                        var chiTiet = new ChiTietHoaDon
                        {
                            maHoaDon = hoaDon.maHoaDon,
                            maSanPham = product.MaSanPham,
                            soLuong = product.SoLuong,
                            giaBan = product.GiaBan,
                            thanhTien = product.SoLuong * product.GiaBan // Calculate thanhTien
                        };
                        hoaDon.ChiTietHoaDons.Add(chiTiet);
                    }
                }

                // Update total amount
                hoaDon.tongTien = model.Products.Sum(p => p.SoLuong * p.GiaBan);

                kn.SaveChanges(); // Save all changes to the database
                return RedirectToAction("ContentHD");
            }

            // Reload Products and TaiKhoans if ModelState is invalid
            model.Products = kn.SanPhams.Select(p => new ProductItem
            {
                MaSanPham = p.maSanPham,
                TenSanPham = p.tenSanPham,
                GiaBan = p.giaBan,
                SoLuong = 0
            }).ToList();

            ViewBag.TaiKhoans = kn.TaiKhoans.ToList();
            return View(model);
        }
        public ActionResult Details(int id)
        {
            var hoaDon = kn.HoaDons.Include("ChiTietHoaDons").FirstOrDefault(h => h.maHoaDon == id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            var model = new HoaDonViewModel
            {
                maHoaDon = hoaDon.maHoaDon,
                NgayLap = hoaDon.ngayLap,
                MaTaiKhoan = hoaDon.maTaiKhoan,
                TongTien = hoaDon.ChiTietHoaDons.Sum(c => c.soLuong * c.giaBan), // Calculate total amount
                TrangThai = hoaDon.TrangThai,
                Products = hoaDon.ChiTietHoaDons.Select(c => new ProductItem
                {
                    MaSanPham = c.maSanPham,
                    TenSanPham = kn.SanPhams.Find(c.maSanPham).tenSanPham, // Fetch product name
                    GiaBan = c.giaBan,
                    SoLuong = c.soLuong
                }).ToList()
            };

            ViewBag.TaiKhoans = kn.TaiKhoans.ToList();
            return View(model);
        }
        // In HoaDonController.cs
        public ActionResult ThanhToan(int id)
        {
            var hoaDon = kn.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            // Update the status to "Paid"
            hoaDon.TrangThai = "Đã thanh toán";
            kn.SaveChanges();

            // Redirect to the PrintInvoice action to generate the printable version
            return RedirectToAction("PrintInvoice", new { id = id });
        }
        public ActionResult PrintInvoice(int id)
        {
            var hoaDon = kn.HoaDons
                .Where(hd => hd.maHoaDon == id)
                .Select(hd => new HoaDonViewModel
                {
                    maHoaDon = hd.maHoaDon,
                    NgayLap = hd.ngayLap,
                    tenDangNhap = hd.TaiKhoan.tenDangNhap,
                    Products = hd.ChiTietHoaDons.Select(ct => new ProductItem
                    {
                        TenSanPham = ct.SanPham.tenSanPham,
                        SoLuong = ct.soLuong,
                        GiaBan = ct.SanPham.giaBan
                    }).ToList(),
                    TongTien = hd.ChiTietHoaDons.Sum(ct => ct.soLuong * ct.SanPham.giaBan),
                    TrangThai = hd.TrangThai
                })
                .FirstOrDefault();

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            return View("PrintInvoice", hoaDon); // Use a separate "PrintInvoice" view for printing
        }



        public ActionResult ExportToExcel()
        {
            // Retrieve the invoice data and map it to HoaDonViewModel
            List<HoaDonViewModel> hoaDonViewModels = kn.HoaDons
                .Select(hd => new HoaDonViewModel
                {
                    maHoaDon = hd.maHoaDon,
                    NgayLap = hd.ngayLap,
                    tenDangNhap = hd.TaiKhoan.tenDangNhap,
                    TrangThai = hd.TrangThai,
                    // Sum calculation with nullable decimal to prevent errors
                    TongTien = hd.ChiTietHoaDons.Sum(ct => (decimal?)ct.soLuong * (decimal?)ct.giaBan) ?? 0
                })
                .ToList();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Hóa đơn");

                // Column headers
                worksheet.Cell(1, 1).Value = "Mã hóa đơn";
                worksheet.Cell(1, 2).Value = "Ngày lập";
                worksheet.Cell(1, 3).Value = "Tên tài khoản";
                worksheet.Cell(1, 4).Value = "Trạng thái";
                worksheet.Cell(1, 5).Value = "Tổng tiền";

                int currentRow = 2;

                // Iterate over the list and populate worksheet cells
                foreach (var hoaDon in hoaDonViewModels)
                {
                    worksheet.Cell(currentRow, 1).Value = hoaDon.maHoaDon;
                    worksheet.Cell(currentRow, 2).Value = hoaDon.NgayLap.ToString("dd/MM/yyyy");
                    worksheet.Cell(currentRow, 3).Value = hoaDon.tenDangNhap;
                    worksheet.Cell(currentRow, 4).Value = hoaDon.TrangThai;
                    worksheet.Cell(currentRow, 5).Value = hoaDon.TongTien;
                    currentRow++;
                }

                // Save to Excel file
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = "HoaDon.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }




    }
}