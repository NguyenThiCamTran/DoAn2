USE [master]
GO
/****** Object:  Database [CuaHangTapHoa]    Script Date: 14/11/2024 11:49:08 PM ******/
CREATE DATABASE [CuaHangTapHoa]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CuaHangTapHoa', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\CuaHangTapHoa.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CuaHangTapHoa_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\CuaHangTapHoa_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CuaHangTapHoa] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CuaHangTapHoa].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CuaHangTapHoa] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET ARITHABORT OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CuaHangTapHoa] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CuaHangTapHoa] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CuaHangTapHoa] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CuaHangTapHoa] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CuaHangTapHoa] SET  MULTI_USER 
GO
ALTER DATABASE [CuaHangTapHoa] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CuaHangTapHoa] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CuaHangTapHoa] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CuaHangTapHoa] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CuaHangTapHoa] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CuaHangTapHoa] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CuaHangTapHoa] SET QUERY_STORE = OFF
GO
USE [CuaHangTapHoa]
GO
/****** Object:  Table [dbo].[ChiTietHoaDon]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietHoaDon](
	[maHoaDon] [int] NOT NULL,
	[maSanPham] [int] NOT NULL,
	[soLuong] [int] NOT NULL,
	[giaBan] [decimal](10, 2) NOT NULL,
	[thanhTien]  AS ([soLuong]*[giaBan]),
PRIMARY KEY CLUSTERED 
(
	[maHoaDon] ASC,
	[maSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuNhap]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuNhap](
	[maPhieuNhap] [int] NOT NULL,
	[maSanPham] [int] NOT NULL,
	[soLuong] [int] NOT NULL,
	[giaNhap] [decimal](10, 2) NOT NULL,
	[thanhTien]  AS ([soLuong]*[giaNhap]),
PRIMARY KEY CLUSTERED 
(
	[maPhieuNhap] ASC,
	[maSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDon]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDon](
	[maHoaDon] [int] IDENTITY(1,1) NOT NULL,
	[ngayLap] [date] NOT NULL,
	[tongTien] [decimal](10, 2) NOT NULL,
	[maTaiKhoan] [int] NULL,
	[TrangThai] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[maHoaDon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiSanPham]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiSanPham](
	[maLoaiSanPham] [int] IDENTITY(1,1) NOT NULL,
	[tenLoaiSanPham] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[maLoaiSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhaCungCap]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhaCungCap](
	[maNhaCungCap] [int] IDENTITY(1,1) NOT NULL,
	[tenNhaCungCap] [nvarchar](255) NULL,
	[soDienThoai] [varchar](20) NULL,
	[diaChi] [nvarchar](255) NULL,
	[email] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[maNhaCungCap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[maNhanVien] [int] IDENTITY(1,1) NOT NULL,
	[tenNhanVien] [nvarchar](255) NULL,
	[soDienThoai] [varchar](20) NULL,
	[diaChi] [nvarchar](255) NULL,
	[email] [nvarchar](255) NULL,
	[chucVu] [nvarchar](255) NULL,
	[luong] [decimal](10, 2) NULL,
	[maTaiKhoan] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[maNhanVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuNhapHang]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuNhapHang](
	[maPhieuNhap] [int] IDENTITY(1,1) NOT NULL,
	[maNhaCungCap] [int] NULL,
	[ngayNhap] [date] NOT NULL,
	[tongTien] [decimal](10, 2) NOT NULL,
	[maTaiKhoan] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[maPhieuNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[maSanPham] [int] IDENTITY(1,1) NOT NULL,
	[tenSanPham] [nvarchar](255) NULL,
	[giaBan] [decimal](10, 2) NOT NULL,
	[soLuong] [int] NOT NULL,
	[ngayNhap] [date] NULL,
	[moTa] [nvarchar](max) NULL,
	[hinhAnh] [nvarchar](max) NULL,
	[maLoaiSanPham] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[maSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 14/11/2024 11:49:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[maTaiKhoan] [int] IDENTITY(1,1) NOT NULL,
	[tenDangNhap] [varchar](50) NOT NULL,
	[matKhau] [nvarchar](255) NULL,
	[email] [varchar](100) NULL,
	[soDienThoai] [varchar](20) NULL,
	[ngayTao] [date] NULL,
	[loaiTaiKhoan] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[maTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (1, 1, 2, CAST(15000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (1, 2, 1, CAST(20000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (2, 3, 1, CAST(10000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (3, 4, 10, CAST(18000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (4, 5, 4, CAST(30000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (5, 1, 2, CAST(36000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (6, 3, 1, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (6, 9, 2, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (7, 3, 1, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (7, 5, 1, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (7, 7, 1, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (7, 9, 2, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (8, 1, 1, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (8, 3, 2, CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (9, 1, 2, CAST(15000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (10, 1, 4, CAST(15000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (10, 3, 3, CAST(10000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (10, 4, 2, CAST(18000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (10, 5, 1, CAST(30000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (10, 8, 1, CAST(250000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (10, 12, 1, CAST(9.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (11, 2, 1, CAST(19000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietHoaDon] ([maHoaDon], [maSanPham], [soLuong], [giaBan]) VALUES (11, 5, 2, CAST(18000.00 AS Decimal(10, 2)))
GO
INSERT [dbo].[ChiTietPhieuNhap] ([maPhieuNhap], [maSanPham], [soLuong], [giaNhap]) VALUES (1, 1, 100, CAST(14000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietPhieuNhap] ([maPhieuNhap], [maSanPham], [soLuong], [giaNhap]) VALUES (1, 2, 50, CAST(18000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietPhieuNhap] ([maPhieuNhap], [maSanPham], [soLuong], [giaNhap]) VALUES (2, 3, 200, CAST(9000.00 AS Decimal(10, 2)))
INSERT [dbo].[ChiTietPhieuNhap] ([maPhieuNhap], [maSanPham], [soLuong], [giaNhap]) VALUES (3, 4, 500, CAST(17000.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[HoaDon] ON 

INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (1, CAST(N'2024-10-12' AS Date), CAST(30000.00 AS Decimal(10, 2)), 2, N'Đã thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (2, CAST(N'2024-10-12' AS Date), CAST(15000.00 AS Decimal(10, 2)), 3, N'Chưa thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (3, CAST(N'2024-10-13' AS Date), CAST(40000.00 AS Decimal(10, 2)), 4, N'Chưa thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (4, CAST(N'2024-10-13' AS Date), CAST(50000.00 AS Decimal(10, 2)), 5, N'Chưa thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (5, CAST(N'2024-10-14' AS Date), CAST(72000.00 AS Decimal(10, 2)), 1, N'Chưa thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (6, CAST(N'2024-10-09' AS Date), CAST(0.00 AS Decimal(10, 2)), 2, N'Đã thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (7, CAST(N'2024-10-09' AS Date), CAST(0.00 AS Decimal(10, 2)), 2, N'Đã thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (8, CAST(N'2024-10-03' AS Date), CAST(0.00 AS Decimal(10, 2)), 2, N'Đã thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (9, CAST(N'2024-10-16' AS Date), CAST(30000.00 AS Decimal(10, 2)), 5, N'Đã thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (10, CAST(N'2024-10-08' AS Date), CAST(406009.00 AS Decimal(10, 2)), 4, N'Đã thanh toán')
INSERT [dbo].[HoaDon] ([maHoaDon], [ngayLap], [tongTien], [maTaiKhoan], [TrangThai]) VALUES (11, CAST(N'2024-11-14' AS Date), CAST(55000.00 AS Decimal(10, 2)), 1, N'Đã thanh toán')
SET IDENTITY_INSERT [dbo].[HoaDon] OFF
GO
SET IDENTITY_INSERT [dbo].[LoaiSanPham] ON 

INSERT [dbo].[LoaiSanPham] ([maLoaiSanPham], [tenLoaiSanPham]) VALUES (1, N'Đồ gia dụng')
INSERT [dbo].[LoaiSanPham] ([maLoaiSanPham], [tenLoaiSanPham]) VALUES (2, N'Bánh các loại')
INSERT [dbo].[LoaiSanPham] ([maLoaiSanPham], [tenLoaiSanPham]) VALUES (3, N'Nước giải khát')
INSERT [dbo].[LoaiSanPham] ([maLoaiSanPham], [tenLoaiSanPham]) VALUES (5, N'Gạo các loại')
INSERT [dbo].[LoaiSanPham] ([maLoaiSanPham], [tenLoaiSanPham]) VALUES (6, N'Sữa tươi')
INSERT [dbo].[LoaiSanPham] ([maLoaiSanPham], [tenLoaiSanPham]) VALUES (7, N'Dầu ăn các loại')
INSERT [dbo].[LoaiSanPham] ([maLoaiSanPham], [tenLoaiSanPham]) VALUES (9, N'Gia vị các loại')
SET IDENTITY_INSERT [dbo].[LoaiSanPham] OFF
GO
SET IDENTITY_INSERT [dbo].[NhaCungCap] ON 

INSERT [dbo].[NhaCungCap] ([maNhaCungCap], [tenNhaCungCap], [soDienThoai], [diaChi], [email]) VALUES (1, N'Công ty ABC', N'0123456789', N'123 Ðuờng A, Hà Nội', N'abc@example.com')
INSERT [dbo].[NhaCungCap] ([maNhaCungCap], [tenNhaCungCap], [soDienThoai], [diaChi], [email]) VALUES (2, N'Công ty XYZ', N'0987654321', N'456 Ðuờng B, TP. HCM', N'xyz@example.com')
INSERT [dbo].[NhaCungCap] ([maNhaCungCap], [tenNhaCungCap], [soDienThoai], [diaChi], [email]) VALUES (3, N'Công ty DEF', N'0345678912', N'789 Ðuờng C, Ðà Nẵng', N'def@example.com')
SET IDENTITY_INSERT [dbo].[NhaCungCap] OFF
GO
SET IDENTITY_INSERT [dbo].[NhanVien] ON 

INSERT [dbo].[NhanVien] ([maNhanVien], [tenNhanVien], [soDienThoai], [diaChi], [email], [chucVu], [luong], [maTaiKhoan]) VALUES (1, N'Nguyễn Thị Cẩm Trân', N'0123456789', N'72, P1, TP.Cao Lãnh', N'nva@example.com', N'Quản Lý', CAST(15000000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[NhanVien] ([maNhanVien], [tenNhanVien], [soDienThoai], [diaChi], [email], [chucVu], [luong], [maTaiKhoan]) VALUES (2, N'Trần Thị Bích', N'0987654321', N'456 P3 B, TP. Cao Lãnh', N'ttb@example.com', N'Quản Lý', CAST(7000000.00 AS Decimal(10, 2)), 2)
INSERT [dbo].[NhanVien] ([maNhanVien], [tenNhanVien], [soDienThoai], [diaChi], [email], [chucVu], [luong], [maTaiKhoan]) VALUES (3, N'Phạm Văn An', N'0321456789', N'Cap Lãnh, Đồng Tháp', N'pvc@example.com', N'Nhân Viên', CAST(6000000.00 AS Decimal(10, 2)), 3)
INSERT [dbo].[NhanVien] ([maNhanVien], [tenNhanVien], [soDienThoai], [diaChi], [email], [chucVu], [luong], [maTaiKhoan]) VALUES (4, N'Lê Thị Dung', N'0998765432', N'321 Phố D, Hải Phòng', N'ltd@example.com', N'Nhân Viên', CAST(7500000.00 AS Decimal(10, 2)), 4)
INSERT [dbo].[NhanVien] ([maNhanVien], [tenNhanVien], [soDienThoai], [diaChi], [email], [chucVu], [luong], [maTaiKhoan]) VALUES (5, N'Ðặng Văn Thanh', N'0765432109', N'Cần Thơ', N'dve@example.com', N'Nhân Viên', CAST(5000000.00 AS Decimal(10, 2)), 5)
INSERT [dbo].[NhanVien] ([maNhanVien], [tenNhanVien], [soDienThoai], [diaChi], [email], [chucVu], [luong], [maTaiKhoan]) VALUES (6, N'Nguyễn Thị Hoa', N'0347176640', N'jklfjg', N'cam18tran7@gmail.com', N'Nhân Viên', CAST(3000000.00 AS Decimal(10, 2)), 6)
SET IDENTITY_INSERT [dbo].[NhanVien] OFF
GO
SET IDENTITY_INSERT [dbo].[PhieuNhapHang] ON 

INSERT [dbo].[PhieuNhapHang] ([maPhieuNhap], [maNhaCungCap], [ngayNhap], [tongTien], [maTaiKhoan]) VALUES (1, 1, CAST(N'2024-10-11' AS Date), CAST(500000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[PhieuNhapHang] ([maPhieuNhap], [maNhaCungCap], [ngayNhap], [tongTien], [maTaiKhoan]) VALUES (2, 2, CAST(N'2024-10-12' AS Date), CAST(300000.00 AS Decimal(10, 2)), 2)
INSERT [dbo].[PhieuNhapHang] ([maPhieuNhap], [maNhaCungCap], [ngayNhap], [tongTien], [maTaiKhoan]) VALUES (3, 3, CAST(N'2024-10-13' AS Date), CAST(450000.00 AS Decimal(10, 2)), 3)
SET IDENTITY_INSERT [dbo].[PhieuNhapHang] OFF
GO
SET IDENTITY_INSERT [dbo].[SanPham] ON 

INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (1, N'Sữa tươi tiệt trùng ít đường Vinamilk', CAST(36000.00 AS Decimal(10, 2)), 100, CAST(N'2024-10-10' AS Date), N'Sữa tươi Vinamilk là thương hiệu sữa được tin dùng hàng đầu Việt Nam với thành phần từ 100% sữa tươi, chứa nhiều dưỡng chất tốt cho xương và hệ miễn dịch. Sữa tươi ít đường Vinamilk 100% bịch 220ml được cắt giảm lượng đường nhưng vẫn thơm ngon, dễ uống và giàu dinh dưỡng tự nhiên từ sữa.', N'suatuoiVinamilk.jpg', 6)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (2, N'Bánh Quy CoSy', CAST(19000.00 AS Decimal(10, 2)), 50, CAST(N'2024-10-10' AS Date), N'Bánh quy giòn rụm, thơm ngon và chứa nhiều dưỡng chất cho cơ thể. Bánh quy sữa Cosy Marie gói 120g tiện lợi, kích thích vị giác với sữa béo, tạo nên sự ngon miệng khi ăn. Bánh quy Cosy là thương hiệu Việt lớn, chuyên sản xuất bánh kẹo chất lượng cao, an tâm cho người dùng', N'banhquyCoSy.jpg', 2)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (3, N'Trà mật ong BONCHA', CAST(9000.00 AS Decimal(10, 2)), 200, CAST(N'2024-10-11' AS Date), N'Trà mật ong Boncha vị chanh 450ml là sự kết hợp tinh tế giữa 100% mật ong nguyên chất và chanh tươi, mang đến hương vị thanh mát, ngọt ngào. Với trà xanh nguyên lá, mỗi giọt trà Boncha vị Chanh không chỉ là cốc thức uống giải khát lý tưởng mà còn là điểm nhấn cho sở thích truyền thống của bạn. Hãy thưởng thức và cảm nhận sự hòa quyện tuyệt vời này ngay hôm nay!', N'boncha-olongdao-background.jpg', 3)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (4, N'Gạo thơm Neptune ST25 Extra túi 5kg', CAST(140000.00 AS Decimal(10, 2)), 500, CAST(N'2024-10-09' AS Date), N'Gạo Neptune chất thơm ngon, chất lượng được các bà nội trợ tin dùng. Gạo thơm Neptune ST25 Extra túi 5kg mang hương vị thơm ngon, độ mềm và dẻo vừa phải mang đến bữa ăn của bạn thêm tròn vị đậm đà và hấp dẫn. Gạo không tẩm các hoá chất độc hại an toàn cho người sử dụng. ', N'gao-thom-neptune.jpg', 5)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (5, N'Dầu ăn cao cấp Happi Koki chai 400ml', CAST(18000.00 AS Decimal(10, 2)), 50, CAST(N'2024-10-08' AS Date), N'Được làm từ 100% dầu đậu nành nguyên chất tinh luyện, vitamin A. Sản phẩm giàu 3, 6, 9 sẽ giúp làm giảm nguy cơ tăng Cholesterol trong máu, tăng cường hệ thống miễn dịch, cho bạn nguồn dinh dưỡng tuyệt vời và một trái tim khoẻ mạnh.', N'dau-an-cao-cap-happi-koki-chai-400ml.jpg', 7)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (6, N'Chảo chống dính đáy từ Sunhouse Saving SV20MY 20cm', CAST(1290000.00 AS Decimal(10, 2)), 10, CAST(N'2024-10-22' AS Date), N'Chảo chống dính Sunhouse là thương hiệu chảo chống dính nổi tiếng hàng đầu với đa dạng mẫu mã, kích thước. Chảo chống dính đáy từ Sunhouse Saving SV20MY 20cm thiết kế gọn nhẹ, sử dụng được cho hầu hết các loại bếp, dùng lâu bền, an toàn khi chế biến.', N'chao-chong-dinh-day-tu-sunhouse.jpg', 1)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (7, N'Sữa tắm Dove dưỡng ẩm chuyên sâu 500g', CAST(99000.00 AS Decimal(10, 2)), 10, CAST(N'2024-10-22' AS Date), N'Thương hiệu Dove với sản phẩm sữa tắm kết hợp các thành phần thiên nhiên. Sữa tắm dưỡng thể Dove dưỡng ẩm chuyên sâu 500g giúp làn da luôn ẩm mượt, mịn màng giúp cung cấp độ ẩm cho da và khiến da sáng rạng rỡ', N'sua-tam-duong-the-dove.jpg', 1)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (8, N'Dầu gội Head & Shoulders làm sạch gàu mềm mượt óng ả 850ml', CAST(172000.00 AS Decimal(10, 2)), 10, CAST(N'2024-10-22' AS Date), N'Dầu gội Head & Shoulders là dòng sản phẩm dầu gội dành cho nam giới được nhiều người yêu thích. Dầu gội sạch gàu Head & Shoulders bạc hà mát rượi 850ml công thức chứa bạc hà giúp gội sạch, làm mát lạnh tóc và da đầu sạch gàu và ngăn gàu quay trở lại', N'daugoiHead-Shoulders.jpg', 1)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (9, N'Gạo thơm Vua Gạo ST25 túi 5kg', CAST(140000.00 AS Decimal(10, 2)), 15, CAST(N'2024-10-22' AS Date), N'Gạo hạt dài, thơm đặc trưng và nở ít tạo giác ăn ngon miệng. Gạo thơm Vua Gạo ST25 túi 5kg vị dẻo, vị thơm đặc trưng sẽ kích thích vị giác giúp thưởng thức các món ăn khác tuyệt vời hơn. Gạo đảm bảo an toàn, không tẩy trắng, không chứa chất bảo quản. Túi 5kg phù hợp với cả gia đình.', N'sellingpoint.jpg', 5)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (10, N'Bánh quế vị sữa chua dâu Kokola Majorico ly 120g', CAST(30000.00 AS Decimal(10, 2)), 15, CAST(N'2024-10-22' AS Date), N'Bánh quế vị sữa chua dâu Kokola Majorico ly 120g là loại bánh quế có lớp vỏ mỏng, giòn, với vị sữa chua dâu vô cùng hấp dẫn. Bánh quế Kokola được làm từ những nguyên liệu chọn lọc, bánh giòn rụm, hương vị hấp dẫn sẽ thích hợp cho những buổi gặp mặt, tụ họp, tiệc tùng bên bạn bè, người thân.', N'banh-que-vi-chocolate-kokola.jpg', 2)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (11, N'Dầu thực vật tinh luyện Bếp Hồng chai 1 lít', CAST(36000.00 AS Decimal(10, 2)), 10, CAST(N'2024-10-26' AS Date), N'Dầu thực vật tinh luyện Bếp Hồng chai 1 lít không sử dụng chất bảo quản, bổ sung năng lượng và vitamin A, E tốt cho cơ thể. Dầu ăn Bếp Hồng là thương hiệu dầu ăn thông dụng, được rất nhiều người tiêu dùng ưa chuộng bởi chất lượng cùng giá thành tốt trên thị trường.', N'dau-thuc-vat-tinh-luyen-bep-hong-chai-1-lit.jpg', 7)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (12, N'Sữa tắm dưỡng sáng Hazeline matcha lựu đỏ 796ml', CAST(69000.00 AS Decimal(10, 2)), 10, CAST(N'2024-10-26' AS Date), N'Sữa tắm Hazeline là thương hiệu sữa tắm của Anh và Hà Lan. Sữa tắm dưỡng ẩm sáng da Hazeline yến mạch dâu tằm 796ml giúp nuôi dưỡng da khỏe mạnh, chống lão hóa, giảm nếp nhăn, tăng cường độ đàn hồi và làm dịu làn da chịu tác hại của ánh nắng', N'sua-tam-duong-sang-hazeline.jpg', 1)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (15, N'Bộ 17 hộp thực phẩm nhựa chữ nhật Biohome', CAST(19000.00 AS Decimal(10, 2)), 5, CAST(N'2024-11-14' AS Date), N'Hộp đựng thực phẩm Biohome là thương hiệu hộp đựng thực phẩm uy tín, đa dạng kiểu dáng, chất lượng, tha hồ cho bạn chọn lựa. Bộ 17 hộp thực phẩm nhựa chữ nhật Biohome với 6 mức dung tích khác nhau cũng như đa dạng kiểu dáng, linh hoạt sử dụng, chất liệu dày dặn chịu nhiệt tốt.', N'bo-17-hop-thuc-pham-nhua.jpg', 1)
INSERT [dbo].[SanPham] ([maSanPham], [tenSanPham], [giaBan], [soLuong], [ngayNhap], [moTa], [hinhAnh], [maLoaiSanPham]) VALUES (16, N'Hạt nêm Knorr thịt thăn, xương ống, tủy gói 900g', CAST(65000.00 AS Decimal(10, 2)), 10, CAST(N'2024-11-14' AS Date), N'Hạt nêm thịt thăn, xương ống, tủy Knorr gói 900g công thức cải tiến với chiết xuất thịt thăn, xương ống và tủy heo sạch được hầm trong nhiều giờ liền mang đến bữa ăn ngọt thanh, tròn vị. Hạt nêm Knorr là thương hiệu hạt nêm số 1 Việt Nam giúp mọi món ăn thơm ngon khó cưỡng chỉ với 1 bước nêm.', N'hat-nem-thit-than-xuong-ong-tuy-knorr-goi-900g.jpg', 9)
SET IDENTITY_INSERT [dbo].[SanPham] OFF
GO
SET IDENTITY_INSERT [dbo].[TaiKhoan] ON 

INSERT [dbo].[TaiKhoan] ([maTaiKhoan], [tenDangNhap], [matKhau], [email], [soDienThoai], [ngayTao], [loaiTaiKhoan]) VALUES (1, N'admin', N'admin', N'admin@example.com', N'0123456789', CAST(N'2024-10-14' AS Date), N'admin')
INSERT [dbo].[TaiKhoan] ([maTaiKhoan], [tenDangNhap], [matKhau], [email], [soDienThoai], [ngayTao], [loaiTaiKhoan]) VALUES (2, N'nv1', N'user123', N'user1@example.com', N'0987654321', CAST(N'2024-10-14' AS Date), N'admin')
INSERT [dbo].[TaiKhoan] ([maTaiKhoan], [tenDangNhap], [matKhau], [email], [soDienThoai], [ngayTao], [loaiTaiKhoan]) VALUES (3, N'nv2', N'user456', N'user2@example.com', N'0112345678', CAST(N'2024-10-14' AS Date), N'user')
INSERT [dbo].[TaiKhoan] ([maTaiKhoan], [tenDangNhap], [matKhau], [email], [soDienThoai], [ngayTao], [loaiTaiKhoan]) VALUES (4, N'nv3', N'user789', N'user3@example.com', N'0321456789', CAST(N'2024-10-14' AS Date), N'user')
INSERT [dbo].[TaiKhoan] ([maTaiKhoan], [tenDangNhap], [matKhau], [email], [soDienThoai], [ngayTao], [loaiTaiKhoan]) VALUES (5, N'nv4', N'user101', N'user4@example.com', N'0987123456', CAST(N'2024-10-14' AS Date), N'user')
INSERT [dbo].[TaiKhoan] ([maTaiKhoan], [tenDangNhap], [matKhau], [email], [soDienThoai], [ngayTao], [loaiTaiKhoan]) VALUES (6, N'nv5', N'123', N'camtran@gmail.com', N'0347176640', CAST(N'2024-10-18' AS Date), N'user')
SET IDENTITY_INSERT [dbo].[TaiKhoan] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__TaiKhoan__59267D4AD833AF49]    Script Date: 14/11/2024 11:49:08 PM ******/
ALTER TABLE [dbo].[TaiKhoan] ADD UNIQUE NONCLUSTERED 
(
	[tenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TaiKhoan] ADD  DEFAULT (getdate()) FOR [ngayTao]
GO
ALTER TABLE [dbo].[TaiKhoan] ADD  DEFAULT ('user') FOR [loaiTaiKhoan]
GO
ALTER TABLE [dbo].[ChiTietHoaDon]  WITH CHECK ADD  CONSTRAINT [FK__ChiTietHo__maHoa__286302EC] FOREIGN KEY([maHoaDon])
REFERENCES [dbo].[HoaDon] ([maHoaDon])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietHoaDon] CHECK CONSTRAINT [FK__ChiTietHo__maHoa__286302EC]
GO
ALTER TABLE [dbo].[ChiTietHoaDon]  WITH CHECK ADD  CONSTRAINT [FK__ChiTietHo__maSan__29572725] FOREIGN KEY([maSanPham])
REFERENCES [dbo].[SanPham] ([maSanPham])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietHoaDon] CHECK CONSTRAINT [FK__ChiTietHo__maSan__29572725]
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap]  WITH CHECK ADD  CONSTRAINT [FK__ChiTietPh__maPhi__30F848ED] FOREIGN KEY([maPhieuNhap])
REFERENCES [dbo].[PhieuNhapHang] ([maPhieuNhap])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap] CHECK CONSTRAINT [FK__ChiTietPh__maPhi__30F848ED]
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap]  WITH CHECK ADD  CONSTRAINT [FK__ChiTietPh__maSan__31EC6D26] FOREIGN KEY([maSanPham])
REFERENCES [dbo].[SanPham] ([maSanPham])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap] CHECK CONSTRAINT [FK__ChiTietPh__maSan__31EC6D26]
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD  CONSTRAINT [FK_HoaDon_TaiKhoan] FOREIGN KEY([maTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([maTaiKhoan])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HoaDon] CHECK CONSTRAINT [FK_HoaDon_TaiKhoan]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [FK__NhanVien__maTaiK__3C69FB99] FOREIGN KEY([maTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([maTaiKhoan])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [FK__NhanVien__maTaiK__3C69FB99]
GO
ALTER TABLE [dbo].[PhieuNhapHang]  WITH CHECK ADD  CONSTRAINT [FK__PhieuNhap__maNha__2E1BDC42] FOREIGN KEY([maNhaCungCap])
REFERENCES [dbo].[NhaCungCap] ([maNhaCungCap])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhieuNhapHang] CHECK CONSTRAINT [FK__PhieuNhap__maNha__2E1BDC42]
GO
ALTER TABLE [dbo].[PhieuNhapHang]  WITH CHECK ADD  CONSTRAINT [FK__PhieuNhap__maTai__3D5E1FD2] FOREIGN KEY([maTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([maTaiKhoan])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhieuNhapHang] CHECK CONSTRAINT [FK__PhieuNhap__maTai__3D5E1FD2]
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD  CONSTRAINT [FK_SanPham_LoaiSanPham] FOREIGN KEY([maLoaiSanPham])
REFERENCES [dbo].[LoaiSanPham] ([maLoaiSanPham])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [FK_SanPham_LoaiSanPham]
GO
USE [master]
GO
ALTER DATABASE [CuaHangTapHoa] SET  READ_WRITE 
GO
