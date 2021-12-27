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
    public partial class ReportGuess : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;

        public ReportGuess()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
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
            foreach(var series in chart1.Series)
            {
                series.Points.Clear();
            }

            command = new SqlCommand("select MONTH, count(*) as num from Reservation  where year = " + dateTimePicker1.Value.ToString("yyyy") + " group by Month order by case when Month = 'January' then 1 when Month = 'February' then 2 when Month = 'March' then 3 when Month = 'April' then 4 when Month = 'May' then 6 when Month = 'June' then 7 when Month = 'July' then 8 when Month = 'August' then 9 when Month = 'September' then 9 when Month = 'October' then 10 when Month = 'November' then 11 when Month = 'December' then 12 else null end", connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    chart1.Series["Total"].Points.AddXY(reader["Month"].ToString(), Convert.ToInt32(reader["num"]));
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
