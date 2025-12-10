USE QLDeAn;
GO

-- KHẮC PHỤC: Chia mỗi CREATE VIEW thành một Batch riêng biệt (dùng GO)
-- Đồng thời, thay thế biến bằng chuỗi Collation cố định.

-- Xóa View nhomnc
DROP VIEW IF EXISTS nhomnc_view;
GO

-- 1. TẠO VIEW nhomnc (Batch riêng)
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

-- Xóa View nhanvien
DROP VIEW IF EXISTS nhanvien_view;
GO

-- 2. TẠO VIEW nhanvien (Batch riêng)
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

-- Xóa View dean
DROP VIEW IF EXISTS dean_view;
GO

-- 3. TẠO VIEW dean (Batch riêng)
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

-- Xóa View thamgia
DROP VIEW IF EXISTS thamgia_view;
GO

-- 4. TẠO VIEW thamgia (Batch riêng)
CREATE VIEW thamgia_view AS
SELECT manv COLLATE Vietnamese_CI_AS AS manv, 
       mada COLLATE Vietnamese_CI_AS AS mada 
FROM LINK_P1.QLDeAn_P1.dbo.thamgia
UNION ALL
SELECT manv COLLATE Vietnamese_CI_AS, 
       mada COLLATE Vietnamese_CI_AS
FROM LINK_P2.QLDeAn_P2.dbo.thamgia;
GO


-- Ví dụ truy vấn bảng đề án từ Site P1 và P2
SELECT mada, tenda, manhom 
FROM   LINK_P1.QLDeAn_P1.dbo.dean  -- <LinkServer>.<Database>.<Schema>.<Table>
UNION ALL
SELECT mada, tenda, manhom 
FROM   LINK_P2.QLDeAn_P2.dbo.dean; -- Cú pháp 4 phần