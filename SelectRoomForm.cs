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
    public partial class SelectRoomForm : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;

        public SelectRoomForm()
        {
            InitializeComponent();
            loadroom();

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            lbl_code.Text = getCode();
        }

        void loadroom()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from RoomType", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
        }

        void loadroomgrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select Room.*, RoomType.RoomPrice from Room join RoomType on Room.RoomTypeID = RoomType.ID where Room.status = 'avail' and RoomType.Name = '"+comboBox1.Text+"'", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            dataGridView1.DataSource = table;
        }

        String getCode()
        {
            command = new SqlCommand("SELECT TOP 1 * FROM Reservation ORDER BY ID DESC", connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            String book = "";
            var i = 0;
            if (!reader.HasRows)
            {
                i = 1;
            }
            else
            {
                i = Convert.ToInt32(reader["ID"]) + 1;
            }
            connection.Close();
            book = "BK" + i.ToString();
            return book;
        }

        String gettgl(DateTimePicker date)
        {
            return date.Value.ToString("yyyy-MM-dd HH:m:s");
        }

        void clear()
        {
            UserSelected.user_id = 0;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            lbl_code.Text = getCode();
            text_stay.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        bool val()
        {
            if(dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Check In date must be less than Check Out date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (text_stay.TextLength < 1)
            {
                MessageBox.Show("Staying Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            UserSelected.user_id = 0;
            UserSelected.user_name = "";

            ReservationForm form = new ReservationForm();
            this.Hide();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadroomgrid();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadroomgrid();

            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;

            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" && textBox2.Text == "" && textBox5.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox6.Text == "")
            {
                MessageBox.Show("Room Must be Selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int rowCount = dataGridView2.Rows.Add();
                dataGridView2.Rows[rowCount].Cells[0].Value = textBox1.Text;
                dataGridView2.Rows[rowCount].Cells[1].Value = textBox1.Text;
                dataGridView2.Rows[rowCount].Cells[2].Value = textBox1.Text;
                dataGridView2.Rows[rowCount].Cells[3].Value = textBox1.Text;
                dataGridView2.Rows[rowCount].Cells[4].Value = textBox1.Text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                var now = DateTime.Now.ToString("yyyy-MM-dd HH:m:s");
                command = new SqlCommand("INSERT INTO Reservation VALUES('" + now + "', " + Model.ID + ", " + UserSelected.user_id + ", '" + getCode() + "', '" + dateTimePicker1.Value.ToString("MMMM") + "', '" + dateTimePicker2.Value.ToString("yyyy") + "')", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                connection.Open();
                SqlCommand com = new SqlCommand("SELECT TOP (1) ID FROM Reservation ORDER BY ID DESC", connection);
                SqlDataReader read = com.ExecuteReader();
                read.Read();
                var res_id = read["ID"];
                connection.Close();

                for(int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    connection.Open();
                    command = new SqlCommand("INSERT INTO ReservationRoom VALUES(" + Convert.ToInt32(res_id) + "," + Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value) + ", '" + now + "', " + Convert.ToInt32(text_stay.Text) + ", " + Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value) * Convert.ToInt32(text_stay.Text) + ",'" + gettgl(dateTimePicker1) + "','" + gettgl(dateTimePicker2) + "')", connection);
                    SqlCommand sql = new SqlCommand("update Room set status='unavail' where ID = " + Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value), connection);
                    sql.ExecuteNonQuery();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                MessageBox.Show("Succes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();

                RequestItem req = new RequestItem();
                this.Hide();
                req.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
