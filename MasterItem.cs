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
    public partial class MasterItem : Form
    {
        int id;
        SqlCommand command;
        DataTable table;

        SqlConnection conn = new SqlConnection(Utils.conn);

        public MasterItem()
        {
            InitializeComponent();

            loadGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MainAdmin main = new MainAdmin();
            this.Hide();
            main.ShowDialog();
        }

        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1)
            {
                MessageBox.Show("Field Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        void kosongkan()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            id = 0;
        }

        void loadGrid()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Item", conn);
                table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;

                dataGridView1.Columns[0].Visible = false;

                dataGridView1.Columns[1].HeaderText = "Name";
                dataGridView1.Columns[2].HeaderText = "Request Price";
                dataGridView1.Columns[3].HeaderText = "Compensation Fee";
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    conn.Open();
                    command = new SqlCommand("INSERT INTO Item(Name, RequestPrice, CompensationFee) VALUES(@name, @reqprice, @comp)", conn);
                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.Parameters.AddWithValue("@reqprice", textBox2.Text);
                    command.Parameters.AddWithValue("@comp", textBox3.Text);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    dataGridView1.Update();
                    loadGrid();
                    kosongkan();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    conn.Open();
                    command = new SqlCommand("UPDATE Item SET Name=@name, RequestPrice=@reqprice, CompensationFee=@comp WHERE ID="+id, conn);
                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.Parameters.AddWithValue("@reqprice", textBox2.Text);
                    command.Parameters.AddWithValue("@comp", textBox3.Text);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    dataGridView1.Update();
                    loadGrid();
                    kosongkan();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                command = new SqlCommand("DELETE FROM Item WHERE ID=" + id, conn);
                command.ExecuteNonQuery();
                
                MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
                dataGridView1.Update();
                loadGrid();
                kosongkan();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            kosongkan();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;

            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            loadGrid();
            DataView view = new DataView(table);
            view.RowFilter = string.Format("Name like '%{0}%'", textBox4.Text);
            dataGridView1.DataSource = view;
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            MasterEmployee employee = new MasterEmployee();
            this.Hide();
            employee.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            MasterRoom room = new MasterRoom();
            this.Hide();
            room.ShowDialog();
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            MasterFD master = new MasterFD();
            this.Hide();
            master.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            RoomType roomType = new RoomType();
            this.Hide();
            roomType.ShowDialog();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
