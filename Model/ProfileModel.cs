using System.ComponentModel.DataAnnotations;

namespace Collegemanagement.Model
{
    public class ProfileModel
    {

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
  

    }
}
