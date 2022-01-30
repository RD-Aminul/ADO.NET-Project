using Course_Management.Entities;
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
    public partial class Register : Form
    {
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public Register()
        {
            InitializeComponent();
        }

        LogIn log = new LogIn();

        private void TxtUsername_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            label1.ForeColor = Color.FromArgb(78, 184, 206);
            picUser.BackgroundImage = Properties.Resources.user1;
            label4.BackColor = Color.FromArgb(78, 184, 206);
            txtUsername.ForeColor = Color.FromArgb(78, 184, 206);

            picEmail.BackgroundImage = Properties.Resources.mail1;
            label3.BackColor = Color.FromArgb(211, 84, 0);
            label2.ForeColor = Color.FromArgb(211, 84, 0);
            txtEmail.ForeColor = Color.FromArgb(211, 84, 0);

            picPass.BackgroundImage = Properties.Resources.lock1;
            label9.BackColor = Color.FromArgb(211, 84, 0);
            label6.ForeColor = Color.FromArgb(211, 84, 0);
            txtPassword.ForeColor = Color.FromArgb(211, 84, 0);

            picConfiramPass.BackgroundImage = Properties.Resources.lock1;
            label8.BackColor = Color.FromArgb(211, 84, 0);
            label7.ForeColor = Color.FromArgb(211, 84, 0);
            txtConfiramPassword.ForeColor = Color.FromArgb(211, 84, 0);
        }

        private void TxtEmail_Click(object sender, EventArgs e)
        {
            txtEmail.Clear();
            label2.ForeColor = Color.FromArgb(78, 184, 206);
            picEmail.BackgroundImage = Properties.Resources.mail;
            label3.BackColor = Color.FromArgb(78, 184, 206);
            txtEmail.ForeColor = Color.FromArgb(78, 184, 206);

            picUser.BackgroundImage = Properties.Resources.user;
            label4.BackColor = Color.FromArgb(211, 84, 0);
            label1.ForeColor = Color.FromArgb(211, 84, 0);
            txtUsername.ForeColor = Color.FromArgb(211, 84, 0);

            picPass.BackgroundImage = Properties.Resources.lock1;
            label9.BackColor = Color.FromArgb(211, 84, 0);
            label6.ForeColor = Color.FromArgb(211, 84, 0);
            txtPassword.ForeColor = Color.FromArgb(211, 84, 0);

            picConfiramPass.BackgroundImage = Properties.Resources.lock1;
            label8.BackColor = Color.FromArgb(211, 84, 0);
            label7.ForeColor = Color.FromArgb(211, 84, 0);
            txtConfiramPassword.ForeColor = Color.FromArgb(211, 84, 0);
        }

        private void TxtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
            label6.ForeColor = Color.FromArgb(78, 184, 206);
            picPass.BackgroundImage = Properties.Resources._lock;
            label9.BackColor = Color.FromArgb(78, 184, 206);
            txtPassword.ForeColor = Color.FromArgb(78, 184, 206);

            picUser.BackgroundImage = Properties.Resources.user;
            label4.BackColor = Color.FromArgb(211, 84, 0);
            label1.ForeColor = Color.FromArgb(211, 84, 0);
            txtUsername.ForeColor = Color.FromArgb(211, 84, 0);

            picEmail.BackgroundImage = Properties.Resources.mail1;
            label3.BackColor = Color.FromArgb(211, 84, 0);
            label2.ForeColor = Color.FromArgb(211, 84, 0);
            txtEmail.ForeColor = Color.FromArgb(211, 84, 0);

            picConfiramPass.BackgroundImage = Properties.Resources.lock1;
            label8.BackColor = Color.FromArgb(211, 84, 0);
            label7.ForeColor = Color.FromArgb(211, 84, 0);
            txtConfiramPassword.ForeColor = Color.FromArgb(211, 84, 0);
        }

        private void TxtConfiramPassword_Click(object sender, EventArgs e)
        {
            txtConfiramPassword.Clear();
            label7.ForeColor = Color.FromArgb(78, 184, 206);
            picConfiramPass.BackgroundImage = Properties.Resources._lock;
            label8.BackColor = Color.FromArgb(78, 184, 206);
            txtConfiramPassword.ForeColor = Color.FromArgb(78, 184, 206);

            picUser.BackgroundImage = Properties.Resources.user;
            label4.BackColor = Color.FromArgb(211, 84, 0);
            label1.ForeColor = Color.FromArgb(211, 84, 0);
            txtUsername.ForeColor = Color.FromArgb(211, 84, 0);

            picEmail.BackgroundImage = Properties.Resources.mail1;
            label3.BackColor = Color.FromArgb(211, 84, 0);
            label2.ForeColor = Color.FromArgb(211, 84, 0);
            txtEmail.ForeColor = Color.FromArgb(211, 84, 0);

            picPass.BackgroundImage = Properties.Resources.lock1;
            label9.BackColor = Color.FromArgb(211, 84, 0);
            label6.ForeColor = Color.FromArgb(211, 84, 0);
            txtPassword.ForeColor = Color.FromArgb(211, 84, 0);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                SqlTransaction tran = null;
                con.Open();
                tran = con.BeginTransaction();
                Users obj = new Users();
                obj.UserName = txtUsername.Text.Trim();
                obj.Email = txtEmail.Text.Trim();
                obj.Password = txtPassword.Text.Trim();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Users (UserName, Email, Password)" +
                    "VALUES ('" + obj.UserName + "', '" + obj.Email + "', '" + obj.Password + "')";
                cmd.Transaction = tran;
                count = cmd.ExecuteNonQuery();
                if (obj.UserName == "" || obj.Password == "")
                {
                    MessageBox.Show("Please Fill Up The Fields");
                }
                else if (obj.Password != txtConfiramPassword.Text)
                {
                    MessageBox.Show("Password Not Match!!");
                    txtPassword.Text = txtConfiramPassword.Text = "";
                }
                else if (count > 0)
                {
                    tran.Commit();
                    MessageBox.Show("User Registered Successfully");
                    timer1.Start();
                }
                else
                {
                    tran.Rollback();
                    MessageBox.Show("ERROR");
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            log.Left += 10;
            if (log.Left >= 830)
            {
                timer1.Stop();
                this.TopMost = false;
                log.TopMost = true;
                timer2.Start();
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            log.Left -= 10;
            if (log.Left <= 515)
            {
                timer2.Stop();
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {
            log.Show();
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
