-- =====================================================================
-- THIẾT LẬP LINK SERVER TẠI SITE P1 (DESKTOP-SEERKGC\ANHTHU)
-- Chạy script này trên SQL Server Instance: DESKTOP-SEERKGC\ANHTHU
-- =====================================================================

-- 1. TẠO LINK SERVER ĐẾN SITE P2 (LAPTOP-F37K83BK\SQLEXPRESS)
EXEC sp_addlinkedserver 
    @server = N'LINK_P2',              -- Tên Link Server
    @srvproduct = N'', 
    @provider = N'MSOLEDBSQL',         -- Provider mới nhất
    @datasrc = N'LAPTOP-F37K83BK\SQLEXPRESS'; -- Instance của Site P2
GO

-- 2. THIẾT LẬP ÁNH XẠ BẢO MẬT (Security Mapping)
-- Ánh xạ tất cả các tài khoản cục bộ đến tài khoản sa của Site P2
EXEC sp_addlinkedsrvlogin 
    @rmtsrvname = N'LINK_P2',          -- Tên Link Server
    @useself = N'False',               -- Không dùng Windows Authentication
    @locallogin = NULL,                -- Áp dụng cho TẤT CẢ tài khoản cục bộ
    @rmtuser = N'sa',                  -- Tài khoản sa của Site P2
    @rmtpassword = N'sa123@';          -- Mật khẩu sa của Site P2
GO

PRINT '✓ Đã tạo Link Server LINK_P2 thành công tại Site P1!'
GO