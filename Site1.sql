CREATE DATABASE CSDLPT_SITE1;
USE CSDLPT_SITE1;

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

-- Dữ liệu máy 1
INSERT INTO Khoa VALUES ('CNTT', N'Công nghệ thông tin');
INSERT INTO SinhVien VALUES ('SV001', N'Nguyễn Văn A', 'CNTT', 'K2023');
INSERT INTO SinhVien VALUES ('SV002', N'Trần Thị B', 'CNTT', 'K2023');





use CSDLPT_SITE1;
go
CREATE LOGIN csdlptsite WITH PASSWORD = 'csdlptsite123@';
go
CREATE USER csdlptsite FOR LOGIN csdlptsite;
go
--DROP USER democsdlpt;
GRANT select,insert,update,delete TO csdlptsite;
GO


-- Thêm khoa
CREATE PROCEDURE sp_AddKhoa
    @MaKhoa CHAR(10),
    @TenKhoa NVARCHAR(100)
AS
BEGIN
    INSERT INTO Khoa (MaKhoa, TenKhoa)
    VALUES (@MaKhoa, @TenKhoa);
END
GO

-- Cập nhật khoa
CREATE PROCEDURE sp_UpdateKhoa
    @MaKhoa CHAR(10),
    @TenKhoa NVARCHAR(100)
AS
BEGIN
    UPDATE Khoa
    SET TenKhoa = @TenKhoa
    WHERE MaKhoa = @MaKhoa;
END
GO

-- Xóa khoa
CREATE PROCEDURE sp_DeleteKhoa
    @MaKhoa CHAR(10)
AS
BEGIN
    DELETE FROM Khoa WHERE MaKhoa = @MaKhoa;
END
GO

-- Lấy toàn bộ khoa
CREATE PROCEDURE sp_GetAllKhoa
AS
BEGIN
    SELECT * FROM Khoa;
END
GO

-- Tìm kiếm khoa theo mã
CREATE PROCEDURE sp_GetKhoaByID
    @MaKhoa CHAR(10)
AS
BEGIN
    SELECT * FROM Khoa WHERE MaKhoa = @MaKhoa;
END
GO
-- Thêm môn học
CREATE PROCEDURE sp_AddMonHoc
    @MaMH CHAR(10),
    @TenMH NVARCHAR(100)
AS
BEGIN
    INSERT INTO MonHoc (MaMH, TenMH)
    VALUES (@MaMH, @TenMH);
END
GO

-- Cập nhật môn học
CREATE PROCEDURE sp_UpdateMonHoc
    @MaMH CHAR(10),
    @TenMH NVARCHAR(100)
AS
BEGIN
    UPDATE MonHoc
    SET TenMH = @TenMH
    WHERE MaMH = @MaMH;
END
GO

-- Xóa môn học
CREATE PROCEDURE sp_DeleteMonHoc
    @MaMH CHAR(10)
AS
BEGIN
    DELETE FROM MonHoc WHERE MaMH = @MaMH;
END
GO

-- Lấy danh sách môn học
CREATE PROCEDURE sp_GetAllMonHoc
AS
BEGIN
    SELECT * FROM MonHoc;
END
GO

-- Lấy môn học theo mã
CREATE PROCEDURE sp_GetMonHocByID
    @MaMH CHAR(10)
AS
BEGIN
    SELECT * FROM MonHoc WHERE MaMH = @MaMH;
END
GO
-- Thêm CT đào tạo
CREATE PROCEDURE sp_AddCTDaoTao
    @MaKhoa CHAR(10),
    @KhoaHoc NVARCHAR(20),
    @MaMH CHAR(10)
AS
BEGIN
    INSERT INTO CTDaoTao (MaKhoa, KhoaHoc, MaMH)
    VALUES (@MaKhoa, @KhoaHoc, @MaMH);
END
GO

-- Cập nhật CT đào tạo (chỉ cho phép cập nhật mã môn)
CREATE PROCEDURE sp_UpdateCTDaoTao
    @MaKhoa CHAR(10),
    @KhoaHoc NVARCHAR(20),
    @MaMH CHAR(10),
    @NewMaMH CHAR(10)
AS
BEGIN
    UPDATE CTDaoTao
    SET MaMH = @NewMaMH
    WHERE MaKhoa = @MaKhoa AND KhoaHoc = @KhoaHoc AND MaMH = @MaMH;
END
GO

-- Xóa CT đào tạo
CREATE PROCEDURE sp_DeleteCTDaoTao
    @MaKhoa CHAR(10),
    @KhoaHoc NVARCHAR(20),
    @MaMH CHAR(10)
AS
BEGIN
    DELETE FROM CTDaoTao
    WHERE MaKhoa = @MaKhoa AND KhoaHoc = @KhoaHoc AND MaMH = @MaMH;
END
GO

-- Lấy toàn bộ CT đào tạo
CREATE PROCEDURE sp_GetAllCTDaoTao
AS
BEGIN
    SELECT * FROM CTDaoTao;
END
GO

-- Lấy CT đào tạo theo khoa
CREATE PROCEDURE sp_GetCTDaoTaoByKhoa
    @MaKhoa CHAR(10)
AS
BEGIN
    SELECT * FROM CTDaoTao WHERE MaKhoa = @MaKhoa;
END
GO
-- Thêm sinh viên
CREATE PROCEDURE sp_AddSinhVien
    @MaSV CHAR(10),
    @HoTenSV NVARCHAR(100),
    @MaKhoa CHAR(10),
    @KhoaHoc NVARCHAR(20)
AS
BEGIN
    INSERT INTO SinhVien (MaSV, HoTenSV, MaKhoa, KhoaHoc)
    VALUES (@MaSV, @HoTenSV, @MaKhoa, @KhoaHoc);
END
GO

-- Cập nhật sinh viên
CREATE PROCEDURE sp_UpdateSinhVien
    @MaSV CHAR(10),
    @HoTenSV NVARCHAR(100),
    @MaKhoa CHAR(10),
    @KhoaHoc NVARCHAR(20)
AS
BEGIN
    UPDATE SinhVien
    SET HoTenSV = @HoTenSV,
        MaKhoa = @MaKhoa,
        KhoaHoc = @KhoaHoc
    WHERE MaSV = @MaSV;
END
GO

-- Xóa sinh viên
CREATE PROCEDURE sp_DeleteSinhVien
    @MaSV CHAR(10)
AS
BEGIN
    DELETE FROM SinhVien WHERE MaSV = @MaSV;
END
GO

-- Lấy toàn bộ sinh viên
CREATE PROCEDURE sp_GetAllSinhVien
AS
BEGIN
    SELECT * FROM SinhVien;
END
GO

-- Lấy sinh viên theo mã
CREATE PROCEDURE sp_GetSinhVienByID
    @MaSV CHAR(10)
AS
BEGIN
    SELECT * FROM SinhVien WHERE MaSV = @MaSV;
END
GO
-- Thêm đăng ký
CREATE PROCEDURE sp_AddDangKy
    @MaSV CHAR(10),
    @MaMH CHAR(10),
    @DiemThi DECIMAL(4,2)
AS
BEGIN
    INSERT INTO DangKy (MaSV, MaMH, DiemThi)
    VALUES (@MaSV, @MaMH, @DiemThi);
END
GO

-- Cập nhật điểm thi
CREATE PROCEDURE sp_UpdateDangKy
    @MaSV CHAR(10),
    @MaMH CHAR(10),
    @DiemThi DECIMAL(4,2)
AS
BEGIN
    UPDATE DangKy
    SET DiemThi = @DiemThi
    WHERE MaSV = @MaSV AND MaMH = @MaMH;
END
GO

-- Xóa đăng ký
CREATE PROCEDURE sp_DeleteDangKy
    @MaSV CHAR(10),
    @MaMH CHAR(10)
AS
BEGIN
    DELETE FROM DangKy
    WHERE MaSV = @MaSV AND MaMH = @MaMH;
END
GO

-- Lấy toàn bộ đăng ký
CREATE PROCEDURE sp_GetAllDangKy
AS
BEGIN
    SELECT * FROM DangKy;
END
GO

-- Lấy đăng ký theo sinh viên
CREATE PROCEDURE sp_GetDangKyBySV
    @MaSV CHAR(10)
AS
BEGIN
    SELECT * FROM DangKy WHERE MaSV = @MaSV;
END
GO

EXEC dbo.sp_GetKhoaByID @MaKhoa = 'CNTT';


USE CSDLPT_SITE1;
GO

-- Kiểm tra user có tồn tại không
SELECT name, type_desc, default_schema_name, create_date
FROM sys.database_principals
WHERE name = 'csdlptsite';

-- Kiểm tra quyền của user
SELECT 
    USER_NAME(grantee_principal_id) AS [User],
    OBJECT_NAME(major_id) AS [Object],
    permission_name,
    state_desc
FROM sys.database_permissions
WHERE grantee_principal_id = USER_ID('csdlptsite');


USE CSDLPT_SITE1;
GO

-- CẤP QUYỀN EXECUTE cho user csdlptsite
GRANT EXECUTE ON SCHEMA::dbo TO csdlptsite;
GO

PRINT '✅ Đã cấp quyền EXECUTE cho csdlptsite!';


-- Xem lại quyền của user
SELECT 
    USER_NAME(grantee_principal_id) AS [User],
    OBJECT_NAME(major_id) AS [Object],
    permission_name,
    state_desc
FROM sys.database_permissions
WHERE grantee_principal_id = USER_ID('csdlptsite')
ORDER BY permission_name;