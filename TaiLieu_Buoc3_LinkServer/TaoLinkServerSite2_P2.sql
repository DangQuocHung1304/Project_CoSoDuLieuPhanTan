-- =====================================================================
-- THIẾT LẬP LINK SERVER TẠI SITE P2 (LAPTOP-F37K83BK\SQLEXPRESS)
-- Chạy script này trên SQL Server Instance: LAPTOP-F37K83BK\SQLEXPRESS
-- =====================================================================

-- 1. TẠO LINK SERVER ĐẾN SITE P1 (DESKTOP-SEERKGC\ANHTHU)
EXEC sp_addlinkedserver 
    @server = N'LINK_P1',              -- Tên Link Server
    @srvproduct = N'', 
    @provider = N'MSOLEDBSQL',         -- Provider mới nhất
    @datasrc = N'DESKTOP-SEERKGC\ANHTHU'; -- Instance của Site P1
GO

-- 2. THIẾT LẬP ÁNH XẠ BẢO MẬT (Security Mapping)
-- Ánh xạ tất cả các tài khoản cục bộ đến tài khoản sa của Site P1
EXEC sp_addlinkedsrvlogin 
    @rmtsrvname = N'LINK_P1',          -- Tên Link Server
    @useself = N'False',               -- Không dùng Windows Authentication
    @locallogin = NULL,                -- Áp dụng cho TẤT CẢ tài khoản cục bộ
    @rmtuser = N'sa',                  -- Tài khoản sa của Site P1
    @rmtpassword = N'123';             -- Mật khẩu sa của Site P1
GO

PRINT '✓ Đã tạo Link Server LINK_P1 thành công tại Site P2!'
GO