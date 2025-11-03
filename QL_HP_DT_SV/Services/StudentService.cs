using QL_HP_DT_SV.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Services
{
    internal class StudentService
    {
        private readonly string _connectionString;

        public StudentService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Lấy tất cả sinh viên
        /// </summary>
        public async Task<DataTable> GetAllStudentsAsync()
        {
            const string sql = @"
                SELECT 
                    MaSV,
                    HoTenSV,
                    RTRIM(MaKhoa) AS MaKhoa,
                    KhoaHoc
                FROM dbo.SinhVien
                ORDER BY MaSV";

            var dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    await conn.OpenAsync();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách sinh viên: {ex.Message}", ex);
            }
            return dt;
        }

        /// <summary>
        /// Thêm sinh viên mới
        /// </summary>
        public async Task<bool> AddStudentAsync(SinhVien sv)
        {
            if (string.IsNullOrWhiteSpace(sv.MaSV))
            {
                throw new Exception("Mã sinh viên không được để trống");
            }

            if (string.IsNullOrWhiteSpace(sv.HoTenSV))
            {
                throw new Exception("Họ tên sinh viên không được để trống");
            }

            const string sql = @"
                INSERT INTO dbo.SinhVien (MaSV, HoTenSV, MaKhoa, KhoaHoc)
                VALUES (@MaSV, @HoTenSV, @MaKhoa, @KhoaHoc)";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSV", sv.MaSV);
                    cmd.Parameters.AddWithValue("@HoTenSV", sv.HoTenSV);
                    cmd.Parameters.AddWithValue("@MaKhoa", (object)sv.MaKhoa ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KhoaHoc", (object)sv.KhoaHoc ?? DBNull.Value);

                    await conn.OpenAsync();
                    int result = await cmd.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Violation of PRIMARY KEY constraint
                {
                    throw new Exception($"Mã sinh viên '{sv.MaSV}' đã tồn tại trong hệ thống.");
                }
                throw new Exception($"Lỗi khi thêm sinh viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin sinh viên
        /// </summary>
        public async Task<bool> UpdateStudentAsync(SinhVien sv)
        {
            if (string.IsNullOrWhiteSpace(sv.MaSV))
            {
                throw new Exception("Mã sinh viên không được để trống");
            }

            if (string.IsNullOrWhiteSpace(sv.HoTenSV))
            {
                throw new Exception("Họ tên sinh viên không được để trống");
            }

            const string sql = @"
                UPDATE dbo.SinhVien
                SET HoTenSV = @HoTenSV,
                    MaKhoa = @MaKhoa,
                    KhoaHoc = @KhoaHoc
                WHERE MaSV = @MaSV";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSV", sv.MaSV);
                    cmd.Parameters.AddWithValue("@HoTenSV", sv.HoTenSV);
                    cmd.Parameters.AddWithValue("@MaKhoa", (object)sv.MaKhoa ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@KhoaHoc", (object)sv.KhoaHoc ?? DBNull.Value);

                    await conn.OpenAsync();
                    int result = await cmd.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật sinh viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa sinh viên
        /// </summary>
        public async Task<bool> DeleteStudentAsync(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                throw new Exception("Mã sinh viên không được để trống");
            }

            const string sql = "DELETE FROM dbo.SinhVien WHERE MaSV = @MaSV";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSV", maSV);

                    await conn.OpenAsync();
                    int result = await cmd.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Foreign key constraint violation
                {
                    throw new Exception($"Không thể xóa sinh viên '{maSV}' vì có dữ liệu liên quan trong hệ thống.");
                }
                throw new Exception($"Lỗi khi xóa sinh viên: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sinh viên theo từ khóa (Mã SV, Họ tên, Mã khoa)
        /// </summary>
        public async Task<DataTable> SearchStudentsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllStudentsAsync();
            }

            const string sql = @"
                SELECT 
                    MaSV,
                    HoTenSV,
                    RTRIM(MaKhoa) AS MaKhoa,
                    KhoaHoc
                FROM dbo.SinhVien
                WHERE MaSV LIKE @Keyword 
                   OR HoTenSV LIKE @Keyword 
                   OR RTRIM(MaKhoa) LIKE @Keyword
                   OR KhoaHoc LIKE @Keyword
                ORDER BY MaSV";

            var dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    string searchPattern = $"%{keyword.Trim()}%";
                    cmd.Parameters.AddWithValue("@Keyword", searchPattern);

                    await conn.OpenAsync();
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm sinh viên: {ex.Message}", ex);
            }
            return dt;
        }

        /// <summary>
        /// Lấy thông tin sinh viên theo mã
        /// </summary>
        public async Task<SinhVien> GetStudentByMaSVAsync(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                return null;
            }

            const string sql = @"
                SELECT 
                    MaSV,
                    HoTenSV,
                    RTRIM(MaKhoa) AS MaKhoa,
                    KhoaHoc
                FROM dbo.SinhVien
                WHERE MaSV = @MaSV";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSV", maSV);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new SinhVien
                            {
                                MaSV = reader["MaSV"].ToString(),
                                HoTenSV = reader["HoTenSV"].ToString(),
                                MaKhoa = reader["MaKhoa"]?.ToString() ?? "",
                                KhoaHoc = reader["KhoaHoc"]?.ToString() ?? ""
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin sinh viên: {ex.Message}", ex);
            }
            return null;
        }
    }
}

