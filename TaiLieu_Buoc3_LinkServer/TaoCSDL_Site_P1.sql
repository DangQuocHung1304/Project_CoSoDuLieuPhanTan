-- =====================================================================
-- TẠO CSDL PHÂN MẢNH - SITE P1 (DESKTOP-SEERKGC\ANHTHU)
-- Chạy script này trên Instance: DESKTOP-SEERKGC\ANHTHU
-- =====================================================================

-- 1. TẠO CƠ SỞ DỮ LIỆU
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QLDeAn_P1')
BEGIN
    CREATE DATABASE QLDeAn_P1;
    PRINT '✓ Đã tạo database QLDeAn_P1'
END
ELSE
BEGIN
    PRINT '⚠ Database QLDeAn_P1 đã tồn tại'
END
GO

USE QLDeAn_P1;
GO

-- 2. TẠO CÁC BẢNG

-- 2.1. Bảng nhomnc (Nhóm Nghiên Cứu)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'nhomnc')
BEGIN
    CREATE TABLE nhomnc (
        manhom CHAR(5) PRIMARY KEY,
        tennhom NVARCHAR(100) NOT NULL,
        tenphong CHAR(2) NOT NULL 
    );
    PRINT '✓ Đã tạo bảng nhomnc'
END
GO

-- 2.2. Bảng nhanvien (Nhân Viên)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'nhanvien')
BEGIN
    CREATE TABLE nhanvien (
        manv CHAR(6) PRIMARY KEY,
        hoten NVARCHAR(100) NOT NULL,
        manhom CHAR(5) FOREIGN KEY REFERENCES nhomnc(manhom)
    );
    PRINT '✓ Đã tạo bảng nhanvien'
END
GO

-- 2.3. Bảng dean (Đề Án)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'dean')
BEGIN
    CREATE TABLE dean (
        mada CHAR(5) PRIMARY KEY,
        tenda NVARCHAR(100) NOT NULL,
        manhom CHAR(5) FOREIGN KEY REFERENCES nhomnc(manhom)
    );
    PRINT '✓ Đã tạo bảng dean'
END
GO

-- 2.4. Bảng thamgia (Tham Gia)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'thamgia')
BEGIN
    CREATE TABLE thamgia (
        manv CHAR(6) NOT NULL,
        mada CHAR(5) NOT NULL,
        PRIMARY KEY (manv, mada),
        FOREIGN KEY (manv) REFERENCES nhanvien(manv),
        FOREIGN KEY (mada) REFERENCES dean(mada)
    );
    PRINT '✓ Đã tạo bảng thamgia'
END
GO

-- 3. CHÈN DỮ LIỆU PHÂN MẢNH P1 (NC01, NC02)

-- 3.1. Chèn nhóm nghiên cứu P1
IF NOT EXISTS (SELECT * FROM nhomnc WHERE manhom = 'NC01')
BEGIN
    INSERT INTO nhomnc (manhom, tennhom, tenphong) VALUES
    ('NC01', N'Nhóm AI & Học Máy', 'P1'),
    ('NC02', N'Nhóm An Toàn Thông Tin', 'P1');
    PRINT '✓ Đã chèn dữ liệu bảng nhomnc (P1)'
END
GO

-- 3.2. Chèn nhân viên P1
IF NOT EXISTS (SELECT * FROM nhanvien WHERE manv = 'NV0001')
BEGIN
    INSERT INTO nhanvien (manv, hoten, manhom) VALUES
    ('NV0001', N'Đặng Quốc Hưng', 'NC01'),
    ('NV0002', N'Nguyễn Anh Thư', 'NC01'),
    ('NV0003', N'Nguyễn Ngọc Minh Thư', 'NC02');
    PRINT '✓ Đã chèn dữ liệu bảng nhanvien (P1)'
END
GO

-- 3.3. Chèn đề án P1
IF NOT EXISTS (SELECT * FROM dean WHERE mada = 'DA001')
BEGIN
    INSERT INTO dean (mada, tenda, manhom) VALUES
    ('DA001', N'Phát hiện gian lận ngân hàng', 'NC01'),
    ('DA002', N'Mô hình bảo mật đa tầng', 'NC02');
    PRINT '✓ Đã chèn dữ liệu bảng dean (P1)'
END
GO

-- 3.4. Chèn tham gia P1 và liên site
IF NOT EXISTS (SELECT * FROM thamgia WHERE manv = 'NV0001' AND mada = 'DA001')
BEGIN
    INSERT INTO thamgia (manv, mada) VALUES
    ('NV0001', 'DA001'), -- NV0001 (P1) tham gia DA001 (P1) - Cục bộ
    ('NV0003', 'DA002'); -- NV0003 (P1) tham gia DA002 (P1) - Cục bộ
    PRINT '✓ Đã chèn dữ liệu bảng thamgia (P1)'
END
GO

PRINT ''
PRINT '========================================='
PRINT '✓ HOÀN THÀNH THIẾT LẬP SITE P1'
PRINT '========================================='
GO
