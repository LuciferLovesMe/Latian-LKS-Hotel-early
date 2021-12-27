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
    public partial class Reservation : Form
    {
        int id, id_req, id_cust;
        SqlCommand command;
        DataTable tbl_cust, tbl2;

        SqlConnection conn = new SqlConnection(Utils.conn);
        public Reservation()
        {
            InitializeComponent();

            checkin.Value = DateTime.Now;
            checkout.Value = DateTime.Now;
            textprice.Enabled = false;
            textsub.Enabled = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            labelcode.Text = getCode();

            loadtype();
            loadCustomer();
            loadItems();
            getCode();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MainFrontOffice main = new MainFrontOffice();
            this.Hide();
            main.ShowDialog();
        }

        void loadtype()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM RoomType", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "Name";
            comboitems.ValueMember = "ID";
        }

        void loadCustomer()
        {
            tbl_cust = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Customer", conn);
            adapter.Fill(tbl_cust);

            dgvcust.DataSource = tbl_cust;
        }

        void loadavail()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room.*, RoomType.RoomPrice FROM Room JOIN RoomType ON Room.RoomTypeID = RoomType.ID WHERE RoomType.Name LIKE '%" + comboBox1.Text + "%' AND Room.ID NOT IN (SELECT ReservationRoom.RoomID FROM ReservationRoom)", conn);
            tbl2 = new DataTable();
            adapter.Fill(tbl2);

            dataGridView1.DataSource = tbl2;
        }

        void loadItems()
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
            command = new SqlCommand("SELECT * FROM Item WHERE Name LIKE '%" + comboitems.Text + "%'", conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                id_req = Convert.ToInt32(reader["id"]);
                textprice.Text = Convert.ToString(reader["RequestPrice"]);
                textsub.Text = Convert.ToString(Convert.ToInt32(reader["RequestPrice"]) * numericUpDown1.Value);
            }
            conn.Close();
        }

        String getCode()
        {
            command = new SqlCommand("SELECT TOP 1 * FROM Reservation ORDER BY ID DESC", conn);
            conn.Open();
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
            conn.Close();
            book = "BK00" + i.ToString();
            return book;
        }

        String gettgl(DateTimePicker date)
        {
            return date.Value.ToString("yyyy-MM-dd HH:m:s");
        }

        bool val()
        {
            if(id_cust == 0 && dataGridView2.RowCount < 1 && textstay.TextLength < 0)
            {
                MessageBox.Show("Field Must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            else if(checkin.Value > checkout.Value)
            {
                MessageBox.Show("CheckIn date Must less than CheckOut date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        void clear()
        {
            id_cust = 0;
            id_req = 0;
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dgvItems.Rows.Clear();
            labelcode.Text = getCode();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadavail();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            loadCustomer();
            DataView view = new DataView(tbl_cust);
            view.RowFilter = string.Format("Name like '%{0}%'", search_cust.Text);
            dgvcust.DataSource = view;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddCustomer add = new AddCustomer();
            this.Hide();
            add.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadavail();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboitems.Text == "" || numericUpDown1.Value < 0)
            {
                MessageBox.Show("Fill", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DataGridViewRow row = (DataGridViewRow)dgvItems.Rows[0].Clone();
            row.Cells[0].Value = id_req;
            row.Cells[1].Value = comboitems.Text;
            row.Cells[2].Value = textsub.Text;
            dgvItems.Rows.Add(row);
        }

        private void comboitems_KeyPress(object sender, KeyPressEventArgs e)
        {
            countsub();
        }

        private void comboitems_SelectedIndexChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(val())
            {
                var now = DateTime.Now.ToString("yyyy-MM-dd HH:m:s");
                command = new SqlCommand("INSERT INTO Reservation VALUES('"+ now + "', " + Model.ID + ", " + id_cust + ", '" + getCode() + "', '"+checkin.Value.ToString("MMMM")+"', '"+ checkin.Value.ToString("yyyy") + "')", conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                conn.Open();
                SqlCommand com = new SqlCommand("SELECT TOP (1) ID FROM Reservation ORDER BY ID DESC", conn);
                SqlDataReader read = com.ExecuteReader();
                read.Read();
                var res_id = read["ID"];
                conn.Close();

                for(int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    conn.Open();
                    command = new SqlCommand("INSERT INTO ReservationRoom VALUES(" + Convert.ToInt32(res_id) + "," + Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value) + ", '" + now + "', " + Convert.ToInt32(textstay.Text) + ", " + Convert.ToInt32(dataGridView2.Rows[i].Cells[5].Value) * Convert.ToInt32(textstay.Text) + ",'" + gettgl(checkin) + "','" + gettgl(checkout) + "')", conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }

                if(dgvItems.RowCount > 1)
                {
                    conn.Open();
                    SqlCommand com1 = new SqlCommand("SELECT TOP (1) ID FROM ReservationRoom ORDER BY ID DESC", conn);
                    SqlDataReader read1 = com1.ExecuteReader();
                    read1.Read();
                    var reser_id = read1["ID"];
                    conn.Close();
                    for (int i = 0; i < dgvItems.RowCount - 1; i++)
                    {
                        conn.Open();
                        command = new SqlCommand("INSERT INTO ReservationRequestItem VALUES(" + Convert.ToInt32(reser_id) + "," + dgvItems.Rows[i].Cells[0].Value + "," + Convert.ToInt32(numericUpDown1.Value) + "," + Convert.ToInt32(textsub.Text) + ")", conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                MessageBox.Show("Succes", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
            }
        }

        private void textstay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)){
                e.Handled = true;
            }
        }

        private void dgvcust_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvcust.CurrentRow.Selected = true;
            id_cust = Convert.ToInt32(dgvcust.SelectedRows[0].Cells[0].Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
            row.Cells[0].Value = textBox1.Text;
            row.Cells[1].Value = textBox2.Text;
            row.Cells[2].Value = textBox3.Text;
            row.Cells[3].Value = textBox4.Text;
            row.Cells[4].Value = textBox5.Text;
            row.Cells[5].Value = textBox6.Text;
            dataGridView2.Rows.Add(row);
        }
    }
}
