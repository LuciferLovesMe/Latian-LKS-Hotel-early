using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class ReservationForm : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        DataTable table;

        public ReservationForm()
        {
            InitializeComponent();
            loaddata();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loaddata()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Customer", connection);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UserSelected.user_id = 0;
            UserSelected.user_name = "";

            dataGridView1.CurrentRow.Selected = true;

            UserSelected.user_id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            UserSelected.user_name = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(UserSelected.user_id == 0)
            {
                MessageBox.Show("Please Select a Customer !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SelectRoomForm form = new SelectRoomForm();
                this.Hide();
                form.ShowDialog();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loaddata();
            DataView view = new DataView(table);
            view.RowFilter = string.Format("Name like '%{0}%'", textBox1.Text);
            dataGridView1.DataSource = view;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddCustomer add = new AddCustomer();
            this.Hide();
            add.ShowDialog();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            CheckIn check = new CheckIn();
            this.Hide();
            check.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            ReportCheckIn report = new ReportCheckIn();
            this.Hide();
            report.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            RequestItem req = new RequestItem();
            this.Hide();
            req.ShowDialog();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            ReportGuess report = new ReportGuess();
            this.Hide();
            report.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            CheckOut check = new CheckOut();
            this.Hide();
            check.ShowDialog();
        }
    }
}
