-- =====================================================================
-- CHÈN DỮ LIỆU LIÊN SITE (Cross-Site Data)
-- Chạy các lệnh này SAU KHI đã thiết lập Link Server giữa P1 và P2
-- =====================================================================

PRINT '========================================='
PRINT 'CHÈN DỮ LIỆU LIÊN SITE'
PRINT '========================================='
GO

-- =====================================================================
-- CHẠY TẠI SITE P1 (DESKTOP-SEERKGC\ANHTHU)
-- =====================================================================

USE QLDeAn_P1;
GO

PRINT ''
PRINT 'Chèn dữ liệu liên site tại P1...'

-- Nhân viên P2 (NV0005) tham gia đề án P1 (DA001)
IF NOT EXISTS (SELECT * FROM thamgia WHERE manv = 'NV0005' AND mada = 'DA001')
BEGIN
    INSERT INTO thamgia (manv, mada) VALUES
    ('NV0005', 'DA001'); -- NV0005 (P2) tham gia DA001 (P1) - Liên site
    PRINT '✓ Đã chèn: NV0005 (P2) tham gia DA001 (P1)'
END
ELSE
    PRINT '⚠ Dữ liệu đã tồn tại: NV0005-DA001'
GO

-- Nhân viên P2 (NV0004) tham gia đề án P1 (DA002)
IF NOT EXISTS (SELECT * FROM thamgia WHERE manv = 'NV0004' AND mada = 'DA002')
BEGIN
    INSERT INTO thamgia (manv, mada) VALUES
    ('NV0004', 'DA002'); -- NV0004 (P2) tham gia DA002 (P1) - Liên site
    PRINT '✓ Đã chèn: NV0004 (P2) tham gia DA002 (P1)'
END
ELSE
    PRINT '⚠ Dữ liệu đã tồn tại: NV0004-DA002'
GO

PRINT ''
PRINT '✓ Hoàn thành chèn dữ liệu liên site tại P1'
GO

-- =====================================================================
-- SAU ĐÓ CHẠY TẠI SITE P2 (LAPTOP-F37K83BK\SQLEXPRESS)
-- =====================================================================
-- Mở kết nối mới đến LAPTOP-F37K83BK\SQLEXPRESS và chạy các lệnh sau:

/*
USE QLDeAn_P2;
GO

PRINT ''
PRINT 'Chèn dữ liệu liên site tại P2...'

-- Có thể thêm dữ liệu liên site khác nếu cần
-- Ví dụ: Nhân viên P1 tham gia đề án P2

PRINT ''
PRINT '✓ Hoàn thành chèn dữ liệu liên site tại P2'
GO
*/

PRINT ''
PRINT '========================================='
PRINT '✓ HOÀN TẤT CHÈN DỮ LIỆU LIÊN SITE'
PRINT '========================================='
GO
