using Course_Management.Entities;
using Course_Management.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_Management
{
    public partial class MainInterface : Form
    {
        string imgLocation = "";
        int studentId = 0;
        string conStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public MainInterface()
        {
            InitializeComponent();
        }
        private void MainInterface_Load(object sender, EventArgs e)
        {
            Clear();
            dataGridView1.AutoGenerateColumns = false;
            LoadDgv();
            
            LoadcmbBatch();
            LoadcmbTsp();
            LoadcmbCourse();
        }

        private void Clear()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            dtpDob.Text = DateTime.Now.ToString();
            txtMobile.Text = "";
            txtEmail.Text = "";
            cmbBatchID.Text = "";
            cmbTspName.Text = "";
            cmbCourseName.Text = "";
            cmbCourseFee.Text = "";
            pictureBox1.ImageLocation = @"D:\IDB_Course\Core_Course\Project\ADO.NET\Course_Management_Project\Course_Management\Resources\placeholder.png";
        }

        private void LoadDgv()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                Course obj = new Course();
                obj.CourseName = txtCourseName.Text;
                obj.CourseFee = txtCourseFee.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT StudentID, FirstName, LastName, DateOfBirth, MobileNO, Email, T.TspName, S.TspID, C.CourseName, C.CourseFee, S.CourseID, " +
                    "B.BatchName, S.BatchID, ImageLocation  FROM Student AS S " +
                    "JOIN Tsp AS T ON S.TspID = T.TspID JOIN Course AS C ON S.CourseID = C.CourseID JOIN Batch AS B ON S.BatchID = B.BatchID";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                dt.Columns.Add("Image", Type.GetType("System.Byte[]"));
                foreach (DataRow row in dt.Rows)
                {
                    row["Image"] = File.ReadAllBytes(row["ImageLocation"].ToString());
                }
                dataGridView1.DataSource = dt;
            }
        }

        private void LoadcmbCourse()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                Course obj = new Course();
                obj.CourseName = txtCourseName.Text;
                obj.CourseFee = txtCourseFee.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Course";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                cmbCourseName.DisplayMember = "CourseName";
                cmbCourseName.ValueMember = "CourseID";
                cmbCourseName.DataSource = dt;
                cmbCourseFee.DisplayMember = "CourseFee";
                cmbCourseFee.ValueMember = "CourseID";
                cmbCourseFee.DataSource = dt;
            }
        }

        private void LoadcmbTsp()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                TSP obj = new TSP();
                obj.TspName = txtTspName.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Tsp";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                cmbTspName.DisplayMember = "TspName";
                cmbTspName.ValueMember = "TspID";
                cmbTspName.DataSource = dt;
            }
        }

        private void LoadcmbBatch()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                Batch obj = new Batch();
                obj.BatchName = txtBatchName.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Batch";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                cmbBatchID.DisplayMember = "BatchName";
                cmbBatchID.ValueMember = "BatchID";
                cmbBatchID.DataSource = dt;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                btnMaximize.Image = Resources.Normal;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                btnMaximize.Image = Resources.Maximize;
            }
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnTspSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                SqlTransaction tran = null;
                con.Open();
                tran = con.BeginTransaction();
                TSP obj = new TSP();
                obj.TspName = txtTspName.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO TSP (TspName) VALUES ('" + obj.TspName + "')";
                cmd.Transaction = tran;
                count = cmd.ExecuteNonQuery();
                if (obj.TspName == "")
                {
                    MessageBox.Show("Please Fill Up The Field");
                }
                else if (count > 0)
                {
                    tran.Commit();
                    MessageBox.Show("Tsp Save Successfully");
                    txtTspName.Text = "";
                    LoadcmbTsp();
                }
                else
                {
                    tran.Rollback();
                    MessageBox.Show("Something Wrong");
                }
            }
        }

        private void btnCourseSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                SqlTransaction tran = null;
                con.Open();
                tran = con.BeginTransaction();
                Course obj = new Course();
                obj.CourseName = txtCourseName.Text;
                obj.CourseFee = txtCourseFee.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Course (CourseName, CourseFee) VALUES ('" + obj.CourseName + "','" + obj.CourseFee + "')";
                cmd.Transaction = tran;
                count = cmd.ExecuteNonQuery();
                if (obj.CourseName == "" || obj.CourseFee == "")
                {
                    MessageBox.Show("Please Fill Up The Fields");
                }
                else if (count > 0)
                {
                    tran.Commit();
                    MessageBox.Show("Course Save Successfully");
                    txtCourseName.Text = "";
                    txtCourseFee.Text = "";
                    LoadcmbCourse();
                }
                else
                {
                    tran.Rollback();
                    MessageBox.Show("Something Wrong");
                }
            }
        }

        private void btnBatchSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                SqlTransaction tran = null;
                con.Open();
                tran = con.BeginTransaction();
                Batch obj = new Batch();
                obj.BatchName = txtBatchName.Text;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Batch (BatchName) VALUES ('" + obj.BatchName + "')";
                cmd.Transaction = tran;
                count = cmd.ExecuteNonQuery();
                if (obj.BatchName == "")
                {
                    MessageBox.Show("Please Fill Up The Field");
                }
                else if (count > 0)
                {
                    tran.Commit();
                    MessageBox.Show("Batch Save Successfully");
                    txtBatchName.Text = "";
                    LoadcmbBatch();
                }
                else
                {
                    tran.Rollback();
                    MessageBox.Show("Something Wrong");
                }
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imgLocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = imgLocation;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                int count = 0;
                SqlTransaction tran = null;
                con.Open();
                tran = con.BeginTransaction();
                string file = imgLocation;
                string[] f = file.Split('\\');
                string fileName = f[f.Length - 1];
                string path = @"D:\IDB_Course\Core_Course\Project\ADO.NET\Course_Management_Project\Course_Management\Images";
                string pathString = System.IO.Path.Combine(path, fileName);

                Student obj = new Student();
                obj.FirstName = txtFirstName.Text;
                obj.LastName = txtLastName.Text;
                obj.DateOfBirth = Convert.ToDateTime(dtpDob.Text);
                obj.MobileNo = txtMobile.Text;
                obj.Email = txtEmail.Text;
                obj.BatchID = Convert.ToInt32(cmbBatchID.SelectedValue);
                obj.TspID = Convert.ToInt32(cmbTspName.SelectedValue);
                obj.CourseID = Convert.ToInt32(cmbCourseName.SelectedValue);
                obj.ImageLocation = pathString;
                Image i = pictureBox1.Image;
                i.Save(pathString);

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Student (FirstName, LastName, DateOfBirth, MobileNo, Email, BatchID, TspID, CourseID, ImageLocation)" +
                    "VALUES ('" + obj.FirstName + "','" + obj.LastName + "','" + obj.DateOfBirth + "','" + obj.MobileNo + "','" + obj.Email + "','" + obj.BatchID + "','" + obj.TspID + "','" + obj.CourseID + "','" + obj.ImageLocation + "')";
                cmd.Transaction = tran;
                count = cmd.ExecuteNonQuery();
                if (obj.FirstName == "" || obj.LastName == "" || obj.MobileNo == "" || obj.Email == "" || obj.BatchID == 0 || obj.TspID == 0 || obj.CourseID == 0 || obj.ImageLocation == "")
                {
                    MessageBox.Show("Please Fill Up The Fields");
                }
                else if (count > 0)
                {
                    tran.Commit();
                    MessageBox.Show("Student Save Successfully");
                    Clear();
                    LoadDgv();
                }
                else
                {
                    tran.Rollback();
                    MessageBox.Show("Something Wrong");
                }
            }
        }

        //private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    int cellId = e.RowIndex;
        //    DataGridViewRow row = dataGridView1.Rows[cellId];
        //    try
        //    {
        //        studentId = Convert.ToInt32(row.Cells[0].Value.ToString());
        //    }
        //    catch (Exception)
        //    {
        //        studentId = 0;
        //    }
        //    txtFirstName.Text = row.Cells[1].Value.ToString();
        //    txtLastName.Text = row.Cells[2].Value.ToString();
        //    dtpDob.Text = row.Cells[3].Value.ToString();
        //    txtMobile.Text = row.Cells[4].Value.ToString();
        //    txtEmail.Text = row.Cells[5].Value.ToString();
        //    cmbBatchID.SelectedValue = BatchName;
        //    cmbTspName.SelectedValue = TspName;
        //    cmbCourseName.SelectedValue = CourseName;
        //    cmbCourseFee.SelectedValue = CourseFee;
        //    if ((row.Cells[7].Value) == null)
        //    {
        //        byte[] data = (byte[])row.Cells[10].Value;
        //        MemoryStream stream = new MemoryStream(data);
        //        pictureBox1.Image = Image.FromStream(stream);
        //        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        //    }

        //    string fileName = row.Cells[8].Value.ToString();
        //    imgLocation = fileName;
        //}

        //private void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    int count = 0;
        //    Student obj = new Student();
        //    obj.ImageName = imgLocation;
        //    byte[] image = null;
        //    FileStream stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
        //    BinaryReader brdr = new BinaryReader(stream);
        //    image = brdr.ReadBytes((int)stream.Length);
        //    using (SqlConnection con = new SqlConnection(conStr))
        //    {
        //        SqlCommand cmd = con.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        obj.FirstName = txtFirstName.Text;
        //        obj.LastName = txtLastName.Text;
        //        obj.DateOfBirth = Convert.ToDateTime(dtpDob.Text);
        //        obj.MobileNo = txtMobile.Text;
        //        obj.Email = txtEmail.Text;
        //        obj.BatchID = Convert.ToInt32(cmbBatchID.SelectedValue.ToString());
        //        obj.TspID = Convert.ToInt32(cmbTspName.SelectedValue.ToString());
        //        obj.CourseID = Convert.ToInt32(cmbCourseName.SelectedValue.ToString());
        //        obj.ImageData = image;
        //        obj.ImageName = objStudent.ImageName;
        //        cmd.CommandText = "UPDATE Student SET FirstName='" + objStudent.FirstName + "', LastName='" + objStudent.LastName + "', DateOfBirth='" + objStudent.DateOfBirth + "', ImageName='" + objStudent.ImageName + "', Email='" + objStudent.Email + "', CourseID='" + objStudent.CourseID + "', ImageData=@img WHERE StudentID ='" + studentID + "'";
        //        cmd.Parameters.Add(new SqlParameter("@img", objStudent.ImageData));
        //        con.Open();
        //        count = cmd.ExecuteNonQuery();
        //        if (count > 0)
        //        {
        //            MessageBox.Show("Student Info Updated Successfully");
        //            ClearStudentData();
        //            studentID = 0;
        //            LoaddgvStudent();
        //        }

        //    }
        //}
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Student WHERE StudentID ='" + StudentID + "'";
                con.Open();
                count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Student Info Deleted Successfully");
                    Clear();
                    studentId = 0;
                    LoadDgv();
                }
            }
        }

        private void btnReoprt_Click(object sender, EventArgs e)
        {
            List<ViewStudents> list = new List<ViewStudents>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT StudentID, FirstName, LastName, DateOfBirth, MobileNO, Email, T.TspName, S.TspID, C.CourseName, C.CourseFee, S.CourseID, " +
                    "B.BatchName, S.BatchID, ImageLocation  FROM Student AS S " +
                    "JOIN Tsp AS T ON S.TspID = T.TspID JOIN Course AS C ON S.CourseID = C.CourseID JOIN Batch AS B ON S.BatchID = B.BatchID";
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr, LoadOption.Upsert);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ViewStudents obj = new ViewStudents();
                    obj.StudentID = Convert.ToInt32(dt.Rows[i]["StudentID"].ToString());
                    obj.FirstName = dt.Rows[i]["FirstName"].ToString();
                    obj.LastName = dt.Rows[i]["LastName"].ToString();
                    obj.DateOfBirth = Convert.ToDateTime(dt.Rows[i]["DateOfBirth"].ToString());
                    obj.MobileNo = dt.Rows[i]["MobileNO"].ToString();
                    obj.Email = dt.Rows[i]["Email"].ToString();
                    obj.TspName = dt.Rows[i]["TspName"].ToString();
                    obj.BatchName = dt.Rows[i]["BatchName"].ToString();
                    obj.CourseName = dt.Rows[i]["CourseName"].ToString();
                    obj.ImageLocation = dt.Rows[i]["ImageLocation"].ToString();
                    list.Add(obj);
                }
            }

            using (Report frm = new Report(list))
            {
                frm.ShowDialog();
            }
        }
    }
}
