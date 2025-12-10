# Ứng dụng Quản lý Đề Án Phân Tán
## CSDL Phân Tán - Kiến trúc 2 Tầng (Client-Server)

### 📋 THÔNG TIN DỰ ÁN
- **Công nghệ**: C# Windows Forms (.NET 6)
- **Kiến trúc**: 2-Tier (Client kết nối trực tiếp đến Database)
- **Database**: SQL Server (Phân tán trên 3 Site)
  - **Site Gốc**: MSI\YLC (chứa View toàn cục và Link Server)
  - **Site P1**: DESKTOP-SEERKGC\ANHTHU (QLDeAn_P1)
  - **Site P2**: LAPTOP-F37K83BK\SQLEXPRESS (QLDeAn_P2)

### 🎯 MỤC TIÊU
Chứng minh tính **Trong suốt Phân mảnh** (Mức 1) và **Trong suốt Vị trí** (Mức 2) trong CSDL Phân tán.

---

## 🔧 CÀI ĐẶT VÀ CHẠY ỨNG DỤNG

### Yêu cầu hệ thống:
- .NET 6.0 SDK trở lên
- SQL Server (đã thiết lập CSDL phân tán với Link Server)
- Visual Studio 2022 hoặc VS Code

### Cách chạy:

#### Từ Visual Studio:
1. Mở file `QLDeAnPhanTan.sln`
2. Nhấn F5 để build và chạy

#### Từ Command Line:
```powershell
cd "d:\3.CSDLPhanTan\LyThuyet\Project\QLDeAnPhanTan"
dotnet restore
dotnet build
dotnet run
```

---

## 📦 CẤU TRÚC PROJECT

```
QLDeAnPhanTan/
├── Program.cs                      # Entry point
├── DatabaseHelper.cs               # Lớp xử lý kết nối và truy vấn CSDL
├── MainForm.cs                     # Form chính
├── MainForm.Designer.cs
├── ConnectionForm.cs               # Form kết nối CSDL
├── ConnectionForm.Designer.cs
├── QLDeAnPhanTan.csproj           # File cấu hình project
└── QLDeAnPhanTan.sln              # Solution file
```

---

## 🌟 TÍNH NĂNG CHÍNH

### 1️⃣ **Tab Quản lý Dữ liệu** (Mức 1 - Trong suốt phân mảnh)
- Hiển thị dữ liệu toàn cục từ các View:
  - `nhomnc_view` - Nhóm nghiên cứu
  - `nhanvien_view` - Nhân viên
  - `dean_view` - Đề án
  - `thamgia_view` - Tham gia đề án
- **Minh chứng**: Người dùng không cần biết dữ liệu nằm ở Site nào (P1 hay P2)

### 2️⃣ **Tab Truy vấn** (So sánh Mức 1 & Mức 2)
**Câu hỏi**: Cho biết mã đề án, tên đề án của các đề án thuộc nhóm ($manhom) mà có nhân viên của nhóm nghiên cứu khác tham gia.

- **Truy vấn Mức 1** (Trong suốt phân mảnh):
  - Sử dụng View toàn cục
  - Câu lệnh SQL đơn giản: `SELECT FROM dean_view JOIN thamgia_view...`

- **Truy vấn Mức 2** (Trong suốt vị trí):
  - Sử dụng cú pháp 4 phần và UNION ALL
  - Minh họa rõ ràng vị trí dữ liệu: `LINK_P1.QLDeAn_P1.dbo.dean UNION ALL LINK_P2.QLDeAn_P2.dbo.dean`

### 3️⃣ **Tab Cập nhật** (Cập nhật phân tán)
**Câu hỏi**: Sửa tên phòng của nhóm nghiên cứu.

- Tự động xác định Site chứa nhóm (LINK_P1 hoặc LINK_P2)
- Thực thi lệnh UPDATE trên đúng Site bằng cú pháp `EXEC AT`
- **Minh chứng**: Cập nhật dữ liệu phân tán một cách minh bạch

---

## 🔑 CẤU HÌNH KẾT NỐI

### Khi chạy ứng dụng lần đầu:
1. Vào menu **Hệ thống** > **Kết nối CSDL**
2. Nhập thông tin kết nối đến **Site Gốc** (chứa View toàn cục và Link Server):
   - **Server Name**: `MSI\YLC`
   - **Username**: `sa` (hoặc tài khoản SQL Server của bạn)
   - **Password**: Mật khẩu của tài khoản sa trên Site Gốc

> ⚠️ **LƯU Ý**: Phải kết nối đến Site Gốc (MSI\YLC), KHÔNG phải Site P1 hoặc P2.

---

## 📚 CẤU TRÚC CSDL PHÂN TÁN

### Site Gốc - MSI\YLC (QLDeAn):
- Chứa View toàn cục: `nhomnc_view`, `nhanvien_view`, `dean_view`, `thamgia_view`
- Có Link Server: `LINK_P1` (đến DESKTOP-SEERKGC\ANHTHU), `LINK_P2` (đến LAPTOP-F37K83BK\SQLEXPRESS)
- **Đây là nơi ứng dụng Client kết nối**

### Site P1 - DESKTOP-SEERKGC\ANHTHU (QLDeAn_P1):
- Dữ liệu phân mảnh 1: NC01, NC02

### Site P2 - LAPTOP-F37K83BK\SQLEXPRESS (QLDeAn_P2):
- Dữ liệu phân mảnh 2: NC03, NC04

---

## 💡 DEMO DỮ LIỆU MẪU

### Nhóm nghiên cứu:
| manhom | tennhom                    | tenphong |
|--------|----------------------------|----------|
| NC01   | Nhóm AI & Học Máy          | P1       |
| NC02   | Nhóm An Toàn Thông Tin     | P1       |
| NC03   | Nhóm Phát Triển Ứng Dụng   | P2       |
| NC04   | Nhóm Dữ Liệu Lớn           | P2       |

### Truy vấn ví dụ:
- Nhập mã nhóm: `NC01`
- Kết quả: Các đề án của NC01 có nhân viên từ nhóm khác (NC02, NC03, NC04) tham gia

---

## 🎓 MINH CHỨNG KỸ THUẬT

### Mức 1: Trong suốt Phân mảnh
```csharp
string sql = @"SELECT DISTINCT DA.mada, DA.tenda 
               FROM dean_view DA 
               JOIN thamgia_view TG ON DA.mada = TG.mada 
               JOIN nhanvien_view NV ON TG.manv = NV.manv 
               WHERE DA.manhom = @manhom AND NV.manhom <> @manhom";
```
→ Người dùng không cần biết dữ liệu phân tán ở đâu

### Mức 2: Trong suốt Vị trí
```csharp
string sql = @"SELECT DISTINCT A.mada, A.tenda 
               FROM (
                   SELECT ... FROM LINK_P1.QLDeAn_P1.dbo.dean 
                   UNION ALL 
                   SELECT ... FROM LINK_P2.QLDeAn_P2.dbo.dean
               ) AS A
               JOIN (...) AS TG ON ...";
```
→ Minh họa rõ ràng dữ liệu ở Site P1 và P2

### Cập nhật Phân tán:
```csharp
string updateSql = @"EXEC ('UPDATE QLDeAn_P1.dbo.nhomnc 
                            SET tenphong = 'P2' 
                            WHERE manhom = 'NC01'') AT LINK_P1";
```
→ Cập nhật trực tiếp trên Site chứa dữ liệu

---

## 📞 HỖ TRỢ

Nếu gặp lỗi kết nối:
1. Kiểm tra SQL Server đang chạy
2. Kiểm tra tên Server chính xác
3. Kiểm tra username/password
4. Kiểm tra Link Server đã được thiết lập: `LINK_P1`, `LINK_P2`
5. Kiểm tra các View toàn cục đã được tạo

---

## 📝 GHI CHÚ QUAN TRỌNG

1. **Kết nối**: Luôn kết nối đến Site Gốc (có View và Link Server)
2. **View**: Các View toàn cục phải được tạo bằng UNION ALL
3. **Link Server**: Phải có quyền truy cập đến các Site P1 và P2
4. **Collation**: Đã xử lý vấn đề Collation bằng `COLLATE Vietnamese_CI_AS`

---

🚀 **Chúc bạn sử dụng ứng dụng thành công!**
