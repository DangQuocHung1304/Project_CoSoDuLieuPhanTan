-- =====================================================================
-- KIỂM TRA KẾT NỐI LINK SERVER
-- Chạy script này sau khi đã thiết lập Link Server để kiểm tra
-- =====================================================================

PRINT '========================================='
PRINT 'KIỂM TRA KẾT NỐI LINK SERVER'
PRINT '========================================='
GO

-- 1. KIỂM TRA LINK SERVER ĐÃ TỒN TẠI
PRINT ''
PRINT '1. Danh sách Link Server hiện có:'
PRINT '-----------------------------------------'
SELECT 
    name AS [Link Server Name],
    provider AS [Provider],
    data_source AS [Data Source],
    is_linked AS [Is Linked]
FROM sys.servers 
WHERE is_linked = 1
GO

-- 2. KIỂM TRA KẾT NỐI ĐẾN LINK SERVER
PRINT ''
PRINT '2. Kiểm tra kết nối đến Link Server:'
PRINT '-----------------------------------------'

-- Kiểm tra LINK_P1 (nếu đang ở Site P2)
BEGIN TRY
    EXEC sp_testlinkedserver 'LINK_P1'
    PRINT '✓ LINK_P1: Kết nối thành công!'
END TRY
BEGIN CATCH
    PRINT '✗ LINK_P1: Không thể kết nối - ' + ERROR_MESSAGE()
END CATCH
GO

-- Kiểm tra LINK_P2 (nếu đang ở Site P1)
BEGIN TRY
    EXEC sp_testlinkedserver 'LINK_P2'
    PRINT '✓ LINK_P2: Kết nối thành công!'
END TRY
BEGIN CATCH
    PRINT '✗ LINK_P2: Không thể kết nối - ' + ERROR_MESSAGE()
END CATCH
GO

-- 3. KIỂM TRA TRUY VẤN DỮ LIỆU QUA LINK SERVER
PRINT ''
PRINT '3. Kiểm tra truy vấn dữ liệu:'
PRINT '-----------------------------------------'

-- Truy vấn từ LINK_P1 (nếu có)
BEGIN TRY
    PRINT 'Truy vấn từ LINK_P1.QLDeAn_P1:'
    SELECT COUNT(*) AS [Số lượng nhóm] FROM LINK_P1.QLDeAn_P1.dbo.nhomnc
    PRINT '✓ Truy vấn LINK_P1 thành công!'
END TRY
BEGIN CATCH
    PRINT '✗ Không thể truy vấn LINK_P1: ' + ERROR_MESSAGE()
END CATCH
GO

-- Truy vấn từ LINK_P2 (nếu có)
BEGIN TRY
    PRINT 'Truy vấn từ LINK_P2.QLDeAn_P2:'
    SELECT COUNT(*) AS [Số lượng nhóm] FROM LINK_P2.QLDeAn_P2.dbo.nhomnc
    PRINT '✓ Truy vấn LINK_P2 thành công!'
END TRY
BEGIN CATCH
    PRINT '✗ Không thể truy vấn LINK_P2: ' + ERROR_MESSAGE()
END CATCH
GO

PRINT ''
PRINT '========================================='
PRINT 'KẾT THÚC KIỂM TRA'
PRINT '========================================='
GO
