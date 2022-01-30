using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Management.Entities
{
    public class ViewStudents
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ImageLocation { get; set; }
        public string BatchName { get; set; }
        public string CourseName { get; set; }
        public string CourseFee { get; set; }
        public string TspName { get; set; }
    }
}
