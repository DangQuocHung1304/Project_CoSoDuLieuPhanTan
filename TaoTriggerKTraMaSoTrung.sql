USE QLDeAn;
GO

CREATE OR ALTER TRIGGER trg_CheckDuplicateNV_GLOBAL
ON dbo.nhanvien
FOR INSERT
AS
BEGIN
    -- Khai báo các biến cục bộ để lưu dữ liệu đang chèn
    DECLARE @manv CHAR(6); 
    DECLARE @manhom CHAR(5);
    DECLARE @tenphong CHAR(2);

    SELECT @manv = manv, @manhom = manhom FROM inserted; 
    
    -- Lấy tên phòng từ bảng nhomnc toàn cục (Dữ liệu phải có)
    SELECT @tenphong = tenphong FROM nhomnc WHERE manhom = @manhom;

    -- Kiểm tra xem dữ liệu chèn có vi phạm nguyên tắc phân mảnh không?
    -- Tức là kiểm tra xem mã nhân viên này đã có sẵn trên site khác hay không.
    -- (Trong Merge Replication, việc kiểm tra này thường được xử lý bởi Agent, 
    -- nhưng Trigger này giúp bắt lỗi sớm hơn)

    -- Kiểm tra trên Site 1 (nếu Site 2 đang chèn)
    IF (@tenphong = 'P2') AND EXISTS (SELECT 1 FROM LINK_P1.QLDeAn_P1.dbo.nhanvien WHERE manv = @manv)
    BEGIN
        DECLARE @ErrorMessageP1 NVARCHAR(200) = FORMATMESSAGE(N'Lỗi: Mã NV [%s] đã tồn tại ở Site P1.', RTRIM(@manv));
        RAISERROR (@ErrorMessageP1, 16, 1);
        ROLLBACK TRANSACTION; 
    END

    -- Kiểm tra trên Site 2 (nếu Site 1 đang chèn)
    IF (@tenphong = 'P1') AND EXISTS (SELECT 1 FROM LINK_P2.QLDeAn_P2.dbo.nhanvien WHERE manv = @manv)
    BEGIN
        DECLARE @ErrorMessageP2 NVARCHAR(200) = FORMATMESSAGE(N'Lỗi: Mã NV [%s] đã tồn tại ở Site P2.', RTRIM(@manv));
        RAISERROR (@ErrorMessageP2, 16, 1);
        ROLLBACK TRANSACTION; 
    END
END
GO