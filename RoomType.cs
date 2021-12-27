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
    public partial class RoomType : Form
    {
        Image image;
        Bitmap bitmap;
        int id;
        DataTable table;
        SqlCommand command;

        SqlConnection conn = new SqlConnection(Utils.conn);

        public RoomType()
        {
            InitializeComponent();
        }

        public Boolean val()
        {
            if(textBox1.Text.Length < 1 || textBox2.Text.Length < 1 || numericUpDown1.Value < 1 || bitmap == null)
            {
                return false;
                MessageBox.Show("Field Must be Filled");
            }

            return true;
        }

        public void kosongkan()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            pictureBox1.Image = null;
        }

        private void RoomType_Load(object sender, EventArgs e)
        {
            loadGrid();
        }

        public void loadGrid()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM RoomType", conn);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            dataGridView1.Columns[4].Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainAdmin main = new MainAdmin();
            main.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure to Close ?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
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
                    image = Image.FromFile(ofd.FileName);
                    bitmap = (Bitmap)image;
                    pictureBox1.Image = bitmap;

                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    conn.Open();
                    command = new SqlCommand("INSERT INTO RoomType(Name, Capacity, RoomPrice, Photo) values (@name, @capacity, @roomprice, @photo)", conn);

                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[]) converter.ConvertTo(pictureBox1.Image, typeof(byte[]));

                    command.Parameters.AddWithValue("@name", textBox1.Text);
                    command.Parameters.AddWithValue("@capacity", numericUpDown1.Value);
                    command.Parameters.AddWithValue("@roomprice", textBox2.Text);
                    command.Parameters.AddWithValue("@photo", image);

                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Inserting Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    loadGrid();
                    dataGridView1.Update();
                    dataGridView1.Refresh();
                    kosongkan();

                }
                catch(Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    kosongkan();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;

            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            numericUpDown1.Value = (int) dataGridView1.SelectedRows[0].Cells[2].Value;
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            
            byte[] byteImage = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
            MemoryStream ms = new MemoryStream(byteImage);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (val())
            {
                if(id < 1)
                {
                    MessageBox.Show("ID Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                    try
                    { 
                        command = new SqlCommand("UPDATE RoomType SET Name=@name, Capacity=@capacity, RoomPrice=@roomprice, Photo=@photo WHERE ID=" + id + "", conn);
                        command.Parameters.AddWithValue("@name", textBox1.Text);
                        command.Parameters.AddWithValue("@capacity", numericUpDown1.Value);
                        command.Parameters.AddWithValue("@roomprice", textBox2.Text);
                        command.Parameters.AddWithValue("@photo", image);
                        conn.Open();
                        command.ExecuteNonQuery();
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
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            kosongkan();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(id < 1)
            {
                MessageBox.Show("Id must be not 0");
            }
            else
            {
                conn.Open();
                command = new SqlCommand("DELETE FROM RoomType WHERE ID = " + id, conn);
                command.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Successfully Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadGrid();
                dataGridView1.Update();
                dataGridView1.Refresh();
                kosongkan();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            loadGrid();
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

        private void panel4_Click(object sender, EventArgs e)
        {
            MasterFD master = new MasterFD();
            this.Hide();
            master.ShowDialog();
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
