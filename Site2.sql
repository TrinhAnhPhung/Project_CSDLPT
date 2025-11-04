CREATE DATABASE CSDLPT_SITE2;
USE CSDLPT_SITE2;

CREATE TABLE Khoa (
    MaKhoa CHAR(10) PRIMARY KEY,
    TenKhoa NVARCHAR(100)
);
CREATE TABLE MonHoc (
    MaMH CHAR(10) PRIMARY KEY,
    TenMH NVARCHAR(100)
);
CREATE TABLE CTDaoTao (
    MaKhoa CHAR(10),
    KhoaHoc NVARCHAR(20),
    MaMH CHAR(10),
    PRIMARY KEY (MaKhoa, KhoaHoc, MaMH)
);
CREATE TABLE SinhVien (
    MaSV CHAR(10) PRIMARY KEY,
    HoTenSV NVARCHAR(100),
    MaKhoa CHAR(10),
    KhoaHoc NVARCHAR(20)
);
CREATE TABLE DangKy (
    MaSV CHAR(10),
    MaMH CHAR(10),
    DiemThi DECIMAL(4,2),
    PRIMARY KEY (MaSV, MaMH)
);


INSERT INTO Khoa VALUES ('QTKD', N'Quản trị kinh doanh');
INSERT INTO SinhVien VALUES ('SV101', N'Lê Văn C', 'QTKD', 'K2023');
INSERT INTO SinhVien VALUES ('SV102', N'Phạm Thị D', 'QTKD', 'K2023');
