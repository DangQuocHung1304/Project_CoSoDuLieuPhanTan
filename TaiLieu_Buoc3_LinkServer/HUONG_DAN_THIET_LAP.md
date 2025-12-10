# 🔗 HƯỚNG DẪN THIẾT LẬP HỆ THỐNG CSDL PHÂN TÁN

## 📋 THÔNG TIN CẤU HÌNH

### Site P1 - Phân mảnh 1
- **Instance**: `DESKTOP-SEERKGC\ANHTHU`
- **Database**: `QLDeAn_P1`
- **Tài khoản**: `sa`
- **Mật khẩu**: `123`
- **Link Server đến P2**: `LINK_P2`
- **Dữ liệu**: NC01, NC02 (Nhóm nghiên cứu ở P1)

### Site P2 - Phân mảnh 2
- **Instance**: `LAPTOP-F37K83BK\SQLEXPRESS`
- **Database**: `QLDeAn_P2`
- **Tài khoản**: `sa`
- **Mật khẩu**: `sa123@`
- **Link Server đến P1**: `LINK_P1`
- **Dữ liệu**: NC03, NC04 (Nhóm nghiên cứu ở P2)

---

## 🚀 CÁC BƯỚC THIẾT LẬP

### **Bước 1: Tạo CSDL trên mỗi Site**

#### 1.1. Tạo CSDL tại Site P1
1. Kết nối đến SQL Server: `DESKTOP-SEERKGC\ANHTHU`
2. Chạy file: `TaoCSDL_Site_P1.sql`
3. Kiểm tra database `QLDeAn_P1` đã được tạo với đầy đủ bảng và dữ liệu P1

#### 1.2. Tạo CSDL tại Site P2
1. Kết nối đến SQL Server: `LAPTOP-F37K83BK\SQLEXPRESS`
2. Chạy file: `TaoCSDL_Site_P2.sql`
3. Kiểm tra database `QLDeAn_P2` đã được tạo với đầy đủ bảng và dữ liệu P2

---

### **Bước 2: Thiết lập Link Server**

#### 2.1. Thiết lập Link Server tại Site P1
1. Kết nối đến SQL Server: `DESKTOP-SEERKGC\ANHTHU`
2. Chạy file: `TaoLinkServerSite1_P1.sql`
3. Link Server `LINK_P2` sẽ được tạo, trỏ đến `LAPTOP-F37K83BK\SQLEXPRESS`

#### 2.2. Thiết lập Link Server tại Site P2
1. Kết nối đến SQL Server: `LAPTOP-F37K83BK\SQLEXPRESS`
2. Chạy file: `TaoLinkServerSite2_P2.sql`
3. Link Server `LINK_P1` sẽ được tạo, trỏ đến `DESKTOP-SEERKGC\ANHTHU`

---

### **Bước 3: Kiểm tra kết nối Link Server**

#### 3.1. Kiểm tra tại Site P1
1. Kết nối đến: `DESKTOP-SEERKGC\ANHTHU`
2. Chạy file: `KiemTraKetNoiLinkServer.sql`
3. Xác nhận `LINK_P2` kết nối thành công

#### 3.2. Kiểm tra tại Site P2
1. Kết nối đến: `LAPTOP-F37K83BK\SQLEXPRESS`
2. Chạy file: `KiemTraKetNoiLinkServer.sql`
3. Xác nhận `LINK_P1` kết nối thành công

---

### **Bước 4: Chèn dữ liệu liên Site**

#### 4.1. Chèn dữ liệu liên site
1. Sau khi Link Server hoạt động, chạy file: `ChenDuLieuLienSite.sql`
2. Dữ liệu này minh họa nhân viên từ Site này tham gia đề án ở Site kia

---

### **Bước 5: Tạo View toàn cục tại Site Gốc**

#### 5.1. Chọn Site Gốc
- Chọn một trong hai Site làm Site Gốc (khuyến nghị: Site P1)
- Site Gốc sẽ chứa các View toàn cục và ứng dụng Client sẽ kết nối vào đây

#### 5.2. Tạo View toàn cục
1. Kết nối đến Site Gốc (ví dụ: `DESKTOP-SEERKGC\ANHTHU`)
2. Chạy file: `../TaiLieu_Buoc4_ThietLapTinhTrongSuotPhanManh/1.TaoViewToanCuc(UNION_ALL).sql`
3. Các View sau sẽ được tạo:
   - `nhomnc_view`
   - `nhanvien_view`
   - `dean_view`
   - `thamgia_view`

---

## 🔍 KIỂM TRA HỆ THỐNG

### Kiểm tra Link Server
```sql
-- Chạy tại mỗi Site
SELECT * FROM sys.servers WHERE is_linked = 1
```

### Kiểm tra View toàn cục (tại Site Gốc)
```sql
-- Kiểm tra dữ liệu toàn bộ hệ thống
SELECT * FROM nhomnc_view    -- Phải có 4 nhóm (NC01-NC04)
SELECT * FROM nhanvien_view  -- Phải có 5 nhân viên
SELECT * FROM dean_view      -- Phải có 3 đề án
SELECT * FROM thamgia_view   -- Phải có 5 bản ghi
```

### Kiểm tra truy vấn phân tán
```sql
-- Tại Site Gốc, kiểm tra truy vấn qua Link Server
SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhomnc
SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhomnc
```

---

## ⚠️ XỬ LÝ LỖI THƯỜNG GẶP

### Lỗi 1: "Login failed for user 'sa'"
**Nguyên nhân**: Sai mật khẩu hoặc tài khoản sa bị khóa

**Giải pháp**:
```sql
-- Kiểm tra tài khoản sa
USE master;
GO
ALTER LOGIN sa ENABLE;
GO
ALTER LOGIN sa WITH PASSWORD = 'your_password';
GO
```

### Lỗi 2: "The OLE DB provider ... has not been registered"
**Nguyên nhân**: Provider MSOLEDBSQL chưa cài đặt

**Giải pháp**:
- Download và cài đặt **Microsoft OLE DB Driver for SQL Server** từ trang chủ Microsoft
- Link: https://aka.ms/downloadmsoledbsql

### Lỗi 3: "Access to the remote server is denied"
**Nguyên nhân**: Cấu hình Security Mapping chưa đúng

**Giải pháp**:
```sql
-- Xóa và tạo lại Security Mapping
EXEC sp_droplinkedsrvlogin 'LINK_P2', NULL
GO
EXEC sp_addlinkedsrvlogin 
    @rmtsrvname = 'LINK_P2',
    @useself = 'False',
    @locallogin = NULL,
    @rmtuser = 'sa',
    @rmtpassword = 'sa123@'
GO
```

### Lỗi 4: Collation conflict
**Nguyên nhân**: Các Site có Collation khác nhau

**Giải pháp**: Đã xử lý trong View bằng cách sử dụng `COLLATE Vietnamese_CI_AS`

---

## 📂 DANH SÁCH FILE QUAN TRỌNG

| File | Mô tả | Chạy tại |
|------|-------|----------|
| `TaoCSDL_Site_P1.sql` | Tạo CSDL và dữ liệu Site P1 | P1 |
| `TaoCSDL_Site_P2.sql` | Tạo CSDL và dữ liệu Site P2 | P2 |
| `TaoLinkServerSite1_P1.sql` | Tạo LINK_P2 tại P1 | P1 |
| `TaoLinkServerSite2_P2.sql` | Tạo LINK_P1 tại P2 | P2 |
| `KiemTraKetNoiLinkServer.sql` | Kiểm tra Link Server | P1 & P2 |
| `ChenDuLieuLienSite.sql` | Chèn dữ liệu liên Site | P1 |
| `XoaLinkServer.sql` | Xóa Link Server (nếu cần reset) | P1 & P2 |

---

## 🎯 BƯỚC TIẾP THEO

Sau khi hoàn thành các bước trên:

1. ✅ Chạy ứng dụng C# trong thư mục `QLDeAnPhanTan`
2. ✅ Kết nối đến Site Gốc trong ứng dụng
3. ✅ Thử nghiệm các tính năng:
   - Quản lý dữ liệu (View toàn cục)
   - Truy vấn Mức 1 & Mức 2
   - Cập nhật phân tán

---

## 📞 LƯU Ý QUAN TRỌNG

1. **Thứ tự thực hiện**: Phải tuân thủ đúng thứ tự các bước
2. **Firewall**: Đảm bảo 2 máy chủ có thể kết nối với nhau qua mạng
3. **SQL Server Port**: Mặc định là 1433, đảm bảo port không bị chặn
4. **Named Instance**: Khi dùng Named Instance (như `\SQLEXPRESS`), phải bật SQL Server Browser Service
5. **Backup**: Nên backup dữ liệu trước khi thực hiện các thao tác quan trọng

---

🎉 **Chúc bạn thiết lập thành công hệ thống CSDL Phân tán!**
