using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Latian_LKS_2
{
    public partial class MainAdmin : Form
    {
        public MainAdmin()
        {
            InitializeComponent();
        }
        private void MainAdmin_Load(object sender, EventArgs e)
        {
            label_time.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            label_hour.Text = DateTime.Now.ToString("HH:mm");
            setNama();
        }
        public void setNama()
        {
            String name = Model.Name;
            String[] first = name.Split(' ');
            label_adm.Text = first[0];
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
            DialogResult result = MessageBox.Show("Are You Sure to Log Out ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Model.ID = 0;
                Model.Name = "";
                Model.Username = "";
                Model.JobID = 0;

                this.Hide();
                MainLogin main = new MainLogin();
                main.ShowDialog();
            }
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            RoomType roomType = new RoomType();
            this.Hide();
            roomType.ShowDialog();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            MasterRoom room = new MasterRoom();
            this.Hide();
            room.ShowDialog();
        }

        private void panel1_Click(object sender, EventArgs e)
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

        private void panel4_Click(object sender, EventArgs e)
        {
            MasterFD master = new MasterFD();
            this.Hide();
            master.ShowDialog();
        }
    }
}
