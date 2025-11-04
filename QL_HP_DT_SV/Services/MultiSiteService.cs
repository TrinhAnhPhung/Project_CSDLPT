using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QL_HP_DT_SV.Services
{
    /// <summary>
    /// Service để truy vấn dữ liệu từ nhiều sites
    /// </summary>
    internal class MultiSiteService
    {
        /// <summary>
        /// Lấy sinh viên theo khoa từ một site cụ thể
        /// </summary>
        public static async Task<DataTable> GetStudentsByKhoaFromSiteAsync(string connectionString, string tenKhoa)
        {
            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(tenKhoa))
            {
                return new DataTable();
            }

            // Tìm mã khoa theo tên khoa
            string maKhoa = await GetMaKhoaByTenKhoaAsync(connectionString, tenKhoa);
            
            if (string.IsNullOrWhiteSpace(maKhoa))
            {
                return new DataTable();
            }

            const string sql = @"
                SELECT 
                    MaSV,
                    HoTenSV,
                    RTRIM(MaKhoa) AS MaKhoa,
                    KhoaHoc
                FROM dbo.SinhVien
                WHERE RTRIM(MaKhoa) = @MaKhoa
                ORDER BY MaSV";

            var dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa.Trim());

                    await conn.OpenAsync();
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy sinh viên từ site: {ex.Message}", ex);
            }
            return dt;
        }

        /// <summary>
        /// Lấy mã khoa theo tên khoa
        /// </summary>
        public static async Task<string> GetMaKhoaByTenKhoaAsync(string connectionString, string tenKhoa)
        {
            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(tenKhoa))
            {
                return null;
            }

            const string sql = @"
                SELECT TOP 1 RTRIM(MaKhoa) AS MaKhoa
                FROM dbo.Khoa
                WHERE TenKhoa LIKE @TenKhoa";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenKhoa", $"%{tenKhoa.Trim()}%");

                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy mã khoa: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả khoa từ một site
        /// </summary>
        public static async Task<DataTable> GetAllKhoaFromSiteAsync(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return new DataTable();
            }

            const string sql = @"
                SELECT 
                    RTRIM(MaKhoa) AS MaKhoa,
                    TenKhoa
                FROM dbo.Khoa
                ORDER BY TenKhoa";

            var dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    await conn.OpenAsync();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách khoa từ site: {ex.Message}", ex);
            }
            return dt;
        }

        /// <summary>
        /// Lấy tất cả sinh viên của 3 khoa (CNTT từ Site1, Ngôn ngữ Anh và Quản trị kinh doanh từ Site2 và Site3)
        /// </summary>
        public static async Task<DataTable> GetAllStudentsFromThreeKhoaAsync()
        {
            var resultTable = new DataTable();
            resultTable.Columns.Add("MaSV", typeof(string));
            resultTable.Columns.Add("HoTenSV", typeof(string));
            resultTable.Columns.Add("MaKhoa", typeof(string));
            resultTable.Columns.Add("KhoaHoc", typeof(string));
            resultTable.Columns.Add("Site", typeof(string));

            try
            {
                // Lấy connection strings
                string site1ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site1"]?.ConnectionString;
                string site2ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site2"]?.ConnectionString;
                string site3ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site3"]?.ConnectionString;

                // Site1: Lấy sinh viên khoa CNTT
                if (!string.IsNullOrWhiteSpace(site1ConnStr))
                {
                    try
                    {
                        var dtCNTT = await GetStudentsByKhoaFromSiteAsync(site1ConnStr, "CNTT");
                        foreach (DataRow row in dtCNTT.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaSV"] = row["MaSV"];
                            newRow["HoTenSV"] = row["HoTenSV"];
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["KhoaHoc"] = row["KhoaHoc"];
                            newRow["Site"] = "Site1";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy CNTT từ Site1: {ex.Message}");
                    }
                }

                // Site2: Lấy sinh viên khoa Ngôn ngữ Anh
                if (!string.IsNullOrWhiteSpace(site2ConnStr))
                {
                    try
                    {
                        var dtNNA = await GetStudentsByKhoaFromSiteAsync(site2ConnStr, "Ngôn ngữ Anh");
                        foreach (DataRow row in dtNNA.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaSV"] = row["MaSV"];
                            newRow["HoTenSV"] = row["HoTenSV"];
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["KhoaHoc"] = row["KhoaHoc"];
                            newRow["Site"] = "Site2";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy Ngôn ngữ Anh từ Site2: {ex.Message}");
                    }
                }

                // Site3: Lấy sinh viên khoa Quản trị kinh doanh
                if (!string.IsNullOrWhiteSpace(site3ConnStr))
                {
                    try
                    {
                        var dtQTKD = await GetStudentsByKhoaFromSiteAsync(site3ConnStr, "Quản trị kinh doanh");
                        foreach (DataRow row in dtQTKD.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaSV"] = row["MaSV"];
                            newRow["HoTenSV"] = row["HoTenSV"];
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["KhoaHoc"] = row["KhoaHoc"];
                            newRow["Site"] = "Site3";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy Quản trị kinh doanh từ Site3: {ex.Message}");
                    }
                }

                // Site2: Lấy sinh viên khoa Quản trị kinh doanh (nếu có)
                if (!string.IsNullOrWhiteSpace(site2ConnStr))
                {
                    try
                    {
                        var dtQTKD = await GetStudentsByKhoaFromSiteAsync(site2ConnStr, "Quản trị kinh doanh");
                        foreach (DataRow row in dtQTKD.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaSV"] = row["MaSV"];
                            newRow["HoTenSV"] = row["HoTenSV"];
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["KhoaHoc"] = row["KhoaHoc"];
                            newRow["Site"] = "Site2";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy Quản trị kinh doanh từ Site2: {ex.Message}");
                    }
                }

                // Site3: Lấy sinh viên khoa Ngôn ngữ Anh (nếu có)
                if (!string.IsNullOrWhiteSpace(site3ConnStr))
                {
                    try
                    {
                        var dtNNA = await GetStudentsByKhoaFromSiteAsync(site3ConnStr, "Ngôn ngữ Anh");
                        foreach (DataRow row in dtNNA.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaSV"] = row["MaSV"];
                            newRow["HoTenSV"] = row["HoTenSV"];
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["KhoaHoc"] = row["KhoaHoc"];
                            newRow["Site"] = "Site3";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy Ngôn ngữ Anh từ Site3: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy sinh viên từ 3 khoa: {ex.Message}", ex);
            }

            return resultTable;
        }

        /// <summary>
        /// Lấy tất cả khoa từ tất cả các sites
        /// </summary>
        public static async Task<DataTable> GetAllKhoaFromAllSitesAsync()
        {
            var resultTable = new DataTable();
            resultTable.Columns.Add("MaKhoa", typeof(string));
            resultTable.Columns.Add("TenKhoa", typeof(string));
            resultTable.Columns.Add("Site", typeof(string));

            try
            {
                string site1ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site1"]?.ConnectionString;
                string site2ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site2"]?.ConnectionString;
                string site3ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site3"]?.ConnectionString;

                // Site1
                if (!string.IsNullOrWhiteSpace(site1ConnStr))
                {
                    try
                    {
                        var dt = await GetAllKhoaFromSiteAsync(site1ConnStr);
                        foreach (DataRow row in dt.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["TenKhoa"] = row["TenKhoa"];
                            newRow["Site"] = "Site1";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy khoa từ Site1: {ex.Message}");
                    }
                }

                // Site2
                if (!string.IsNullOrWhiteSpace(site2ConnStr))
                {
                    try
                    {
                        var dt = await GetAllKhoaFromSiteAsync(site2ConnStr);
                        foreach (DataRow row in dt.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["TenKhoa"] = row["TenKhoa"];
                            newRow["Site"] = "Site2";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy khoa từ Site2: {ex.Message}");
                    }
                }

                // Site3
                if (!string.IsNullOrWhiteSpace(site3ConnStr))
                {
                    try
                    {
                        var dt = await GetAllKhoaFromSiteAsync(site3ConnStr);
                        foreach (DataRow row in dt.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["TenKhoa"] = row["TenKhoa"];
                            newRow["Site"] = "Site3";
                            resultTable.Rows.Add(newRow);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lấy khoa từ Site3: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy khoa từ tất cả sites: {ex.Message}", ex);
            }

            return resultTable;
        }
    }
}

