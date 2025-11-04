namespace QL_HP_DT_SV.Admin
{
    partial class AdminMainDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelStats = new System.Windows.Forms.Panel();
            this.lblTotalUsersLabel = new System.Windows.Forms.Label();
            this.lblTotalHocPhanLabel = new System.Windows.Forms.Label();
            this.lblTotalStudentsLabel = new System.Windows.Forms.Label();
            this.lblTotalUsers = new System.Windows.Forms.Label();
            this.lblTotalHocPhan = new System.Windows.Forms.Label();
            this.lblTotalStudents = new System.Windows.Forms.Label();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnPhanQuyen = new System.Windows.Forms.Button();
            this.btnQuanLyNguoiDung = new System.Windows.Forms.Button();
            this.btnQuanLySinhVien = new System.Windows.Forms.Button();
            this.panelUserInfo = new System.Windows.Forms.Panel();
            this.lblCurrentRole = new System.Windows.Forms.Label();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelStats.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.panelUserInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1200, 80);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(395, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Bảng điều khiển Admin";
            // 
            // panelStats
            // 
            this.panelStats.BackColor = System.Drawing.Color.White;
            this.panelStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStats.Controls.Add(this.lblTotalUsersLabel);
            this.panelStats.Controls.Add(this.lblTotalHocPhanLabel);
            this.panelStats.Controls.Add(this.lblTotalStudentsLabel);
            this.panelStats.Controls.Add(this.lblTotalUsers);
            this.panelStats.Controls.Add(this.lblTotalHocPhan);
            this.panelStats.Controls.Add(this.lblTotalStudents);
            this.panelStats.Location = new System.Drawing.Point(30, 100);
            this.panelStats.Name = "panelStats";
            this.panelStats.Size = new System.Drawing.Size(1140, 150);
            this.panelStats.TabIndex = 1;
            // 
            // lblTotalUsersLabel
            // 
            this.lblTotalUsersLabel.AutoSize = true;
            this.lblTotalUsersLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalUsersLabel.Location = new System.Drawing.Point(800, 30);
            this.lblTotalUsersLabel.Name = "lblTotalUsersLabel";
            this.lblTotalUsersLabel.Size = new System.Drawing.Size(158, 25);
            this.lblTotalUsersLabel.TabIndex = 5;
            this.lblTotalUsersLabel.Text = "Tổng người dùng";
            // 
            // lblTotalHocPhanLabel
            // 
            this.lblTotalHocPhanLabel.AutoSize = true;
            this.lblTotalHocPhanLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHocPhanLabel.Location = new System.Drawing.Point(430, 30);
            this.lblTotalHocPhanLabel.Name = "lblTotalHocPhanLabel";
            this.lblTotalHocPhanLabel.Size = new System.Drawing.Size(139, 25);
            this.lblTotalHocPhanLabel.TabIndex = 4;
            this.lblTotalHocPhanLabel.Text = "Tổng học phần";
            // 
            // lblTotalStudentsLabel
            // 
            this.lblTotalStudentsLabel.AutoSize = true;
            this.lblTotalStudentsLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalStudentsLabel.Location = new System.Drawing.Point(60, 30);
            this.lblTotalStudentsLabel.Name = "lblTotalStudentsLabel";
            this.lblTotalStudentsLabel.Size = new System.Drawing.Size(135, 25);
            this.lblTotalStudentsLabel.TabIndex = 3;
            this.lblTotalStudentsLabel.Text = "Tổng sinh viên";
            // 
            // lblTotalUsers
            // 
            this.lblTotalUsers.AutoSize = true;
            this.lblTotalUsers.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalUsers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblTotalUsers.Location = new System.Drawing.Point(805, 70);
            this.lblTotalUsers.Name = "lblTotalUsers";
            this.lblTotalUsers.Size = new System.Drawing.Size(46, 54);
            this.lblTotalUsers.TabIndex = 2;
            this.lblTotalUsers.Text = "0";
            // 
            // lblTotalHocPhan
            // 
            this.lblTotalHocPhan.AutoSize = true;
            this.lblTotalHocPhan.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHocPhan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblTotalHocPhan.Location = new System.Drawing.Point(435, 70);
            this.lblTotalHocPhan.Name = "lblTotalHocPhan";
            this.lblTotalHocPhan.Size = new System.Drawing.Size(46, 54);
            this.lblTotalHocPhan.TabIndex = 1;
            this.lblTotalHocPhan.Text = "0";
            // 
            // lblTotalStudents
            // 
            this.lblTotalStudents.AutoSize = true;
            this.lblTotalStudents.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalStudents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.lblTotalStudents.Location = new System.Drawing.Point(65, 70);
            this.lblTotalStudents.Name = "lblTotalStudents";
            this.lblTotalStudents.Size = new System.Drawing.Size(46, 54);
            this.lblTotalStudents.TabIndex = 0;
            this.lblTotalStudents.Text = "0";
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.White;
            this.panelMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMenu.Controls.Add(this.btnPhanQuyen);
            this.panelMenu.Controls.Add(this.btnQuanLyNguoiDung);
            this.panelMenu.Controls.Add(this.btnQuanLySinhVien);
            this.panelMenu.Location = new System.Drawing.Point(30, 280);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(1140, 200);
            this.panelMenu.TabIndex = 2;
            // 
            // btnPhanQuyen
            // 
            this.btnPhanQuyen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnPhanQuyen.FlatAppearance.BorderSize = 0;
            this.btnPhanQuyen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPhanQuyen.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPhanQuyen.ForeColor = System.Drawing.Color.White;
            this.btnPhanQuyen.Location = new System.Drawing.Point(800, 50);
            this.btnPhanQuyen.Name = "btnPhanQuyen";
            this.btnPhanQuyen.Size = new System.Drawing.Size(300, 100);
            this.btnPhanQuyen.TabIndex = 2;
            this.btnPhanQuyen.Text = "Phân quyền người dùng";
            this.btnPhanQuyen.UseVisualStyleBackColor = false;
            this.btnPhanQuyen.Click += new System.EventHandler(this.btnPhanQuyen_Click_1);
            // 
            // btnQuanLyNguoiDung
            // 
            this.btnQuanLyNguoiDung.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnQuanLyNguoiDung.FlatAppearance.BorderSize = 0;
            this.btnQuanLyNguoiDung.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLyNguoiDung.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuanLyNguoiDung.ForeColor = System.Drawing.Color.White;
            this.btnQuanLyNguoiDung.Location = new System.Drawing.Point(420, 50);
            this.btnQuanLyNguoiDung.Name = "btnQuanLyNguoiDung";
            this.btnQuanLyNguoiDung.Size = new System.Drawing.Size(300, 100);
            this.btnQuanLyNguoiDung.TabIndex = 1;
            this.btnQuanLyNguoiDung.Text = "Quản lý người dùng";
            this.btnQuanLyNguoiDung.UseVisualStyleBackColor = false;
            // 
            // btnQuanLySinhVien
            // 
            this.btnQuanLySinhVien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnQuanLySinhVien.FlatAppearance.BorderSize = 0;
            this.btnQuanLySinhVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuanLySinhVien.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuanLySinhVien.ForeColor = System.Drawing.Color.White;
            this.btnQuanLySinhVien.Location = new System.Drawing.Point(40, 50);
            this.btnQuanLySinhVien.Name = "btnQuanLySinhVien";
            this.btnQuanLySinhVien.Size = new System.Drawing.Size(300, 100);
            this.btnQuanLySinhVien.TabIndex = 0;
            this.btnQuanLySinhVien.Text = "Quản lý sinh viên";
            this.btnQuanLySinhVien.UseVisualStyleBackColor = false;
            this.btnQuanLySinhVien.Click += new System.EventHandler(this.btnQuanLySinhVien_Click_1);
            // 
            // panelUserInfo
            // 
            this.panelUserInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelUserInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUserInfo.Controls.Add(this.lblCurrentRole);
            this.panelUserInfo.Controls.Add(this.lblCurrentUser);
            this.panelUserInfo.Controls.Add(this.btnRefresh);
            this.panelUserInfo.Controls.Add(this.btnLogout);
            this.panelUserInfo.Location = new System.Drawing.Point(30, 510);
            this.panelUserInfo.Name = "panelUserInfo";
            this.panelUserInfo.Size = new System.Drawing.Size(1140, 80);
            this.panelUserInfo.TabIndex = 3;
            // 
            // lblCurrentRole
            // 
            this.lblCurrentRole.AutoSize = true;
            this.lblCurrentRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRole.Location = new System.Drawing.Point(300, 30);
            this.lblCurrentRole.Name = "lblCurrentRole";
            this.lblCurrentRole.Size = new System.Drawing.Size(119, 23);
            this.lblCurrentRole.TabIndex = 3;
            this.lblCurrentRole.Text = "Vai trò: Admin";
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.AutoSize = true;
            this.lblCurrentUser.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentUser.Location = new System.Drawing.Point(40, 30);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(161, 23);
            this.lblCurrentUser.TabIndex = 2;
            this.lblCurrentUser.Text = "Người dùng: Admin";
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(900, 20);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 40);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(1020, 20);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(100, 40);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            // 
            // AdminMainDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1200, 650);
            this.Controls.Add(this.panelUserInfo);
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.panelStats);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AdminMainDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard - Quản lý hệ thống";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelStats.ResumeLayout(false);
            this.panelStats.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.panelUserInfo.ResumeLayout(false);
            this.panelUserInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelStats;
        private System.Windows.Forms.Label lblTotalStudents;
        private System.Windows.Forms.Label lblTotalHocPhan;
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Label lblTotalStudentsLabel;
        private System.Windows.Forms.Label lblTotalHocPhanLabel;
        private System.Windows.Forms.Label lblTotalUsersLabel;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btnQuanLySinhVien;
        private System.Windows.Forms.Button btnQuanLyNguoiDung;
        private System.Windows.Forms.Button btnPhanQuyen;
        private System.Windows.Forms.Panel panelUserInfo;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.Label lblCurrentRole;
    }
}

