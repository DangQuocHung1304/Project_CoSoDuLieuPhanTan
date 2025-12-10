-- =====================================================================
-- TẠO CSDL PHÂN MẢNH - SITE P2 (LAPTOP-F37K83BK\SQLEXPRESS)
-- Chạy script này trên Instance: LAPTOP-F37K83BK\SQLEXPRESS
-- =====================================================================

-- 1. TẠO CƠ SỞ DỮ LIỆU
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QLDeAn_P2')
BEGIN
    CREATE DATABASE QLDeAn_P2;
    PRINT '✓ Đã tạo database QLDeAn_P2'
END
ELSE
BEGIN
    PRINT '⚠ Database QLDeAn_P2 đã tồn tại'
END
GO

USE QLDeAn_P2;
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

-- 3. CHÈN DỮ LIỆU PHÂN MẢNH P2 (NC03, NC04)

-- 3.1. Chèn nhóm nghiên cứu P2
IF NOT EXISTS (SELECT * FROM nhomnc WHERE manhom = 'NC03')
BEGIN
    INSERT INTO nhomnc (manhom, tennhom, tenphong) VALUES
    ('NC03', N'Nhóm Phát Triển Ứng Dụng', 'P2'),
    ('NC04', N'Nhóm Dữ Liệu Lớn', 'P2');
    PRINT '✓ Đã chèn dữ liệu bảng nhomnc (P2)'
END
GO

-- 3.2. Chèn nhân viên P2
IF NOT EXISTS (SELECT * FROM nhanvien WHERE manv = 'NV0004')
BEGIN
    INSERT INTO nhanvien (manv, hoten, manhom) VALUES
    ('NV0004', N'Phạm Đức Thành', 'NC03'),
    ('NV0005', N'Lê Thị Mai', 'NC04');
    PRINT '✓ Đã chèn dữ liệu bảng nhanvien (P2)'
END
GO

-- 3.3. Chèn đề án P2
IF NOT EXISTS (SELECT * FROM dean WHERE mada = 'DA003')
BEGIN
    INSERT INTO dean (mada, tenda, manhom) VALUES
    ('DA003', N'Xây dựng ứng dụng di động', 'NC03');
    PRINT '✓ Đã chèn dữ liệu bảng dean (P2)'
END
GO

-- 3.4. Chèn tham gia P2 và liên site
IF NOT EXISTS (SELECT * FROM thamgia WHERE manv = 'NV0004' AND mada = 'DA003')
BEGIN
    INSERT INTO thamgia (manv, mada) VALUES
    ('NV0004', 'DA003'); -- NV0004 (P2) tham gia DA003 (P2) - Cục bộ
    PRINT '✓ Đã chèn dữ liệu bảng thamgia (P2)'
END
GO

-- Lưu ý: Dữ liệu liên site (NV0005 tham gia DA001, NV0004 tham gia DA002) 
-- sẽ được chèn sau khi thiết lập Link Server

PRINT ''
PRINT '========================================='
PRINT '✓ HOÀN THÀNH THIẾT LẬP SITE P2'
PRINT '========================================='
GO
