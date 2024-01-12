using System.ComponentModel.DataAnnotations;

namespace Collegemanagement.Model
{
    public class UserAdmissionModel
    {
        public string Program { get; set; }
        public string Courseid { get; set; }
        public string Coursename { get; set; }
        public int ID { get; set; }
        [Display(Name = "Name")]

        public string FirstName { get; set; }
        [Display(Name = "Last Name")]

        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }

        [Display(Name = "HighSchool")]
        public string HighSchoolName { get; set; }
        [Display(Name = "12th Group")]
        public string HighSchoolGroup { get; set; }
        [Display(Name = "12th Mark")]
        public int HighSchoolMark { get; set; }
        [Display(Name = "SchoolName")]
        public string SecondarySchoolName { get; set; }
        [Display(Name = "10th Mark")]
        public int SecondarySchoolMark { get; set; }
        [Display(Name = "Community Certificate")]

        public byte[] CommunityCertificate { get; set; }
        public byte[] Photo { get; set; }
        public int Status { get; set; }
    }
}
