INSERT INTO LINK_P2.QLDeAn_P2.dbo.nhomnc (manhom, tennhom, tenphong)
VALUES ('NC05', N'Nhóm Dữ liệu Đám mây', 'P2');

select * from LINK_P2.QLDeAn_P2.dbo.nhomnc

UPDATE LINK_P1.QLDeAn_P1.dbo.dean
SET tenda = N'Phát hiện gian lận ngân hàng nâng cao'
WHERE mada = 'DA001';

select * from LINK_P1.QLDeAn_P1.dbo.dean


select * from LINK_P1.QLDeAn_P1.dbo.nhanvien


-- Bước 1: Xóa các lượt tham gia của NV0001 (từ bảng con)
DELETE FROM LINK_P1.QLDeAn_P1.dbo.thamgia
WHERE manv = 'NV0001';

-- Bước 2: Xóa nhân viên NV0001 (từ bảng cha)
DELETE FROM LINK_P1.QLDeAn_P1.dbo.nhanvien
WHERE manv = 'NV0001';


SELECT NV.manv, NV.hoten, NC.tennhom, NC.tenphong
FROM v_nhanvien AS NV
INNER JOIN v_nhomnc AS NC ON NV.manhom = NC.manhom;


SELECT NV.manv, NV.hoten, NC.tennhom, NC.tenphong
FROM
    -- Bảng NV (nhanvien) đã được sửa Collation
    (SELECT 
         manv COLLATE DATABASE_DEFAULT AS manv, 
         hoten COLLATE DATABASE_DEFAULT AS hoten, 
         manhom COLLATE DATABASE_DEFAULT AS manhom 
     FROM LINK_P1.QLDeAn_P1.dbo.nhanvien
     UNION ALL
     SELECT 
         manv COLLATE DATABASE_DEFAULT AS manv, 
         hoten COLLATE DATABASE_DEFAULT AS hoten, 
         manhom COLLATE DATABASE_DEFAULT AS manhom 
     FROM LINK_P2.QLDeAn_P2.dbo.nhanvien) AS NV
INNER JOIN
    -- Bảng NC (nhomnc) đã được sửa Collation
    (SELECT 
         manhom COLLATE DATABASE_DEFAULT AS manhom, 
         tennhom COLLATE DATABASE_DEFAULT AS tennhom, 
         tenphong COLLATE DATABASE_DEFAULT AS tenphong 
     FROM LINK_P1.QLDeAn_P1.dbo.nhomnc
     UNION ALL
     SELECT 
         manhom COLLATE DATABASE_DEFAULT AS manhom, 
         tennhom COLLATE DATABASE_DEFAULT AS tennhom, 
         tenphong COLLATE DATABASE_DEFAULT AS tenphong 
     FROM LINK_P2.QLDeAn_P2.dbo.nhomnc) AS NC
ON NV.manhom = NC.manhom; -- Lệnh JOIN bây giờ sẽ thành công vì collation đã giống nhau





SELECT
    NV.hoten AS N'Tên Nhân Viên (P2)',
    DA.tenda AS N'Tên Đề Án (P1)'
FROM
    v_nhanvien AS NV
INNER JOIN
    v_thamgia AS TG ON NV.manv = TG.manv
INNER JOIN
    v_dean AS DA ON TG.mada = DA.mada
INNER JOIN
    v_nhomnc AS NV_NC ON NV.manhom = NV_NC.manhom -- Join để lấy phòng của NV
INNER JOIN
    v_nhomnc AS DA_NC ON DA.manhom = DA_NC.manhom -- Join để lấy phòng của Đề Án
WHERE
    NV_NC.tenphong = 'P2' AND DA_NC.tenphong = 'P1';






	SELECT
    NV.hoten AS N'Tên Nhân Viên (P2)',
    DA.tenda AS N'Tên Đề Án (P1)'
FROM
    LINK_P2.QLDeAn_P2.dbo.nhanvien AS NV
INNER JOIN
    LINK_P2.QLDeAn_P2.dbo.thamgia AS TG ON NV.manv = TG.manv
INNER JOIN
    LINK_P1.QLDeAn_P1.dbo.dean AS DA ON TG.mada = DA.mada;

-- Lưu ý: Lệnh này ngầm giả định rằng bảng thamgia tại P2 có thể
-- chứa các mã đề án (mada) không thuộc P2,
-- điều này phù hợp với dữ liệu mẫu (Hình 19).



SELECT
    NV.hoten AS N'Tên Nhân Viên (P2)',
    DA.tenda AS N'Tên Đề Án (P1)'
FROM
    LINK_P2.QLDeAn_P2.dbo.nhanvien AS NV
INNER JOIN
    LINK_P2.QLDeAn_P2.dbo.thamgia AS TG ON NV.manv = TG.manv
INNER JOIN
    LINK_P1.QLDeAn_P1.dbo.dean AS DA 
    -- SỬA LỖI TẠI ĐÂY: Thêm COLLATE DATABASE_DEFAULT vào vế so sánh
    ON TG.mada COLLATE DATABASE_DEFAULT = DA.mada COLLATE DATABASE_DEFAULT;