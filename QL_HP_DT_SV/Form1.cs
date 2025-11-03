using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Kết nối đến sql server 
using System.Data.SqlClient;
namespace QL_HP_DT_SV
{
    public partial class Form1 : Form
    {
        //Chuỗi kết nối đến sql server
        string connectionString = "Data Source=LAPTOP-F1I2GP2R;Initial Catalog=QL_HP_DT_SV;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void B_TK_Click(object sender, EventArgs e)
        {
            //Kết nối đến csdl sql server
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Kết nối SQL Server thành công!");

                    // Ví dụ đọc dữ liệu
                    string query = "SELECT * FROM SinhVien";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    DGV_TTSV.DataSource = dt; 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                }
            }
        }
    }
}
