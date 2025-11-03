using System;
using System.Windows.Forms;

namespace QL_HP_DT_SV.Admin
{
    public partial class SearchStudentForm : Form
    {
        public string SearchKeyword { get; private set; } = "";

        public SearchStudentForm()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchKeyword = txtKeyword.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SearchKeyword = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }
    }
}

