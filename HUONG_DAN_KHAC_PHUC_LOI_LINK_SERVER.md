# 🔧 HƯỚNG DẪN KHẮC PHỤC LỖI: "Invalid object name LINK_P2"

## ❌ Vấn đề
Khi kết nối vào **Site P1 (DESKTOP-SEERKGC\ANHTHU)** và cố gắng xem dữ liệu từ Site P2, gặp lỗi:
```
Invalid object name 'QLDeAn_P2.dbo.nhomnc'
```

**Nguyên nhân**: Site P1 chưa có Link Server `LINK_P2` để kết nối sang Site P2.

---

## ✅ GIẢI PHÁP

### Bước 1: Tạo Link Server trên Site P1
1. **Mở SQL Server Management Studio (SSMS)**
2. **Kết nối đến**: `DESKTOP-SEERKGC\ANHTHU` (Site P1)
3. **Mở file**: `TaiLieu_Buoc3_LinkServer\TaoLinkServerSite1_P1.sql`
4. **Chạy script** (Execute hoặc F5)

**Script sẽ thực hiện**:
```sql
-- Tạo Link Server LINK_P2 trỏ đến LAPTOP-F37K83BK\SQLEXPRESS
EXEC sp_addlinkedserver 
    @server = N'LINK_P2',
    @srvproduct = N'', 
    @provider = N'MSOLEDBSQL',
    @datasrc = N'LAPTOP-F37K83BK\SQLEXPRESS';

-- Cấu hình xác thực
EXEC sp_addlinkedsrvlogin 
    @rmtsrvname = N'LINK_P2',
    @useself = N'False',
    @locallogin = NULL,
    @rmtuser = N'sa',
    @rmtpassword = N'sa123@';
```

### Bước 2: Kiểm tra Link Server
Chạy lệnh sau trên Site P1 để kiểm tra:
```sql
-- Kiểm tra Link Server đã tạo
SELECT * FROM sys.servers WHERE name = 'LINK_P2';

-- Test kết nối
SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhomnc;
```

**Kết quả mong đợi**: Thấy danh sách nhóm nghiên cứu của Site P2 (NC03, NC04...)

---

### Bước 3: Tạo Link Server trên Site P2 (để P2 xem P1)
1. **Kết nối đến**: `LAPTOP-F37K83BK\SQLEXPRESS` (Site P2)
2. **Mở file**: `TaiLieu_Buoc3_LinkServer\TaoLinkServerSite2_P2.sql`
3. **Chạy script**

**Script sẽ tạo Link Server `LINK_P1`** trỏ về Site P1.

---

## 🎯 KẾT QUẢ SAU KHI CẤU HÌNH

### ✅ Từ Site P1 (QLDeAn_P1):
- ✅ Xem dữ liệu Site P2: `SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhomnc`
- ✅ Sửa dữ liệu Site P2: `UPDATE LINK_P2.QLDeAn_P2.dbo.nhomnc SET ...`
- ✅ Xóa dữ liệu Site P2: `DELETE FROM LINK_P2.QLDeAn_P2.dbo.nhomnc WHERE ...`

### ✅ Từ Site P2 (QLDeAn_P2):
- ✅ Xem dữ liệu Site P1: `SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhomnc`
- ✅ Sửa dữ liệu Site P1: `UPDATE LINK_P1.QLDeAn_P1.dbo.nhomnc SET ...`
- ✅ Xóa dữ liệu Site P1: `DELETE FROM LINK_P1.QLDeAn_P1.dbo.nhomnc WHERE ...`

### ✅ Từ Site Gốc (QLDeAn):
- ✅ Xem dữ liệu P1: `SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhomnc`
- ✅ Xem dữ liệu P2: `SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhomnc`

---

## 📝 LƯU Ý QUAN TRỌNG

### 1. Yêu cầu về mạng
- ✅ Hai máy P1 và P2 phải **kết nối mạng** với nhau
- ✅ **SQL Server Browser** phải đang chạy trên cả 2 máy
- ✅ **Firewall** cho phép kết nối SQL Server (port 1433)

### 2. Kiểm tra SQL Server Browser
```powershell
# Kiểm tra SQL Server Browser đang chạy
Get-Service -Name "SQLBrowser"

# Khởi động nếu chưa chạy
Start-Service -Name "SQLBrowser"
```

### 3. Kiểm tra kết nối mạng giữa 2 máy
```powershell
# Từ P1 ping P2
ping LAPTOP-F37K83BK

# Test kết nối SQL Server
Test-NetConnection -ComputerName LAPTOP-F37K83BK -Port 1433
```

### 4. Nếu gặp lỗi "Could not find server"
- **Nguyên nhân**: Firewall chặn hoặc SQL Server Browser không chạy
- **Giải pháp**:
  1. Bật SQL Server Browser
  2. Thêm rule Firewall cho SQL Server
  3. Enable TCP/IP protocol trong SQL Server Configuration Manager

---

## 🔍 TROUBLESHOOTING

### Lỗi: "Login failed for user 'sa'"
**Giải pháp**: Kiểm tra mật khẩu trong script:
- Site P2 dùng mật khẩu: `sa123@`
- Sửa trong file `TaoLinkServerSite1_P1.sql` nếu mật khẩu khác

### Lỗi: "The OLE DB provider 'MSOLEDBSQL' has not been registered"
**Giải pháp**: Cài đặt **Microsoft OLE DB Driver for SQL Server**
- Download: https://aka.ms/downloadmsoledbs
- Hoặc đổi provider thành `SQLNCLI` trong script

### Lỗi: "Cannot initialize the data source object of OLE DB provider"
**Giải pháp**: Kiểm tra:
1. Tên server đúng chưa (LAPTOP-F37K83BK\SQLEXPRESS)
2. SQL Server có bật Remote Connections chưa
3. Firewall có chặn không

---

## 📞 HỖ TRỢ THÊM

Nếu vẫn gặp lỗi sau khi làm theo hướng dẫn, kiểm tra:
1. ✅ File log: `TaiLieu_Buoc3_LinkServer\KiemTraKetNoi.sql`
2. ✅ Test Link Server: `TaiLieu_Buoc3_LinkServer\KiemTraLinkServer.sql`
3. ✅ Xem cấu hình chi tiết: `HUONG_DAN_THIET_LAP.md`

---

✅ **Sau khi cấu hình xong, chạy lại ứng dụng và thử chức năng "Xem Site khác"!**
