using Course_Management.Entities;
using Course_Management.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Course_Management
{
    public partial class Report : Form
    {
        List<ViewStudents> innerList;
        public Report()
        {
            InitializeComponent();
        }

        public Report(List<ViewStudents>outerList)
        {
            InitializeComponent();
            innerList = outerList;
        }
        private void Report_Load(object sender, EventArgs e)
        {
            StudentReport rpt = new StudentReport();
            rpt.SetDataSource(innerList);
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Refresh();
        }
    }
}
