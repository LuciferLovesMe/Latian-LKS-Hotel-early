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
    public partial class CheckIn : Form
    {
        SqlCommand command;

        SqlConnection conn = new SqlConnection(Utils.conn);
        int id_customer;
        public CheckIn()
        {
            InitializeComponent();

            loadcombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainFrontOffice frontOffice = new MainFrontOffice();
            frontOffice.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loadcombo()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Reservation", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "BookingCode";
        }

        void loadcust()
        {
            conn.Open();
            command = new SqlCommand("SELECT * FROM Customer WHERE ID = " + dataGridView1.Rows[0].Cells[0].Value, conn);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                id_customer = Convert.ToInt32(reader["ID"]);
                textBox1.Text = reader["Name"].ToString();
                textBox2.Text = reader["Email"].ToString();
                textBox3.Text = reader["NIK"].ToString();
                textBox5.Text = reader["phone_number"].ToString();
                if(reader["Gender"].ToString() == "M")
                {
                    mradio.Checked = true;
                    fradio.Checked = false;
                }
                else
                {
                    fradio.Checked = true;
                    mradio.Checked = false;
                }
                textBox4.Text = reader["Age"].ToString();
                textBox4.ReadOnly = true;
                dateTimePicker1.Value = Convert.ToDateTime(reader["DateOfBirth"]);
                conn.Close();
            }
        }
        
        void kosongkan()
        {
            comboBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            mradio.Checked = false;
            fradio.Checked = false;
            textBox4.Text = "";
            textBox5.Text = "";
            dateTimePicker1.CustomFormat = " ";

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }

        bool val()
        {
            if (textBox1.TextLength < 1 && textBox2.TextLength < 1 && textBox4.TextLength < 1 && textBox5.TextLength < 1)
            {
                MessageBox.Show("Fill the field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (textBox3.TextLength != 16)
            {
                MessageBox.Show("NIK Must be 16 Characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(fradio.Checked && mradio.Checked)
            {
                MessageBox.Show("Fill the GENDER field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text.Length > 0)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Reservation.CustomerID, Reservation.ID, ReservationRoom.StartDateTime, Room.RoomNumber, Room.RoomFloor, RoomType.Name, RoomType.Capacity, RoomType.RoomPrice FROM Reservation JOIN ReservationRoom ON Reservation.ID = ReservationRoom.ReservationID JOIN Room on ReservationRoom.RoomID = Room.ID JOIN RoomType ON Room.RoomTypeID = RoomType.ID WHERE Reservation.BookingCode LIKE '%"+comboBox1.Text+"%'", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                loadcust();
            }
            else
            {
                MessageBox.Show("Fill Booking Code");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            kosongkan();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var now = DateTime.Now.ToString("yyyy-MM-dd HH:m:s");
                command = new SqlCommand("UPDATE ReservationRoom SET CheckInDateTime = '" + now + "' WHERE ID = " + dataGridView1.Rows[0].Cells[1].Value, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Success");
                kosongkan();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime.Now.ToString("yyyy");

            int umur = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
            textBox4.Text = umur.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (val())
            {
                command = new SqlCommand("update Customer set Name = '" + textBox1.Text + "',NIK='" + Convert.ToInt64(textBox3.Text) + "',Email='" + textBox2.Text + "',phone_number='" + textBox5.Text + "', DateOfBirth='" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' where id = "+id_customer, conn);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    kosongkan();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            ReservationForm reservation = new ReservationForm();
            this.Hide();
            reservation.ShowDialog();
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
