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
    public partial class RequestItem : Form
    {
        SqlCommand command;
        int id_req, total;

        SqlConnection conn = new SqlConnection(Utils.conn);
        public RequestItem()
        {
            InitializeComponent();

            loadcombo();
            loaditems();
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainFrontOffice main = new MainFrontOffice();
            main.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loadcombo()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room.RoomNumber FROM Room WHERE Room.status = 'unavail'", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "RoomNumber";
        }

        void loaditems()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Item", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            comboitems.DataSource = table;
            comboitems.DisplayMember = "Name";
            comboitems.ValueMember = "ID";
        }

        void countsub()
        {
            conn.Open();
            command = new SqlCommand("SELECT * FROM Item WHERE Name LIKE '%" + comboitems.Text + "%'", conn);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                id_req = Convert.ToInt32(reader["id"]);
                textprice.Text = Convert.ToString(reader["RequestPrice"]);
                textsub.Text = Convert.ToString(Convert.ToInt32(reader["RequestPrice"]) * numericUpDown1.Value);
            }
            conn.Close();
        }

        void gettotal()
        {
            for(int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                total += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
            }

            lbltotal.Text = total.ToString();
        }

        void kosongkan()
        {
            comboBox1.Text = "";
            comboitems.Text = "";
            numericUpDown1.Value = 0;
            textprice.Text = "";
            textsub.Text = "";
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void comboitems_SelectedIndexChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            command = new SqlCommand("SELECT ReservationRoom.ID FROM ReservationRoom join Room on ReservationRoom.RoomID = Room.ID where Room.status = 'unavail' and Room.RoomNumber = " + comboBox1.Text, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            var id_res = reader["ID"];
            conn.Close();
            for(int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                try
                {
                    command = new SqlCommand("INSERT INTO ReservationRequestItem VALUES(" + Convert.ToInt32(id_res) + "," + dataGridView1.Rows[i].Cells[0].Value + "," + Convert.ToInt32(numericUpDown1.Value) + "," + Convert.ToInt32(textsub.Text) + ")", conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    kosongkan();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                MessageBox.Show("Success", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lbltotal.Text = "0";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            int rowCount = dataGridView1.Rows.Add();
            dataGridView1.Rows[rowCount].Cells[0].Value = id_req;
            dataGridView1.Rows[rowCount].Cells[1].Value = comboitems.Text;
            dataGridView1.Rows[rowCount].Cells[2].Value = numericUpDown1.Value.ToString();
            dataGridView1.Rows[rowCount].Cells[3].Value = textsub.Text; 

            gettotal();
        }
    }
}
