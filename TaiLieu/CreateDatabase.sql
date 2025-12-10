-- 1. TẠO CƠ SỞ DỮ LIỆU
CREATE DATABASE QLDeAn;
GO

USE QLDeAn;
GO

-- 2. TẠO BẢNG QUAN HỆ TOÀN CỤC

-- 2.1. Bảng nhomnc (Nhóm Nghiên Cứu)
CREATE TABLE nhomnc (
    manhom CHAR(5) PRIMARY KEY,
    tennhom NVARCHAR(100) NOT NULL,
    tenphong CHAR(2) NOT NULL 
);

-- 2.2. Bảng nhanvien (Nhân Viên)
CREATE TABLE nhanvien (
    manv CHAR(6) PRIMARY KEY,
    hoten NVARCHAR(100) NOT NULL,
    manhom CHAR(5) FOREIGN KEY REFERENCES nhomnc(manhom)
);

-- 2.3. Bảng dean (Đề Án)
CREATE TABLE dean (
    mada CHAR(5) PRIMARY KEY,
    tenda NVARCHAR(100) NOT NULL,
    manhom CHAR(5) FOREIGN KEY REFERENCES nhomnc(manhom)
);

-- 2.4. Bảng thamgia (Tham Gia)
CREATE TABLE thamgia (
    manv CHAR(6) NOT NULL,
    mada CHAR(5) NOT NULL,
    PRIMARY KEY (manv, mada),
    FOREIGN KEY (manv) REFERENCES nhanvien(manv),
    FOREIGN KEY (mada) REFERENCES dean(mada)
);


USE QLDeAn;
GO

-- 1. Chèn dữ liệu vào nhomnc (4 dòng - P1: NC01, NC02 | P2: NC03, NC04)
INSERT INTO nhomnc (manhom, tennhom, tenphong) VALUES
('NC01', N'Nhóm AI & Học Máy', 'P1'),
('NC02', N'Nhóm An Toàn Thông Tin', 'P1'),
('NC03', N'Nhóm Phát Triển Ứng Dụng', 'P2'),
('NC04', N'Nhóm Dữ Liệu Lớn', 'P2');

-- 2. Chèn dữ liệu vào nhanvien (5 dòng - NV0001 đến NV0005)
INSERT INTO nhanvien (manv, hoten, manhom) VALUES
('NV0001', N'Đặng Quốc Hưng', 'NC01'),
('NV0002', N'Nguyễn Anh Thư', 'NC01'),
('NV0003', N'Nguyễn Ngọc Minh Thư', 'NC02'),
('NV0004', N'Phạm Đức Thành', 'NC03'),
('NV0005', N'Lê Thị Mai', 'NC04');

-- 3. Chèn dữ liệu vào dean (3 dòng - DA001 đến DA003)
INSERT INTO dean (mada, tenda, manhom) VALUES
('DA001', N'Phát hiện gian lận ngân hàng', 'NC01'),
('DA002', N'Mô hình bảo mật đa tầng', 'NC02'),
('DA003', N'Xây dựng ứng dụng di động', 'NC03');

-- 4. Chèn dữ liệu vào thamgia (5 dòng - Minh họa cả cục bộ và liên site)
INSERT INTO thamgia (manv, mada) VALUES
('NV0001', 'DA001'), -- NV0001 (P1) tham gia DA001 (P1) - Cục bộ
('NV0003', 'DA002'), -- NV0003 (P1) tham gia DA002 (P1) - Cục bộ
('NV0004', 'DA003'), -- NV0004 (P2) tham gia DA003 (P2) - Cục bộ
('NV0005', 'DA001'), -- NV0005 (P2) tham gia DA001 (P1) - Liên site
('NV0004', 'DA002'); -- NV0004 (P2) tham gia DA002 (P1) - Liên site