using QL_HP_DT_SV.Admin;
using QL_HP_DT_SV.Models;
using QL_HP_DT_SV.Services;
using QL_HP_DT_SV.Utils;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Load += LoginForm_Load;
            btn_login.Click += tbn_Click;
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            // Load khoa từ cả 3 sites
            await LoadKhoaFromAllSitesAsync();
        }

        /// <summary>
        /// Load danh sách khoa từ cả 3 sites
        /// </summary>
        private async System.Threading.Tasks.Task LoadKhoaFromAllSitesAsync()
        {
            try
            {
                var allKhoa = await MultiSiteService.GetAllKhoaFromAllSitesAsync();
                
                // Tạo DataTable để hiển thị với thông tin Site
                var displayTable = new DataTable();
                displayTable.Columns.Add("MaKhoa", typeof(string));
                displayTable.Columns.Add("TenKhoa", typeof(string));
                displayTable.Columns.Add("Site", typeof(string));
                displayTable.Columns.Add("DisplayText", typeof(string));

                foreach (DataRow row in allKhoa.Rows)
                {
                    var newRow = displayTable.NewRow();
                    newRow["MaKhoa"] = row["MaKhoa"];
                    newRow["TenKhoa"] = row["TenKhoa"];
                    newRow["Site"] = row["Site"];
                    newRow["DisplayText"] = $"{row["TenKhoa"]} ({row["Site"]})";
                    displayTable.Rows.Add(newRow);
                }

                cb_chonnganh.DisplayMember = "DisplayText";
                cb_chonnganh.ValueMember = "MaKhoa";
                cb_chonnganh.DataSource = displayTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể load danh sách khoa.\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xác định Site dựa trên tên khoa hoặc mã khoa
        /// </summary>
        private string DetermineSiteFromKhoa(string maKhoa, string tenKhoa)
        {
            if (string.IsNullOrWhiteSpace(tenKhoa) && string.IsNullOrWhiteSpace(maKhoa))
                return "Site1"; // Default

            // Site1: CNTT
            if (maKhoa?.Equals("CNTT", StringComparison.OrdinalIgnoreCase) == true ||
                tenKhoa?.Contains("CNTT", StringComparison.OrdinalIgnoreCase) == true ||
                tenKhoa?.Contains("Công nghệ thông tin", StringComparison.OrdinalIgnoreCase) == true)
            {
                return "Site1";
            }

            // Site2: Ngôn ngữ Anh
            if (tenKhoa?.Equals("Ngôn ngữ Anh", StringComparison.OrdinalIgnoreCase) == true ||
                tenKhoa?.Contains("Ngôn ngữ Anh", StringComparison.OrdinalIgnoreCase) == true ||
                tenKhoa?.Contains("English Language", StringComparison.OrdinalIgnoreCase) == true)
            {
                return "Site2";
            }
            
            // Site3: Quản trị kinh doanh / Business Administration
            if (tenKhoa?.Equals("Quản trị kinh doanh", StringComparison.OrdinalIgnoreCase) == true ||
                tenKhoa?.Contains("Quản trị kinh doanh", StringComparison.OrdinalIgnoreCase) == true ||
                tenKhoa?.Contains("Business Administration", StringComparison.OrdinalIgnoreCase) == true)
            {
                return "Site3";
            }
            
            return "Site1"; // Default
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

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập và Mật khẩu.",
                                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cb_chonnganh.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn ngành học.",
                                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin khoa được chọn
            var selectedRow = cb_chonnganh.SelectedItem as DataRowView;
            if (selectedRow == null)
            {
                MessageBox.Show("Không thể lấy thông tin khoa được chọn.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maKhoa = selectedRow["MaKhoa"]?.ToString();
            string tenKhoa = selectedRow["TenKhoa"]?.ToString();
            string site = DetermineSiteFromKhoa(maKhoa, tenKhoa);

            // Dựng chuỗi kết nối SQL Authentication từ user/password nhập vào cho site tương ứng
            string connStr;
            try
            {
                connStr = Helpers.BuildSqlAuthConnection(username, password, site);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tạo kết nối đến {site}.\n{ex.Message}",
                                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    // Kiểm tra khoa có tồn tại trong site không
                    string checkSql = @"SELECT TOP 1 RTRIM(MaKhoa) AS MaKhoa, TenKhoa 
                                       FROM dbo.Khoa 
                                       WHERE RTRIM(MaKhoa) = @MaKhoa";
                    
                    using (var cmd = new SqlCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhoa", maKhoa?.Trim() ?? "");
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show($"Không tìm thấy khoa '{tenKhoa}' trong {site}.",
                                                "Sai dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                }

                // ĐĂNG NHẬP THÀNH CÔNG
                
                // Lưu thông tin session
                Session.ConnectionString = connStr;
                Session.CurrentUserName = username;
                Session.CurrentSite = site;
                Session.CurrentMaKhoa = maKhoa;
                Session.CurrentTenKhoa = tenKhoa;
                
                // Kiểm tra quyền của user để xác định vai trò
                string userRole = await GetUserRoleAsync(connStr, username);
                Session.CurrentUserRole = userRole;
                Session.IsAdmin = userRole == "db_owner" || userRole == "db_securityadmin";

                // Đóng form đăng nhập
                this.Hide();

                // Mở dashboard tương ứng với site
                Form dashboard = null;
                if (site == "Site1")
                {
                    dashboard = new AdminDashboard();
                }
                else if (site == "Site2")
                {
                    dashboard = new Site2Dashboard();
                }
                else if (site == "Site3")
                {
                    dashboard = new Site3Dashboard();
                }

                if (dashboard != null)
                {
                    dashboard.FormClosed += (s, args) => this.Close(); // Đóng app khi đóng dashboard
                    dashboard.Show();
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
