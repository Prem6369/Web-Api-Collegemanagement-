using Collegemanagement.extension;
using Collegemanagement.Repository.Interface;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Collegemanagement.Model
{
    public class RegisterModel : IMapper
    {
        public int Id { get; set; }
        public int Role { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public byte[] Photo { get; set; }



        public void Map(IDataReader reader)
        {
            // Mapping Resorts properties
            if (reader.FieldCount > 0)
            {
                Id = reader.GetValue<int>("Id");
                Role = reader.GetValue<int>("Role");
                FirstName = reader.GetValue<string>("FirstName");
                LastName = reader.GetValue<string>("LastName");
                DateOfBirth = reader.GetValue<DateTime>("DateOfBirth");
                Age = reader.GetValue<int>("Age");
                Gender = reader.GetValue<string>("Gender");
                PhoneNumber = reader.GetValue<string>("PhoneNumber");
                Address = reader.GetValue<string>("Address");
                State = reader.GetValue<string>("State");
                City = reader.GetValue<string>("City");
                Email = reader.GetValue<string>("Email");
            }


        }
    }
}
