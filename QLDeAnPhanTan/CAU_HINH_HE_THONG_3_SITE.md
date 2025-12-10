# 🔗 CẤU HÌNH HỆ THỐNG CSDL PHÂN TÁN - 3 SITE

## 📋 THÔNG TIN CẤU HÌNH HỆ THỐNG

### 🖥️ Site Gốc (Master Site)
- **Instance**: `MSI\YLC`
- **Database**: `QLDeAn`
- **Vai trò**: 
  - Chứa các View toàn cục (UNION ALL)
  - Có Link Server đến cả 2 Site P1 và P2
  - **Ứng dụng Client kết nối vào đây**
- **Link Server**:
  - `LINK_P1` → kết nối đến `DESKTOP-SEERKGC\ANHTHU`
  - `LINK_P2` → kết nối đến `LAPTOP-F37K83BK\SQLEXPRESS`

### 🖥️ Site P1 (Phân mảnh 1)
- **Instance**: `DESKTOP-SEERKGC\ANHTHU`
- **Database**: `QLDeAn_P1`
- **Dữ liệu**: NC01, NC02 (Nhóm nghiên cứu P1)
- **Link Server**: Đã được thiết lập sẵn

### 🖥️ Site P2 (Phân mảnh 2)
- **Instance**: `LAPTOP-F37K83BK\SQLEXPRESS`
- **Database**: `QLDeAn_P2`
- **Dữ liệu**: NC03, NC04 (Nhóm nghiên cứu P2)
- **Link Server**: Đã được thiết lập sẵn

---

## ✅ TRẠNG THÁI HIỆN TẠI

- ✅ Link Server đã được thiết lập sẵn giữa các Site
- ✅ CSDL QLDeAn_P1 và QLDeAn_P2 đã có dữ liệu phân mảnh

---

## 🎯 CÁC BƯỚC CÒN LẠI

### Bước 1: Tạo Database QLDeAn tại Site Gốc (MSI\YLC)

Kết nối đến `MSI\YLC` và chạy:

```sql
-- Tạo database QLDeAn tại Site Gốc
CREATE DATABASE QLDeAn;
GO

USE QLDeAn;
GO

PRINT '✓ Đã tạo database QLDeAn tại Site Gốc (MSI\YLC)'
```

### Bước 2: Tạo View Toàn cục tại Site Gốc

Kết nối đến `MSI\YLC` và chạy file:
```
../TaiLieu_Buoc4_ThietLapTinhTrongSuotPhanManh/1.TaoViewToanCuc(UNION_ALL).sql
```

Hoặc chạy trực tiếp:

```sql
USE QLDeAn;
GO

-- 1. View nhomnc_view
DROP VIEW IF EXISTS nhomnc_view;
GO

CREATE VIEW nhomnc_view AS
SELECT manhom COLLATE Vietnamese_CI_AS AS manhom, 
       tennhom COLLATE Vietnamese_CI_AS AS tennhom, 
       tenphong COLLATE Vietnamese_CI_AS AS tenphong 
FROM LINK_P1.QLDeAn_P1.dbo.nhomnc
UNION ALL
SELECT manhom COLLATE Vietnamese_CI_AS, 
       tennhom COLLATE Vietnamese_CI_AS, 
       tenphong COLLATE Vietnamese_CI_AS 
FROM LINK_P2.QLDeAn_P2.dbo.nhomnc;
GO

-- 2. View nhanvien_view
DROP VIEW IF EXISTS nhanvien_view;
GO

CREATE VIEW nhanvien_view AS
SELECT manv COLLATE Vietnamese_CI_AS AS manv, 
       hoten COLLATE Vietnamese_CI_AS AS hoten, 
       manhom COLLATE Vietnamese_CI_AS AS manhom
FROM LINK_P1.QLDeAn_P1.dbo.nhanvien
UNION ALL
SELECT manv COLLATE Vietnamese_CI_AS, 
       hoten COLLATE Vietnamese_CI_AS, 
       manhom COLLATE Vietnamese_CI_AS
FROM LINK_P2.QLDeAn_P2.dbo.nhanvien;
GO

-- 3. View dean_view
DROP VIEW IF EXISTS dean_view;
GO

CREATE VIEW dean_view AS
SELECT mada COLLATE Vietnamese_CI_AS AS mada, 
       tenda COLLATE Vietnamese_CI_AS AS tenda, 
       manhom COLLATE Vietnamese_CI_AS AS manhom
FROM LINK_P1.QLDeAn_P1.dbo.dean
UNION ALL
SELECT mada COLLATE Vietnamese_CI_AS, 
       tenda COLLATE Vietnamese_CI_AS, 
       manhom COLLATE Vietnamese_CI_AS
FROM LINK_P2.QLDeAn_P2.dbo.dean;
GO

-- 4. View thamgia_view
DROP VIEW IF EXISTS thamgia_view;
GO

CREATE VIEW thamgia_view AS
SELECT manv COLLATE Vietnamese_CI_AS AS manv, 
       mada COLLATE Vietnamese_CI_AS AS mada 
FROM LINK_P1.QLDeAn_P1.dbo.thamgia
UNION ALL
SELECT manv COLLATE Vietnamese_CI_AS, 
       mada COLLATE Vietnamese_CI_AS
FROM LINK_P2.QLDeAn_P2.dbo.thamgia;
GO

PRINT '✓ Đã tạo tất cả View toàn cục tại Site Gốc!'
```

### Bước 3: Kiểm tra View

Chạy tại `MSI\YLC`:

```sql
USE QLDeAn;
GO

-- Kiểm tra các View
SELECT 'nhomnc_view' AS ViewName, COUNT(*) AS RecordCount FROM nhomnc_view
UNION ALL
SELECT 'nhanvien_view', COUNT(*) FROM nhanvien_view
UNION ALL
SELECT 'dean_view', COUNT(*) FROM dean_view
UNION ALL
SELECT 'thamgia_view', COUNT(*) FROM thamgia_view;

-- Xem dữ liệu chi tiết
SELECT * FROM nhomnc_view ORDER BY manhom;
SELECT * FROM nhanvien_view ORDER BY manv;
SELECT * FROM dean_view ORDER BY mada;
SELECT * FROM thamgia_view;
```

Kết quả mong đợi:
- `nhomnc_view`: 4 bản ghi (NC01, NC02, NC03, NC04)
- `nhanvien_view`: 5 bản ghi
- `dean_view`: 3 bản ghi
- `thamgia_view`: 5 bản ghi

---

## 🚀 CHẠY ỨNG DỤNG

### Bước 1: Build và chạy ứng dụng

```powershell
cd "d:\3.CSDLPhanTan\LyThuyet\Project\QLDeAnPhanTan"
dotnet restore
dotnet build
dotnet run
```

### Bước 2: Kết nối đến Site Gốc

1. Vào menu **Hệ thống** > **Kết nối CSDL**
2. Nhập thông tin:
   - **Server Name**: `MSI\YLC`
   - **Username**: `sa` (hoặc tài khoản của bạn)
   - **Password**: Mật khẩu tài khoản sa trên MSI\YLC

### Bước 3: Sử dụng ứng dụng

#### Tab 1: Quản lý Dữ liệu
- Click các nút: **Nhóm NC**, **Nhân Viên**, **Đề Án**, **Tham Gia**
- Xem dữ liệu toàn bộ hệ thống từ cả 2 phân mảnh

#### Tab 2: Truy vấn
- Nhập mã nhóm (ví dụ: `NC01`)
- Click **Truy vấn Mức 1**: Sử dụng View (Trong suốt phân mảnh)
- Click **Truy vấn Mức 2**: Sử dụng UNION ALL (Trong suốt vị trí)
- So sánh kết quả giống nhau

#### Tab 3: Cập nhật
- Nhập mã nhóm (ví dụ: `NC01`)
- Hệ thống tự động xác định Site chứa nhóm đó
- Nhập tên phòng mới và Click **Cập nhật**
- Kiểm tra thay đổi trong Tab Quản lý Dữ liệu

---

## 🔍 KIỂM TRA HỆ THỐNG

### Kiểm tra Link Server tại Site Gốc

Chạy tại `MSI\YLC`:

```sql
-- Kiểm tra Link Server đã tồn tại
SELECT name, provider, data_source 
FROM sys.servers 
WHERE is_linked = 1;

-- Kết quả mong đợi:
-- LINK_P1 → DESKTOP-SEERKGC\ANHTHU
-- LINK_P2 → LAPTOP-F37K83BK\SQLEXPRESS
```

### Kiểm tra kết nối đến các Site

```sql
-- Test kết nối LINK_P1
EXEC sp_testlinkedserver 'LINK_P1';
-- Kết quả: NULL (thành công) hoặc lỗi

-- Test kết nối LINK_P2
EXEC sp_testlinkedserver 'LINK_P2';
```

### Kiểm tra truy vấn dữ liệu

```sql
-- Truy vấn từ P1
SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhomnc;

-- Truy vấn từ P2
SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhomnc;

-- Truy vấn từ View toàn cục
SELECT * FROM QLDeAn.dbo.nhomnc_view;
```

---

## ⚠️ XỬ LÝ SỰ CỐ

### Lỗi: "Invalid object name 'nhomnc_view'"
**Nguyên nhân**: Chưa tạo View tại Site Gốc

**Giải pháp**: Chạy lại Bước 2 (Tạo View Toàn cục)

### Lỗi: "Could not find server 'LINK_P1'"
**Nguyên nhân**: Link Server không tồn tại hoặc chưa thiết lập

**Giải pháp**: 
```sql
-- Kiểm tra Link Server
SELECT * FROM sys.servers WHERE is_linked = 1;

-- Nếu thiếu, tạo lại Link Server
```

### Lỗi: "Login failed for user 'sa'"
**Nguyên nhân**: Sai mật khẩu hoặc tài khoản

**Giải pháp**: Kiểm tra lại thông tin đăng nhập

---

## 📊 SƠ ĐỒ KẾT NỐI

```
┌─────────────────────────────────────┐
│     Ứng dụng C# Client              │
│     (Windows Forms)                 │
└──────────────┬──────────────────────┘
               │ Kết nối
               ▼
┌─────────────────────────────────────┐
│  Site Gốc: MSI\YLC                  │
│  Database: QLDeAn                   │
│  - nhomnc_view (UNION ALL)          │
│  - nhanvien_view (UNION ALL)        │
│  - dean_view (UNION ALL)            │
│  - thamgia_view (UNION ALL)         │
└──────┬──────────────────┬───────────┘
       │                  │
       │ LINK_P1          │ LINK_P2
       ▼                  ▼
┌──────────────┐   ┌──────────────────┐
│ Site P1      │   │ Site P2          │
│ DESKTOP-     │   │ LAPTOP-F37K83BK\ │
│ SEERKGC\     │   │ SQLEXPRESS       │
│ ANHTHU       │   │                  │
├──────────────┤   ├──────────────────┤
│ QLDeAn_P1    │   │ QLDeAn_P2        │
│ - NC01, NC02 │   │ - NC03, NC04     │
└──────────────┘   └──────────────────┘
```

---

## ✨ TÍNH NĂNG CHÍNH

### 🎯 Mức 1: Trong suốt Phân mảnh
- Người dùng truy vấn View mà không cần biết dữ liệu ở Site nào
- SQL đơn giản: `SELECT * FROM nhomnc_view`

### 🎯 Mức 2: Trong suốt Vị trí
- Minh họa rõ ràng vị trí dữ liệu
- SQL phức tạp: `SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhomnc UNION ALL ...`

### 🎯 Cập nhật Phân tán
- Tự động xác định Site chứa dữ liệu
- Thực thi UPDATE trực tiếp trên Site đúng

---

🎉 **Hệ thống đã sẵn sàng sử dụng!**
