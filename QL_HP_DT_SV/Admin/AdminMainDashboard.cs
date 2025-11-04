using QL_HP_DT_SV.Services;
using QL_HP_DT_SV.Utils;
using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Admin
{
    public partial class AdminMainDashboard : Form
    {
        private readonly string _connStr;
        private readonly UserService _userService;
        private readonly StudentService _studentService;

        public AdminMainDashboard()
        {
            InitializeComponent();
            
            _connStr = !string.IsNullOrWhiteSpace(Session.ConnectionString)
                ? Session.ConnectionString
                : ConfigurationManager.ConnectionStrings["MyDB_Site1"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(_connStr))
            {
                MessageBox.Show("Chưa có ConnectionString.", "Thiếu cấu hình", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _userService = new UserService(_connStr);
            _studentService = new StudentService(_connStr);

            // Load thống kê ban đầu
            LoadStatisticsAsync();

            // Gắn sự kiện
            btnQuanLySinhVien.Click += BtnQuanLySinhVien_Click;
            btnQuanLyNguoiDung.Click += BtnQuanLyNguoiDung_Click;
            btnPhanQuyen.Click += BtnPhanQuyen_Click;
           
            btnLogout.Click += BtnLogout_Click;
            this.FormClosing += AdminMainDashboard_FormClosing;
        }

        private async void LoadStatisticsAsync()
        {
            try
            {
                // Load thống kê
                var stats = await _userService.GetStatisticsAsync();
                
                if (stats.Rows.Count > 0)
                {
                    var row = stats.Rows[0];
                    lblTotalStudents.Text = row["TotalStudents"]?.ToString() ?? "0";
                    lblTotalHocPhan.Text = row["TotalHocPhan"]?.ToString() ?? "0";
                    lblTotalUsers.Text = row["TotalUsers"]?.ToString() ?? "0";
                }

                // Load thông tin người dùng hiện tại
                if (!string.IsNullOrWhiteSpace(Session.CurrentUserName))
                {
                    lblCurrentUser.Text = $"Người dùng: {Session.CurrentUserName}";
                    lblCurrentRole.Text = $"Vai trò: {Session.CurrentUserRole ?? "Admin"}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được thống kê.\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnQuanLySinhVien_Click(object sender, EventArgs e)
        {
            var form = new AdminDashboard();
            form.ShowDialog();
        }

        private void BtnQuanLyNguoiDung_Click(object sender, EventArgs e)
        {
            var form = new QuanLyNguoiDung();
            form.ShowDialog();
        }

        private void BtnPhanQuyen_Click(object sender, EventArgs e)
        {
         
        }

        

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Session.Clear();
                this.Hide();
                
                var loginForm = new Login.LoginForm();
                loginForm.Show();
            }
        }

        private void AdminMainDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đóng ứng dụng?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Session.Clear();
                Application.Exit();
            }
        }

        private void btnPhanQuyen_Click_1(object sender, EventArgs e)
        {

        }

        private void btnQuanLySinhVien_Click_1(object sender, EventArgs e)
        {

        }
    }
}

