using QL_HP_DT_SV.Admin;
using QL_HP_DT_SV.Models;
using QL_HP_DT_SV.Services;
using QL_HP_DT_SV.Sinh_Viên;
using QL_HP_DT_SV.Utils;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Login
{
    public partial class LoginForm : Form
    {

        private readonly AuthService _auth = new AuthService();
        public LoginForm()
        {
            InitializeComponent();
            this.Load += LoginForm_Load;
            btn_login.Click += tbn_Click;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            var baseConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site1"].ConnectionString;
            using (var conn = new SqlConnection(baseConnStr))
            using (var cmd = new SqlCommand(
                    "SELECT RTRIM(MaKhoa) AS MaKhoa, TenKhoa FROM dbo.Khoa ORDER BY TenKhoa", conn))
            {
                conn.Open();
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                cb_chonnganh.DisplayMember = "TenKhoa";
                cb_chonnganh.ValueMember = "MaKhoa";   // đã là bản RTRIM
                cb_chonnganh.DataSource = dt;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private async void tbn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text?.Trim();
            string password = txtPassword.Text; // giữ nguyên phân biệt ký tự
            string maKhoa = cb_chonnganh.SelectedValue?.ToString(); // ví dụ "CNTT"

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập và Mật khẩu.",
                                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!string.Equals(maKhoa, "CNTT", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Site 1 chỉ cho phép đăng nhập khi chọn ngành Công nghệ thông tin (CNTT).",
                                "Sai ngành", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dựng chuỗi kết nối SQL Authentication từ user/password nhập vào
            string connStr = Helpers.BuildSqlAuthConnection(username, password);

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    // Test quyền & đúng DB bằng cách gọi 1 SP có sẵn (hoặc SELECT đơn giản)
                    using (var cmd = new SqlCommand("dbo.sp_GetKhoaByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaKhoa", "CNTT");

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Không tìm thấy Khoa CNTT trong CSDLPT_SITE1.",
                                                "Sai dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                // Nếu tới đây: mở được kết nối bằng user/password + SP chạy OK => ĐĂNG NHẬP THÀNH CÔNG
                
                // Lưu thông tin session
                Session.ConnectionString = connStr;
                Session.CurrentUserName = username;
                
                // Kiểm tra quyền của user để xác định vai trò
                string userRole = await GetUserRoleAsync(connStr, username);
                Session.CurrentUserRole = userRole;
                Session.IsAdmin = userRole == "db_owner" || userRole == "db_securityadmin";

                // Hiển thị dashboard tương ứng với vai trò
                if (Session.IsAdmin)
                {
                    var dashboard = new AdminMainDashboard();
                    dashboard.StartPosition = FormStartPosition.CenterScreen;
                    dashboard.Show();
                    this.Hide(); // ẩn form Login
                }
                else
                {
                    // Mở AdminDashboard cho các role khác (có thể xem và quản lý sinh viên)
                    MessageBox.Show($"Chào mừng {username}! Vai trò: {userRole}",
                        "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    var dashboard = new AdminDashboard();
                    dashboard.StartPosition = FormStartPosition.CenterScreen;
                    dashboard.Show();
                    this.Hide(); // ẩn form Login
                }
            }
            catch (SqlException sqlEx)
            {
                // Lỗi xác thực/ quyền/ mạng...
                MessageBox.Show("Đăng nhập thất bại.\n" + sqlEx.Message,
                                "SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng nhập thất bại.\n" + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cb_chonnganh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Lấy vai trò của user từ database
        /// </summary>
        private async System.Threading.Tasks.Task<string> GetUserRoleAsync(string connStr, string userName)
        {
            const string sql = @"
                SELECT TOP 1 r.name AS RoleName
                FROM sys.database_role_members rm
                INNER JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                INNER JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id
                WHERE m.name = @UserName
                    AND r.type = 'R'
                    AND r.name IN ('db_owner', 'db_securityadmin', 'db_accessadmin', 'db_datareader', 'db_datawriter')
                ORDER BY CASE r.name
                    WHEN 'db_owner' THEN 1
                    WHEN 'db_securityadmin' THEN 2
                    WHEN 'db_accessadmin' THEN 3
                    ELSE 4
                END";

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return result?.ToString() ?? "User";
                }
            }
            catch
            {
                // Nếu không lấy được role, mặc định là User
                return "User";
            }
        }
    }
}
