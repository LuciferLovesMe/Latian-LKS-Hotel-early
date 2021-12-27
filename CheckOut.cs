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
    public partial class CheckOut : Form
    {
        SqlCommand command;
        int totalitems = 0, id_reser, id_req;
        double totalfd = 0;
        String now = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");

        SqlConnection conn = new SqlConnection(Utils.conn);
        public CheckOut()
        {
            //lbltotal.Text = Convert.ToString(totalfd + totalitems);

            InitializeComponent();
            loadcombo();
            //loadItems();
            loadstatus();
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
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        void loadcombo()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room.RoomNumber FROM Room WHERE status = 'unavail'", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "RoomNumber";
        }
        
        void sumitems()
        {
            totalitems = 0;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                totalitems += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
            }

            lbltotalitem.Text = totalitems.ToString();
            lbltotal.Text = Convert.ToString(totalfd + totalitems);
        }

        void countsub()
        {
            conn.Open();
            command = new SqlCommand("select * from item where name like '%" + comboitems.Text + "%'", conn);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                id_req = Convert.ToInt32(reader["id"]);
                textsub.Text = Convert.ToString(Convert.ToInt32(reader["requestprice"]) * numericUpDown1.Value);
                textfee.Text = Convert.ToString(Convert.ToInt32(reader["compensationfee"]));
            }
            conn.Close();
        }

        void loadItems()
        {
            numericUpDown1.Value = 0;
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT ReservationRequestItem.*, Item.Name FROM ReservationRequestItem join Item on ReservationRequestItem.ItemID = Item.ID where ReservationRoomID = " + id_reser, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            command = new SqlCommand("select ReservationRequestItem.Qty from ReservationRequestItem where ReservationRoomID = "+id_reser, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                numericUpDown1.Value = Convert.ToDecimal(reader["Qty"]);
            }
            conn.Close();

            comboitems.DataSource = table;
            comboitems.ValueMember = "ItemID";
            comboitems.DisplayMember = "Name";

            sumitems();
        }
        void sumfd()
        {
            totalfd = 0;
            for (int i = 0; i < dataGridView2.RowCount - 1; i++)
            {
                totalfd += Convert.ToDouble(dataGridView2.Rows[i].Cells[4].Value) + Convert.ToDouble(dataGridView2.Rows[i].Cells[5].Value);
            }

            lbltotalfd.Text = totalfd.ToString();
            lbltotal.Text = Convert.ToString(totalfd + totalitems);
        }

        void loadstatus()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM ItemStatus", conn);
            DataTable table = new DataTable();
            adapter.Fill(table);

            combostatus.DataSource = table;
            combostatus.DisplayMember = "Name";
            combostatus.ValueMember = "ID";
        }

        void clear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            comboitems.Text = "";
            comboitems.Text = "";
            textfee.Text = "";
            textsub.Text = "";
            totalitems = 0;
            numericUpDown1.Value = 0;
        }

        private void comboitems_SelectedIndexChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0 && id_reser != 0)
            {
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {

                    command = new SqlCommand("INSERT INTO ReservationCheckOut VALUES (" + id_reser + "," + Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value) + ", " + Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) + ", " + Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value) + ", " + Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value) + ")", conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }
            if (id_reser != 0)
            {
                SqlCommand sql = new SqlCommand("update Room set status = 'avail' where RoomNumber = " + Convert.ToInt32(comboBox1.Text), conn);
                SqlCommand comm = new SqlCommand("UPDATE ReservationRoom SET CheckOutDateTime = '" + now + "' WHERE ID = " + id_reser, conn);
                
                conn.Open();
                sql.ExecuteNonQuery();
                comm.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Success");
                clear();
                loadcombo();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            countsub();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadItems();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = id_req;
            row.Cells[1].Value = comboitems.Text;
            row.Cells[2].Value = numericUpDown1.Value.ToString();
            row.Cells[3].Value = textsub.Text;
            row.Cells[4].Value = combostatus.SelectedValue.ToString();
            if(Convert.ToInt32(combostatus.SelectedValue) != 1)
            {
                row.Cells[5].Value = Convert.ToInt32(textfee.Text) * numericUpDown1.Value;
            }
            else
            {
                row.Cells[5].Value = 0;
            }
            dataGridView1.Rows.Add(row);

            sumitems();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            command = new SqlCommand("SELECT ReservationRoom.ID FROM ReservationRoom JOIN Room ON Room.ID = ReservationRoom.RoomID WHERE Room.RoomNumber =" + comboBox1.Text, conn);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            id_reser = Convert.ToInt32(reader["ID"]);
            conn.Close();

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT FDCheckOut.*, FoodsAndDrinks.Name FROM FDCheckOut JOIN FoodsAndDrinks ON FDCheckOut.FDID = FoodsAndDrinks.ID WHERE FDCheckOut.ReservationRoomID = " + id_reser, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView2.DataSource = table;

            loadItems();
            sumfd();
            sumitems();
        }
    }
}
