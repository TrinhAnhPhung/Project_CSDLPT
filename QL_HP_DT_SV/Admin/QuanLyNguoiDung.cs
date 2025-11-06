using QL_HP_DT_SV.Services;
using QL_HP_DT_SV.Utils;
using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Admin
{
    public partial class QuanLyNguoiDung : Form
    {
        private readonly string _connStr;
        private readonly UserService _userService;

        public QuanLyNguoiDung()
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
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var dt = await _userService.GetAllUsersAsync();
                dgvUsers.DataSource = dt;

                // Đặt header dễ đọc
                if (dgvUsers.Columns["UserName"] != null)
                    dgvUsers.Columns["UserName"].HeaderText = "Tên đăng nhập";
                if (dgvUsers.Columns["UserType"] != null)
                    dgvUsers.Columns["UserType"].HeaderText = "Loại";
                if (dgvUsers.Columns["Role"] != null)
                    dgvUsers.Columns["Role"].HeaderText = "Vai trò";
                if (dgvUsers.Columns["CreatedDate"] != null)
                    dgvUsers.Columns["CreatedDate"].HeaderText = "Ngày tạo";
                if (dgvUsers.Columns["ModifiedDate"] != null)
                    dgvUsers.Columns["ModifiedDate"].HeaderText = "Ngày sửa";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách người dùng.\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                var selectedRow = dgvUsers.SelectedRows[0];
                txtUserName.Text = selectedRow.Cells["UserName"]?.Value?.ToString() ?? "";
                lblUserRole.Text = selectedRow.Cells["Role"]?.Value?.ToString() ?? "";
            }
        }
    }
}

