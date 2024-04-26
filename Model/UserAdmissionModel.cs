using Collegemanagement.extension;
using Collegemanagement.Repository.Interface;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;

namespace Collegemanagement.Model
{
    public class UserAdmissionModel : IMapper
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




        public void Map(IDataReader reader)
        {
            
            if (reader.FieldCount > 0)
            {
                Program = reader.GetValue<string>("Program");
                Courseid = reader.GetValue<string>("Courseid");
                Coursename = reader.GetValue<string>("Coursename");
                LastName = reader.GetValue<string>("LastName");
                ID = reader.GetValue<int>("ID");
                FirstName = reader.GetValue<string>("FirstName");
                LastName = reader.GetValue<string>("LastName");
                Gender = reader.GetValue<string>("Gender");
                Email = reader.GetValue<string>("Email");
                HighSchoolName = reader.GetValue<string>("HighSchoolName");
                HighSchoolGroup = reader.GetValue<string>("HighSchoolGroup");
                HighSchoolMark = reader.GetValue<int>("HighSchoolMark");
                SecondarySchoolName = reader.GetValue<string>("SecondarySchoolName");
                SecondarySchoolMark = reader.GetValue<int>("SecondarySchoolMark");
                CommunityCertificate = reader.GetValue<byte[]>("CommunityCertificate");
                Photo = reader.GetValue<byte[]>("Photo");
                Status = reader.GetValue<int>("Status");
            }


        }
    }


}
