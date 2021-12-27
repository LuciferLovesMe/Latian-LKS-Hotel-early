using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class MasterFD : Form
    {
        int id;
        SqlCommand command;
        DataTable table;

        SqlConnection conn = new SqlConnection(Utils.conn);
        public MasterFD()
        {
            InitializeComponent();

            loadgrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you Sure?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MainAdmin admin = new MainAdmin();
            this.Hide();
            admin.ShowDialog();
        }

        bool val()
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1 || comboBox1.Text == null || pictureBox1.Image == null)
            {
                MessageBox.Show("Field must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        void loadgrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM FoodsAndDrinks", conn);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Name";
            dataGridView1.Columns[2].HeaderText = "Type";
            dataGridView1.Columns[3].HeaderText = "Price";
        }

        void kosongkan()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            pictureBox1.Image = null;
            id = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    ImageConverter converter = new ImageConverter();
                    byte[] byteImg = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                    conn.Open();
                    command = new SqlCommand("INSERT INTO FoodsAndDrinks(Name, Type, Price, Photo) VALUES (@name, @type, @price, @photo)", conn);
                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.Parameters.AddWithValue("@type", comboBox1.Text);
                    command.Parameters.AddWithValue("@price", textBox2.Text);
                    command.Parameters.AddWithValue("@photo", byteImg);

                    command.ExecuteNonQuery();
                    conn.Close();
                    loadgrid();
                    kosongkan();
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp;";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Image image = Image.FromFile(ofd.FileName);
                    Bitmap bitmap = (Bitmap)image;
                    pictureBox1.Image = bitmap;

                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.CurrentRow.Selected = true;

                id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                if(dataGridView1.SelectedRows[0].Cells[2].Value.ToString().ToLower() == "d")
                {
                    comboBox1.Text = "Drink";
                }
                else if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString().ToLower() == "f")
                {
                    comboBox1.Text = "Food";
                }

                byte[] byteImage = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
                MemoryStream ms = new MemoryStream(byteImage);
                pictureBox1.Image = Image.FromStream(ms);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    conn.Open();
                    ImageConverter converter = new ImageConverter();
                    byte[] byteImg = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                    char comb = comboBox1.Text[0];

                    command = new SqlCommand("UPDATE FoodsAndDrinks SET Name=@name, Type=@type, Price=@price WHERE ID=" + id, conn); 
                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.Parameters.AddWithValue("@type", comb);
                    command.Parameters.AddWithValue("@price", textBox2.Text);
                    command.Parameters.AddWithValue("@photo", byteImg);
                
                    command.ExecuteNonQuery();
                    conn.Close();
                    kosongkan();
                    loadgrid();
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                command = new SqlCommand("DELETE FROM FoodsAndDrinks WHERE ID=" + id, conn);
                command.ExecuteNonQuery();

                conn.Close();
                loadgrid();
                kosongkan();
                MessageBox.Show("Success", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            kosongkan();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
            DataView view = new DataView(table);
            view.RowFilter = string.Format("Name like '%{0}%'", textBox3.Text);
            dataGridView1.DataSource = view;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            MasterEmployee employee = new MasterEmployee();
            this.Hide();
            employee.ShowDialog();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            MasterRoom room = new MasterRoom();
            this.Hide();
            room.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            MasterItem item = new MasterItem();
            this.Hide();
            item.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
