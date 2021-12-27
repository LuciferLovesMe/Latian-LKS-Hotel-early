using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Latian_LKS_2
{
    public partial class ReportCheckIn : Form
    {
        SqlCommand command;

        SqlConnection conn = new SqlConnection(Utils.conn);
        public ReportCheckIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrontOffice office = new MainFrontOffice();
            this.Hide();
            office.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            ReservationForm reservation = new ReservationForm();
            this.Hide();
            reservation.ShowDialog();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            CheckIn check = new CheckIn();
            this.Hide();
            check.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            RequestItem req = new RequestItem();
            this.Hide();
            req.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            CheckOut check = new CheckOut();
            this.Hide();
            check.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            ReportCheckIn report = new ReportCheckIn();
            this.Hide();
            report.ShowDialog();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            ReportGuess report = new ReportGuess();
            this.Hide();
            report.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dateTimePicker2.Value > dateTimePicker1.Value)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from Room join RoomType on RoomType.ID = Room.RoomTypeID join ReservationRoom on ReservationRoom.RoomID = Room.ID join Reservation on Reservation.ID = ReservationRoom.ReservationID where ReservationRoom.CheckInDateTime >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:m:ss") + "' AND ReservationRoom.CheckOutDateTime <= '"+dateTimePicker2.Value.ToString("yyyy-MM-dd HH:m:ss") + "'", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[18].Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount > 0)
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Application.Workbooks.Add(Type.Missing);
                for(int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    excel.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;

                }
                for(int i = 0; i<dataGridView1.Rows.Count; i++)
                {
                    for(int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        excel.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                excel.Columns.AutoFit();
                excel.Visible = true;
            }
        }
    }
}
