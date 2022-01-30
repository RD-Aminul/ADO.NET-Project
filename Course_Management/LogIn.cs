using Course_Management.Entities;
using Course_Management.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_Management
{
    public partial class LogIn : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public LogIn()
        {
            InitializeComponent();
        }

        private void TxtUsername_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            label1.ForeColor = Color.FromArgb(78, 184, 206);
            picUser.BackgroundImage = Properties.Resources.user1;
            label4.BackColor = Color.FromArgb(78, 184, 206);
            txtUsername.ForeColor = Color.FromArgb(78, 184, 206);

            picUnlock.BackgroundImage = Properties.Resources.key1;
            label3.BackColor = Color.FromArgb(211, 84, 0);
            label2.ForeColor = Color.FromArgb(211, 84, 0);
            txtPassword.ForeColor = Color.FromArgb(211, 84, 0);
        }

        private void TxtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            label2.ForeColor = Color.FromArgb(78, 184, 206);
            picUnlock.BackgroundImage = Properties.Resources.key;
            label3.BackColor = Color.FromArgb(78, 184, 206);
            txtPassword.ForeColor = Color.FromArgb(78, 184, 206);

            picUser.BackgroundImage = Properties.Resources.user;
            label4.BackColor = Color.FromArgb(211, 84, 0);
            label1.ForeColor = Color.FromArgb(211, 84, 0);
            txtUsername.ForeColor = Color.FromArgb(211, 84, 0);
        }

        private void BtnLogIn_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                Users User = new Users();
                User.UserName = txtUsername.Text;
                User.Password = txtPassword.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Users WHERE UserName ='" + User.UserName + "' AND Password ='" + User.Password + "'";
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                if (dt.Rows.Count > 0)
                {

                    MainInterface main = new MainInterface();
                    main.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Wrong Username or Password. If you are not registered. Please Registration below...");
                }
            }
        }

        private void Linkbtn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register reg = new Register();
            reg.Show();
            Hide();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
