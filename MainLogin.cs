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
    public partial class MainLogin : Form
    {
        public MainLogin()
        {
            InitializeComponent();
        }

        void doLogin()
        {
            if (val())
            {
                SqlConnection conn = new SqlConnection(Utils.conn);
                try
                {
                    conn.Open();

                    String pass = Encrypt.encryptPass(textBox2.Text);

                    SqlCommand command = new SqlCommand("SELECT * FROM Employee WHERE Username=@Username AND Password=@Password", conn);
                    command.Parameters.Add("@Username", SqlDbType.VarChar).Value = textBox1.Text;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = pass;
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Model.ID = Convert.ToInt32(reader["ID"]);
                        Model.Name = Convert.ToString(reader["Name"]);
                        Model.Username = Convert.ToString(reader["Username"]);
                        Model.JobID = Convert.ToInt32(reader["JobID"]);

                        if (Model.JobID == 1)
                        {
                            this.Hide();
                            MainFrontOffice main = new MainFrontOffice();
                            main.Show();
                        }
                        else
                        {
                            this.Hide();
                            MainAdmin main = new MainAdmin();
                            main.Show();
                        }
                    }
                    else
                    {
                        textBox1.Text = "";
                        textBox2.Text = "";
                        checkBox1.Checked = false;
                        MessageBox.Show("Wrong Password or Username", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex), "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure to Close ?", "Alert",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else if(checkBox1.Checked == false)
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doLogin();
        }

        public Boolean val()
        {
            if (textBox1.Text.Length < 1 || textBox2.Text.Length < 1)
            {
                MessageBox.Show("Insert Field With the Right Values", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox2.Text.Length < 8)
            {
                MessageBox.Show("Password Can't Under 8 Characters!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                doLogin();
            }
        }
    }
}
