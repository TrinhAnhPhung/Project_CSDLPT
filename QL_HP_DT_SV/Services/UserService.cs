using QL_HP_DT_SV.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QL_HP_DT_SV.Services
{
    internal class UserService
    {
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Lấy tất cả người dùng từ SQL Server (sys.database_principals)
        /// </summary>
        public async Task<DataTable> GetAllUsersAsync()
        {
            const string sql = @"
                SELECT 
                    name AS UserName,
                    type_desc AS UserType,
                    create_date AS CreatedDate,
                    modify_date AS ModifiedDate,
                    ISNULL((
                        SELECT TOP 1 name 
                        FROM sys.database_role_members rm
                        INNER JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                        WHERE rm.member_principal_id = dp.principal_id
                        AND r.name IN ('db_owner', 'db_securityadmin', 'db_accessadmin', 'db_datareader', 'db_datawriter')
                        ORDER BY CASE r.name
                            WHEN 'db_owner' THEN 1
                            WHEN 'db_securityadmin' THEN 2
                            WHEN 'db_accessadmin' THEN 3
                            ELSE 4
                        END
                    ), 'User') AS Role
                FROM sys.database_principals dp
                WHERE dp.type IN ('S', 'U')  -- SQL User hoặc Windows User
                    AND dp.name NOT IN ('dbo', 'guest', 'INFORMATION_SCHEMA', 'sys')
                    AND dp.is_fixed_role = 0
                ORDER BY dp.name";

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
                throw new Exception($"Lỗi khi lấy danh sách người dùng: {ex.Message}", ex);
            }
            return dt;
        }

        /// <summary>
        /// Lấy danh sách roles có sẵn
        /// </summary>
        public async Task<DataTable> GetAvailableRolesAsync()
        {
            const string sql = @"
                SELECT name AS RoleName, 
                       type_desc AS RoleType
                FROM sys.database_principals
                WHERE type = 'R'
                    AND name IN ('db_owner', 'db_securityadmin', 'db_accessadmin', 
                                 'db_datareader', 'db_datawriter', 'db_ddladmin')
                ORDER BY name";

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
                throw new Exception($"Lỗi khi lấy danh sách roles: {ex.Message}", ex);
            }
            return dt;
        }

        /// <summary>
        /// Lấy roles của một user cụ thể
        /// </summary>
        public async Task<DataTable> GetUserRolesAsync(string userName)
        {
            const string sql = @"
                SELECT r.name AS RoleName
                FROM sys.database_role_members rm
                INNER JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                INNER JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id
                WHERE m.name = @UserName
                    AND r.type = 'R'";

            var dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    await conn.OpenAsync();
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy roles của user: {ex.Message}", ex);
            }
            return dt;
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            const string sql = @"
                SELECT COUNT(*) 
                FROM sys.database_principals 
                WHERE name = @UserName 
                    AND type IN ('S', 'U')";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thống kê tổng quan
        /// </summary>
        public async Task<DataTable> GetStatisticsAsync()
        {
            const string sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM dbo.SinhVien) AS TotalStudents,
                    (SELECT COUNT(*) FROM dbo.HocPhan) AS TotalHocPhan,
                    (SELECT COUNT(*) FROM sys.database_principals WHERE type IN ('S', 'U') AND is_fixed_role = 0) AS TotalUsers";

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
                // Nếu bảng không tồn tại, trả về bảng rỗng
                dt.Columns.Add("TotalStudents", typeof(int));
                dt.Columns.Add("TotalHocPhan", typeof(int));
                dt.Columns.Add("TotalUsers", typeof(int));
                var row = dt.NewRow();
                row["TotalStudents"] = 0;
                row["TotalHocPhan"] = 0;
                row["TotalUsers"] = 0;
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}

