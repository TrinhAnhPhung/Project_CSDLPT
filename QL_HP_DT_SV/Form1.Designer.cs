namespace QL_HP_DT_SV
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_MSSV = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DGV_TTSV = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_K = new System.Windows.Forms.TextBox();
            this.B_TK = new System.Windows.Forms.Button();
            this.CB_DKTN = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_TTSV)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(607, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chương trình quản lý học phần và điểm thi sinh viên";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TB_MSSV
            // 
            this.TB_MSSV.Location = new System.Drawing.Point(151, 104);
            this.TB_MSSV.Name = "TB_MSSV";
            this.TB_MSSV.Size = new System.Drawing.Size(162, 22);
            this.TB_MSSV.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mã Sinh Viên:";
            // 
            // DGV_TTSV
            // 
            this.DGV_TTSV.AllowUserToOrderColumns = true;
            this.DGV_TTSV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DGV_TTSV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_TTSV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_TTSV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_TTSV.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DGV_TTSV.Location = new System.Drawing.Point(1, 199);
            this.DGV_TTSV.Name = "DGV_TTSV";
            this.DGV_TTSV.RowHeadersWidth = 51;
            this.DGV_TTSV.RowTemplate.Height = 24;
            this.DGV_TTSV.Size = new System.Drawing.Size(728, 370);
            this.DGV_TTSV.TabIndex = 4;
            this.DGV_TTSV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(29, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Khoa:";
            // 
            // TB_K
            // 
            this.TB_K.Location = new System.Drawing.Point(82, 149);
            this.TB_K.Name = "TB_K";
            this.TB_K.Size = new System.Drawing.Size(162, 22);
            this.TB_K.TabIndex = 6;
            // 
            // B_TK
            // 
            this.B_TK.BackColor = System.Drawing.Color.Chartreuse;
            this.B_TK.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_TK.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.B_TK.Location = new System.Drawing.Point(570, 104);
            this.B_TK.Name = "B_TK";
            this.B_TK.Size = new System.Drawing.Size(114, 52);
            this.B_TK.TabIndex = 7;
            this.B_TK.Text = "Tìm kiếm";
            this.B_TK.UseVisualStyleBackColor = false;
            this.B_TK.Click += new System.EventHandler(this.B_TK_Click);
            // 
            // CB_DKTN
            // 
            this.CB_DKTN.AutoSize = true;
            this.CB_DKTN.Location = new System.Drawing.Point(151, 78);
            this.CB_DKTN.Name = "CB_DKTN";
            this.CB_DKTN.Size = new System.Drawing.Size(163, 20);
            this.CB_DKTN.TabIndex = 8;
            this.CB_DKTN.Text = "Đủ điều kiện tốt nghiệp";
            this.CB_DKTN.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1298, 571);
            this.Controls.Add(this.CB_DKTN);
            this.Controls.Add(this.B_TK);
            this.Controls.Add(this.TB_K);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DGV_TTSV);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TB_MSSV);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.DGV_TTSV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_MSSV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView DGV_TTSV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_K;
        private System.Windows.Forms.Button B_TK;
        private System.Windows.Forms.CheckBox CB_DKTN;
    }
}

