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
    public partial class MasterEmployee : Form
    {
        int id;
        SqlCommand command;
        DataTable tbl_emp;

        SqlConnection conn = new SqlConnection(Utils.conn);

        public MasterEmployee()
        {
            InitializeComponent();

            loadCombo();
            loadgrid();
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

        public Boolean val()
        {
            if(textbox_username.TextLength < 1 || textbox_pass.TextLength < 1 || textbox_conf.TextLength < 1 || textbox_name.TextLength < 1 || textbox_email.TextLength < 1 || textbox_address.TextLength < 1 || pictureBox1.Image == null)
            {
                MessageBox.Show("Field Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            if(textbox_conf.Text != textbox_pass.Text)
            {
                MessageBox.Show("Confirm Password doesn't same with password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(textbox_pass.TextLength < 8 || textbox_conf.TextLength < 8)
            {
                MessageBox.Show("Password Must be 8 Characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            command = new SqlCommand("select Username from Employee where username = '" + textbox_username.Text + "'", conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                MessageBox.Show("Username Already In Used", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
                return false;
            }
            conn.Close();

            return true;
        }

        public void loadgrid()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Employee JOIN Job ON Employee.JobId = Job.ID", conn);
                tbl_emp = new DataTable();
                adapter.Fill(tbl_emp);
                dataGridView1.DataSource = tbl_emp;

                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;

                dataGridView1.Columns[1].HeaderText = "Username";
                dataGridView1.Columns[3].HeaderText = "Name";
                dataGridView1.Columns[4].HeaderText = "Email";
                dataGridView1.Columns[5].HeaderText = "Address";
                dataGridView1.Columns[6].HeaderText = "Date Of Birth";
                dataGridView1.Columns[10].HeaderText = "Job";

                dataGridView1.Columns[6].DefaultCellStyle.Format = "MM/dd/yyyy";
            }
            catch (Exception e)
            {

                MessageBox.Show(Convert.ToString(e));
            }
        }

        public void loadCombo()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Job", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                comboBox1.DataSource = table;
                comboBox1.DisplayMember = "Name";
                comboBox1.ValueMember = "ID";
            }
            catch(Exception e)
            {
                
            }

        }

        public void kosongkan()
        {
            textbox_username.Text = "";
            textbox_name.Text = "";
            textbox_pass.Text = "";
            textbox_conf.Text = "";
            textbox_email.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            textbox_address.Text = "";
            comboBox1.Text = "";
            pictureBox1.Image = null; 
        }

        bool val_update()
        {
            if (textbox_username.TextLength < 1 || textbox_name.TextLength < 1 || textbox_email.TextLength < 1 || textbox_address.TextLength < 1 || pictureBox1.Image == null)
            {
                MessageBox.Show("Field Must be Filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (textbox_conf.Text != textbox_pass.Text)
            {
                MessageBox.Show("Confirm Password doesn't same with password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (val())
            {
                try
                {
                    conn.Open();
                    ImageConverter converter = new ImageConverter();
                    byte[] image = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                    String pass = Encrypt.encryptPass(textbox_pass.Text);

                    command = new SqlCommand("INSERT INTO Employee(Username, Password, Name, Email, Address, DateOfBirth, JobId, Photo) VALUES(@username, @password, @name, @email, @address, @dob, @job, @photo)", conn);
                    command.Parameters.AddWithValue("@username", textbox_name.Text);
                    command.Parameters.AddWithValue("@password", pass);
                    command.Parameters.AddWithValue("@name", textbox_name.Text);
                    command.Parameters.AddWithValue("@email", textbox_email.Text);
                    command.Parameters.AddWithValue("@address", textbox_address.Text);
                    command.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@job", comboBox1.SelectedValue);
                    command.Parameters.AddWithValue("@photo", image);

                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    command.ExecuteNonQuery();
                    kosongkan();
                    loadgrid();
                    conn.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (val_update())
            {
                DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if(result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        ImageConverter converter = new ImageConverter();
                        byte[] image = (byte[])converter.ConvertTo(pictureBox1.Image, typeof(byte[]));
                        String pass = Encrypt.encryptPass(textbox_pass.Text);

                        command = new SqlCommand("UPDATE Employee SET Username=@username, Name=@name, Email=@email, Address=@address, DateOfBirth=@dob, JobId=@job, Photo=@photo WHERE ID ="+id, conn);
                        command.Parameters.AddWithValue("@username", textbox_username.Text);
                        command.Parameters.AddWithValue("@name", textbox_name.Text);
                        command.Parameters.AddWithValue("@email", textbox_email.Text);
                        command.Parameters.AddWithValue("@address", textbox_address.Text);
                        command.Parameters.AddWithValue("@dob", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@job", comboBox1.SelectedValue);
                        command.Parameters.AddWithValue("@photo", image);

                        MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        command.ExecuteNonQuery();
                        dataGridView1.Update();
                        kosongkan();
                        loadgrid();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(Convert.ToString(ex));
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textbox_pass.Enabled = false;
            textbox_conf.Enabled = false;

            try
            {
                dataGridView1.CurrentRow.Selected = true;

                textbox_username.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textbox_name.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textbox_email.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                textbox_address.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                comboBox1.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                comboBox1.SelectedValue = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
                dateTimePicker1.Value = (DateTime)dataGridView1.SelectedRows[0].Cells[6].Value;

                id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                byte[] image64 = (byte[])dataGridView1.SelectedRows[0].Cells[8].Value;
                MemoryStream ms = new MemoryStream(image64);
                pictureBox1.Image = Image.FromStream(ms);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    conn.Open();
                    command = new SqlCommand("DELETE FROM Employee WHERE ID=" + id, conn);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    dataGridView1.Update();
                    loadgrid();
                    kosongkan();
                    conn.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            kosongkan();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textbox_pass.PasswordChar = '\0';
                textbox_conf.PasswordChar = '\0';
            }
            else if (checkBox1.Checked == false)
            {
                textbox_pass.PasswordChar = '*';
                textbox_conf.PasswordChar = '*';
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
            DataView view = new DataView(tbl_emp);
            view.RowFilter = string.Format("Name like '%{0}%'", textBox1.Text);
            dataGridView1.DataSource = view;
        }

        private void panel3_Click(object sender, EventArgs e)
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

        private void textbox_username_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textbox_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
