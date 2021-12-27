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
    public partial class MasterRoom : Form
    {
        int id;
        SqlCommand command;
        DataTable tbl_room;

        SqlConnection conn = new SqlConnection(Utils.conn);
        public MasterRoom()
        {
            InitializeComponent();

            loadGrid();
            loadCombo();
        }
        
        public Boolean val()
        {
            if(comboBox1.SelectedItem == null || textBox2.TextLength < 1)
            {
                MessageBox.Show("Field Must be Filled All!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void loadGrid()
        {
            tbl_room = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room.ID, Room.RoomNumber, Room.RoomFloor, Room.Description, RoomType.Name, RoomType.RoomPrice FROM Room JOIN RoomType on Room.RoomTypeID = RoomType.ID", conn);
            adapter.Fill(tbl_room);

            dataGridView1.DataSource = tbl_room;

            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Room Number";
            dataGridView1.Columns[2].HeaderText = "Room Floor";
            dataGridView1.Columns[3].HeaderText = "Description";
            dataGridView1.Columns[4].HeaderText = "Room Type";
            dataGridView1.Columns[5].HeaderText = "Room Price";
        }

        public void loadCombo()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID, Name FROM RoomType", conn);
                DataTable data = new DataTable();
                adapter.Fill(data);
                comboBox1.DataSource = data;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "ID";
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }

        public void kosongkan()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
        }

        int getNum()
        {
            command = new SqlCommand("select top 1 * from Room order by ID desc", conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            int id = Convert.ToInt32(reader["ID"]);
            conn.Close();

            int plus = id + 1;

            return plus;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainAdmin main = new MainAdmin();
            main.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (val())
            {
                command = new SqlCommand("INSERT INTO Room(RoomTypeID, RoomNumber, RoomFloor, Description, status) VALUES(@roomtypeid," + getNum() + ", @roomfloor, @description, 'avail')", conn);
                command.Parameters.AddWithValue("@roomtypeid", comboBox1.SelectedValue);
                command.Parameters.AddWithValue("@roomfloor", textBox2.Text);
                command.Parameters.AddWithValue("@description", textBox3.Text);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Successfully inserted!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadGrid();
                dataGridView1.Update();
                dataGridView1.Refresh();
                kosongkan();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    conn.Open();
                    command = new SqlCommand("UPDATE Room SET RoomTypeID=@roomtypeid, RoomFloor=@roomfloor, Description=@description WHERE ID = "+id, conn);
                    command.Parameters.AddWithValue("@roomtypeid", comboBox1.SelectedValue);
                    command.Parameters.AddWithValue("@roomfloor", textBox2.Text);
                    command.Parameters.AddWithValue("@description", textBox3.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Successful Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();

                    loadGrid();
                    dataGridView1.Update();
                    dataGridView1.Refresh();
                    kosongkan();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;

            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure to delete it?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if(result == DialogResult.Yes)
                    {
                        conn.Open();

                        command = new SqlCommand("DELETE FROM Room WHERE ID=" + id, conn);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Successfully Deleted", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadGrid();
                        kosongkan();

                        conn.Close();
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            kosongkan();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            loadGrid();
            DataView view = new DataView(tbl_room);
            view.RowFilter = string.Format("RoomNumber like '%{0}%'", textBox4.Text);
            dataGridView1.DataSource = view;
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            MasterEmployee employee = new MasterEmployee();
            this.Hide();
            employee.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            MasterItem item = new MasterItem();
            this.Hide();
            item.ShowDialog();
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            MasterItem item = new MasterItem();
            this.Hide();
            item.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            RoomType roomType = new RoomType();
            this.Hide();
            roomType.ShowDialog();
        }
    }
}
