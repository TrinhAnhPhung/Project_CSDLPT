using QL_HP_DT_SV.Models;
using QL_HP_DT_SV.Services;
using QL_HP_DT_SV.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Admin
{
    public partial class AdminDashboard : Form
    {
        private readonly string _connStr; // Chuỗi kết nối từ LoginForm
        private readonly StudentService _studentService;
        private bool _isEditing = false; // Flag để phân biệt chế độ thêm/sửa

        public AdminDashboard()
        {
            InitializeComponent();
            // 1) Ưu tiên dùng chuỗi kết nối đã đăng nhập
            _connStr = !string.IsNullOrWhiteSpace(Session.ConnectionString)
                ? Session.ConnectionString
                // 2) Fallback: lấy từ App.config (MyDB_Site1)
                : ConfigurationManager.ConnectionStrings["MyDB_Site1"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(_connStr))
            {
                MessageBox.Show("Chưa có ConnectionString (Session & App.config đều rỗng).",
                                "Thiếu cấu hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Khởi tạo StudentService
            _studentService = new StudentService(_connStr);

            // Gắn sự kiện
            btnTruyXuat.Click += btnTruyXuat_Click;
            button2.Click += btnThem_Click; // Thêm
            button1.Click += btnSua_Click; // Sửa
            button4.Click += btnXoa_Click; // Xóa
            button6.Click += btnTimKiem_Click; // Tìm kiếm
            button3.Click += button3_Click; // Khoa Ngôn ngữ Anh
            button7.Click += button7_Click; // Khoa Quản trị kinh doanh
            button8.Click += button8_Click; // Truy xuất tất cả sinh viên 3 khoa
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged; // Chọn ngành học
            tbl_result.SelectionChanged += Tbl_result_SelectionChanged;
            tbl_result.CellDoubleClick += Tbl_result_CellDoubleClick;

            // Cấu hình DataGridView
            tbl_result.AutoGenerateColumns = true;
            tbl_result.ReadOnly = true;
            tbl_result.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbl_result.AllowUserToAddRows = false;
            tbl_result.AllowUserToDeleteRows = false;
            tbl_result.MultiSelect = false;

            // Load danh sách khoa vào ComboBox
            _ = LoadKhoaToComboBoxAsync();

            // Load dữ liệu ban đầu
            _ = LoadAllStudentsAsync();
        }

        private void Tbl_result_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                LoadSelectedStudentToForm();
            }
        }

        private void Tbl_result_SelectionChanged(object sender, EventArgs e)
        {
            // Tự động load dữ liệu khi chọn row
            if (tbl_result.SelectedRows.Count > 0)
            {
                var selectedRow = tbl_result.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["MaSV"]?.Value?.ToString() ?? "";
                textBox2.Text = selectedRow.Cells["HoTenSV"]?.Value?.ToString() ?? "";
                textBox3.Text = selectedRow.Cells["MaKhoa"]?.Value?.ToString() ?? "";
                if (selectedRow.Cells["KhoaHoc"] != null)
                {
                    textBox4.Text = selectedRow.Cells["KhoaHoc"]?.Value?.ToString() ?? "";
                }
                _isEditing = true;
            }
        }

        /// <summary>
        /// Load dữ liệu sinh viên được chọn vào form
        /// </summary>
        private void LoadSelectedStudentToForm()
        {
            if (tbl_result.SelectedRows.Count > 0)
            {
                var selectedRow = tbl_result.SelectedRows[0];
                textBox1.Text = selectedRow.Cells["MaSV"]?.Value?.ToString() ?? "";
                textBox2.Text = selectedRow.Cells["HoTenSV"]?.Value?.ToString() ?? "";
                textBox3.Text = selectedRow.Cells["MaKhoa"]?.Value?.ToString() ?? "";
                // textBox4 được dùng cho KhoaHoc (tạm thời repurpose, có thể cần thêm textbox mới)
                // Kiểm tra xem có cột KhoaHoc không
                if (selectedRow.Cells["KhoaHoc"] != null)
                {
                    textBox4.Text = selectedRow.Cells["KhoaHoc"]?.Value?.ToString() ?? "";
                }
                _isEditing = true;
            }
        }

        /// <summary>
        /// Xóa dữ liệu trong form
        /// </summary>
        private void ClearForm()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            _isEditing = false;
            tbl_result.ClearSelection();
        }

        /// <summary>
        /// Lấy dữ liệu từ form vào model SinhVien
        /// </summary>
        private SinhVien GetStudentFromForm()
        {
            return new SinhVien
            {
                MaSV = textBox1.Text.Trim(),
                HoTenSV = textBox2.Text.Trim(),
                MaKhoa = textBox3.Text.Trim(),
                KhoaHoc = textBox4.Text.Trim()
            };
        }

        /// <summary>
        /// Kiểm tra dữ liệu đầu vào
        /// </summary>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã sinh viên!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên sinh viên!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            return true;
        }

        private async void btnTruyXuat_Click(object sender, EventArgs e) => await LoadAllStudentsAsync();

        private async Task LoadAllStudentsAsync()
        {
            try
            {
                var dt = await _studentService.GetAllStudentsAsync();
                tbl_result.DataSource = dt;

                // Đặt header dễ đọc
                if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Mã SV";
                if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Họ tên SV";
                if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Mã khoa";
                if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Khoá học";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách sinh viên.\n" + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thêm sinh viên mới
        /// </summary>
        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            var sv = GetStudentFromForm();

            try
            {
                bool success = await _studentService.AddStudentAsync(sv);
                if (success)
                {
                    MessageBox.Show("Thêm sinh viên thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    await LoadAllStudentsAsync();
                }
                else
                {
                    MessageBox.Show("Không thể thêm sinh viên!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sửa thông tin sinh viên
        /// </summary>
        private async void btnSua_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            if (!_isEditing && string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ danh sách (double-click hoặc click vào row rồi nhấn Sửa)!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sv = GetStudentFromForm();

            try
            {
                // Kiểm tra xem sinh viên có tồn tại không
                var existing = await _studentService.GetStudentByMaSVAsync(sv.MaSV);
                if (existing == null)
                {
                    MessageBox.Show($"Không tìm thấy sinh viên với mã '{sv.MaSV}'!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool success = await _studentService.UpdateStudentAsync(sv);
                if (success)
                {
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    await LoadAllStudentsAsync();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật thông tin sinh viên!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xóa sinh viên
        /// </summary>
        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa từ danh sách!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maSV = textBox1.Text.Trim();
            
            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sinh viên '{maSV}'?\n\nLưu ý: Hành động này không thể hoàn tác!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
                return;

            try
            {
                bool success = await _studentService.DeleteStudentAsync(maSV);
                if (success)
                {
                    MessageBox.Show("Xóa sinh viên thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    await LoadAllStudentsAsync();
                }
                else
                {
                    MessageBox.Show("Không thể xóa sinh viên!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tìm kiếm sinh viên
        /// </summary>
        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            using (var searchForm = new SearchStudentForm())
            {
                if (searchForm.ShowDialog(this) == DialogResult.OK)
                {
                    string keyword = searchForm.SearchKeyword;

                    if (string.IsNullOrWhiteSpace(keyword))
                    {
                        // Nếu không nhập gì, load tất cả
                        await LoadAllStudentsAsync();
                        return;
                    }

                    try
                    {
                        var dt = await _studentService.SearchStudentsAsync(keyword);
                        tbl_result.DataSource = dt;

                        // Đặt header dễ đọc
                        if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Mã SV";
                        if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Họ tên SV";
                        if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Mã khoa";
                        if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Khoá học";

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy sinh viên nào phù hợp!", "Thông báo", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi tìm kiếm sinh viên.\n" + ex.Message, "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tbl_result_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Không cần xử lý gì ở đây, dùng SelectionChanged hoặc DoubleClick
        }

        /// <summary>
        /// Load danh sách khoa vào ComboBox
        /// </summary>
        private async Task LoadKhoaToComboBoxAsync()
        {
            try
            {
                var dt = await MultiSiteService.GetAllKhoaFromAllSitesAsync();
                
                // Tạo DataTable mới với TenKhoa và Site để hiển thị
                var displayTable = new DataTable();
                displayTable.Columns.Add("DisplayText", typeof(string));
                displayTable.Columns.Add("TenKhoa", typeof(string));
                displayTable.Columns.Add("Site", typeof(string));
                displayTable.Columns.Add("MaKhoa", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    var newRow = displayTable.NewRow();
                    string tenKhoa = row["TenKhoa"]?.ToString() ?? "";
                    string site = row["Site"]?.ToString() ?? "";
                    string maKhoa = row["MaKhoa"]?.ToString() ?? "";
                    newRow["DisplayText"] = $"{tenKhoa} ({site})";
                    newRow["TenKhoa"] = tenKhoa;
                    newRow["Site"] = site;
                    newRow["MaKhoa"] = maKhoa;
                    displayTable.Rows.Add(newRow);
                }

                comboBox1.DisplayMember = "DisplayText";
                comboBox1.ValueMember = "TenKhoa";
                comboBox1.DataSource = displayTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách khoa.\n" + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Truy xuất sinh viên khoa Ngôn ngữ Anh từ Site2 và Site3
        /// </summary>
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var resultTable = new DataTable();
                resultTable.Columns.Add("MaSV", typeof(string));
                resultTable.Columns.Add("HoTenSV", typeof(string));
                resultTable.Columns.Add("MaKhoa", typeof(string));
                resultTable.Columns.Add("KhoaHoc", typeof(string));
                resultTable.Columns.Add("Site", typeof(string));

                string site2ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site2"]?.ConnectionString;
                string site3ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site3"]?.ConnectionString;

                // Site2
                if (!string.IsNullOrWhiteSpace(site2ConnStr))
                {
                    try
                    {
                        var dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(site2ConnStr, "Ngôn ngữ Anh");
                        foreach (DataRow row in dt.Rows)
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
                        MessageBox.Show($"Lỗi khi lấy dữ liệu từ Site2: {ex.Message}", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Site3
                if (!string.IsNullOrWhiteSpace(site3ConnStr))
                {
                    try
                    {
                        var dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(site3ConnStr, "Ngôn ngữ Anh");
                        foreach (DataRow row in dt.Rows)
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
                        MessageBox.Show($"Lỗi khi lấy dữ liệu từ Site3: {ex.Message}", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                tbl_result.DataSource = resultTable;

                // Đặt header dễ đọc
                if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Mã SV";
                if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Họ tên SV";
                if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Mã khoa";
                if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Khoá học";
                if (tbl_result.Columns["Site"] != null) tbl_result.Columns["Site"].HeaderText = "Site";

                if (resultTable.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sinh viên khoa Ngôn ngữ Anh từ Site2 và Site3!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Đã tìm thấy {resultTable.Rows.Count} sinh viên khoa Ngôn ngữ Anh!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy xuất sinh viên khoa Ngôn ngữ Anh.\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Query Business Administration faculty students from Site2 and Site3 separately
        /// </summary>
        private async void button7_Click(object sender, EventArgs e)
        {
            try
            {
                var resultTable = new DataTable();
                resultTable.Columns.Add("MaSV", typeof(string));
                resultTable.Columns.Add("HoTenSV", typeof(string));
                resultTable.Columns.Add("MaKhoa", typeof(string));
                resultTable.Columns.Add("KhoaHoc", typeof(string));
                resultTable.Columns.Add("Site", typeof(string));

                string site2ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site2"]?.ConnectionString;
                string site3ConnStr = ConfigurationManager.ConnectionStrings["MyDB_Site3"]?.ConnectionString;

                int site2Count = 0;
                int site3Count = 0;

                // Query Site2 separately
                if (!string.IsNullOrWhiteSpace(site2ConnStr))
                {
                    try
                    {
                        // Try English name first, fallback to Vietnamese
                        var dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(site2ConnStr, "Business Administration");
                        if (dt.Rows.Count == 0)
                        {
                            dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(site2ConnStr, "Quản trị kinh doanh");
                        }
                        
                        foreach (DataRow row in dt.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaSV"] = row["MaSV"];
                            newRow["HoTenSV"] = row["HoTenSV"];
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["KhoaHoc"] = row["KhoaHoc"];
                            newRow["Site"] = "Site2";
                            resultTable.Rows.Add(newRow);
                            site2Count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error retrieving data from Site2: {ex.Message}", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                // Query Site3 separately
                if (!string.IsNullOrWhiteSpace(site3ConnStr))
                {
                    try
                    {
                        // Try English name first, fallback to Vietnamese
                        var dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(site3ConnStr, "Business Administration");
                        if (dt.Rows.Count == 0)
                        {
                            dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(site3ConnStr, "Quản trị kinh doanh");
                        }
                        
                        foreach (DataRow row in dt.Rows)
                        {
                            var newRow = resultTable.NewRow();
                            newRow["MaSV"] = row["MaSV"];
                            newRow["HoTenSV"] = row["HoTenSV"];
                            newRow["MaKhoa"] = row["MaKhoa"];
                            newRow["KhoaHoc"] = row["KhoaHoc"];
                            newRow["Site"] = "Site3";
                            resultTable.Rows.Add(newRow);
                            site3Count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error retrieving data from Site3: {ex.Message}", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                tbl_result.DataSource = resultTable;

                // Set readable headers
                if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Student ID";
                if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Full Name";
                if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Faculty Code";
                if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Academic Year";
                if (tbl_result.Columns["Site"] != null) tbl_result.Columns["Site"].HeaderText = "Site";

                if (resultTable.Rows.Count == 0)
                {
                    MessageBox.Show("No Business Administration students found from Site2 and Site3.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Found {resultTable.Rows.Count} Business Administration students:\n" +
                                   $"  - Site2: {site2Count} students\n" +
                                   $"  - Site3: {site3Count} students", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Business Administration students.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Truy xuất tất cả sinh viên của 3 khoa (CNTT từ Site1, Ngôn ngữ Anh và Quản trị kinh doanh từ Site2 và Site3)
        /// </summary>
        private async void button8_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = await MultiSiteService.GetAllStudentsFromThreeKhoaAsync();
                tbl_result.DataSource = dt;

                // Đặt header dễ đọc
                if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Mã SV";
                if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Họ tên SV";
                if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Mã khoa";
                if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Khoá học";
                if (tbl_result.Columns["Site"] != null) tbl_result.Columns["Site"].HeaderText = "Site";

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sinh viên nào!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Đã tìm thấy {dt.Rows.Count} sinh viên từ 3 khoa!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy xuất tất cả sinh viên.\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý khi chọn ngành học từ ComboBox
        /// </summary>
        private async void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
                return;

            try
            {
                var selectedRow = comboBox1.SelectedItem as DataRowView;
                if (selectedRow == null)
                    return;

                string tenKhoa = selectedRow["TenKhoa"]?.ToString() ?? "";
                string site = selectedRow["Site"]?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(tenKhoa) || string.IsNullOrWhiteSpace(site))
                    return;

                // Lấy connection string theo site
                string connectionString = null;
                if (site == "Site1")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["MyDB_Site1"]?.ConnectionString;
                }
                else if (site == "Site2")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["MyDB_Site2"]?.ConnectionString;
                }
                else if (site == "Site3")
                {
                    connectionString = ConfigurationManager.ConnectionStrings["MyDB_Site3"]?.ConnectionString;
                }

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    MessageBox.Show($"Không tìm thấy connection string cho {site}!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(connectionString, tenKhoa);
                
                // Thêm cột Site vào DataTable
                if (!dt.Columns.Contains("Site"))
                {
                    dt.Columns.Add("Site", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["Site"] = site;
                    }
                }

                tbl_result.DataSource = dt;

                // Đặt header dễ đọc
                if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Mã SV";
                if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Họ tên SV";
                if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Mã khoa";
                if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Khoá học";
                if (tbl_result.Columns["Site"] != null) tbl_result.Columns["Site"].HeaderText = "Site";

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy sinh viên khoa {tenKhoa} từ {site}!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy xuất sinh viên theo ngành học.\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
