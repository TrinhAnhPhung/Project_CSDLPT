using QL_HP_DT_SV.Services;
using QL_HP_DT_SV.Utils;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Admin
{
    public partial class Site2Dashboard : Form
    {
        private readonly string _connStr;
        private readonly string _allowedKhoa = "Ngôn ngữ Anh"; // Chỉ cho phép truy vấn khoa này

        public Site2Dashboard()
        {
            InitializeComponent();
            
            // Lấy connection string từ Session hoặc config
            _connStr = !string.IsNullOrWhiteSpace(Session.ConnectionString)
                ? Session.ConnectionString
                : ConfigurationManager.ConnectionStrings["MyDB_Site2"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(_connStr))
            {
                MessageBox.Show("Connection string not found for Site2.",
                                "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cấu hình DataGridView
            tbl_result.AutoGenerateColumns = true;
            tbl_result.ReadOnly = true;
            tbl_result.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbl_result.AllowUserToAddRows = false;
            tbl_result.AllowUserToDeleteRows = false;
            tbl_result.MultiSelect = false;

            // Load dữ liệu ban đầu
            _ = LoadStudentsAsync();
        }

        /// <summary>
        /// Load sinh viên khoa Ngôn ngữ Anh từ Site2
        /// </summary>
        private async Task LoadStudentsAsync()
        {
            try
            {
                var dt = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(_connStr, _allowedKhoa);
                
                // Thêm cột Site
                if (!dt.Columns.Contains("Site"))
                {
                    dt.Columns.Add("Site", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["Site"] = "Site2";
                    }
                }

                tbl_result.DataSource = dt;

                // Set readable headers
                if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Student ID";
                if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Full Name";
                if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Faculty Code";
                if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Academic Year";
                if (tbl_result.Columns["Site"] != null) tbl_result.Columns["Site"].HeaderText = "Site";

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show($"No students found for {_allowedKhoa} in Site2.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students.\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Refresh button - reload data
        /// </summary>
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadStudentsAsync();
        }

        /// <summary>
        /// Search button
        /// </summary>
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            using (var searchForm = new SearchStudentForm())
            {
                if (searchForm.ShowDialog(this) == DialogResult.OK)
                {
                    string keyword = searchForm.SearchKeyword;

                    if (string.IsNullOrWhiteSpace(keyword))
                    {
                        await LoadStudentsAsync();
                        return;
                    }

                    try
                    {
                        // Search in current site only
                        var allStudents = await MultiSiteService.GetStudentsByKhoaFromSiteAsync(_connStr, _allowedKhoa);
                        
                        // Filter by keyword
                        var filteredRows = allStudents.AsEnumerable()
                            .Where(row => 
                                row["MaSV"]?.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                                row["HoTenSV"]?.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                                row["MaKhoa"]?.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
                            .ToArray();

                        if (filteredRows.Length > 0)
                        {
                            var filteredTable = allStudents.Clone();
                            foreach (var row in filteredRows)
                            {
                                filteredTable.ImportRow(row);
                            }

                            // Add Site column if not exists
                            if (!filteredTable.Columns.Contains("Site"))
                            {
                                filteredTable.Columns.Add("Site", typeof(string));
                                foreach (DataRow row in filteredTable.Rows)
                                {
                                    row["Site"] = "Site2";
                                }
                            }

                            tbl_result.DataSource = filteredTable;

                            // Set headers
                            if (tbl_result.Columns["MaSV"] != null) tbl_result.Columns["MaSV"].HeaderText = "Student ID";
                            if (tbl_result.Columns["HoTenSV"] != null) tbl_result.Columns["HoTenSV"].HeaderText = "Full Name";
                            if (tbl_result.Columns["MaKhoa"] != null) tbl_result.Columns["MaKhoa"].HeaderText = "Faculty Code";
                            if (tbl_result.Columns["KhoaHoc"] != null) tbl_result.Columns["KhoaHoc"].HeaderText = "Academic Year";
                            if (tbl_result.Columns["Site"] != null) tbl_result.Columns["Site"].HeaderText = "Site";
                        }
                        else
                        {
                            MessageBox.Show("No students found matching the search criteria.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error searching students.\n{ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
