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
    public partial class AddCustomer : Form
    {
        SqlCommand command;

        SqlConnection conn = new SqlConnection(Utils.conn);
        public AddCustomer()
        {
            InitializeComponent();

            textBox5.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ReservationForm reservation = new ReservationForm();
            reservation.ShowDialog();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        bool val()
        {
            if (textBox1.TextLength < 1 && textBox2.TextLength < 1 && textBox4.TextLength < 1 && comboBox1.SelectedValue == null && textBox5.TextLength < 1)
            {
                MessageBox.Show("Fill the field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            if(textBox2.TextLength != 16)
            {
                MessageBox.Show("NIK Must be 16 Characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (val())
            {
                command = new SqlCommand("INSERT INTO Customer(Name, NIK, Email, Gender, Age, phone_number, DateOfBirth) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "','" + textBox4.Text + "','" + comboBox1.Text[0] + "','" + Convert.ToInt32(textBox5.Text) + "', " + textBox3.Text + ", '"+ dateTimePicker1.Value.ToString("yyyy-MM-dd") +"')", conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Success");
                ReservationForm form = new ReservationForm();
                this.Hide();
                form.ShowDialog();
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime.Now.ToString("yyyy");

            int umur = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
            textBox5.Text = umur.ToString();
        }

        private void label21_Click(object sender, EventArgs e)
        {
            ReservationForm form = new ReservationForm();
            this.Hide();
            form.ShowDialog();
        }
    }
}
