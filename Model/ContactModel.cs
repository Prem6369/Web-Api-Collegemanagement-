using Collegemanagement.extension;
using Collegemanagement.Repository.Interface;
using System.Data;
using System.Reflection;

namespace Collegemanagement.Model
{
    public class ContactModel :IMapper
    {
        public int id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phonenumber { get; set; }

        public string Message { get; set; }

        public void Map(IDataReader reader)
        {
            
            if (reader.FieldCount > 0)
            {
                id = reader.GetValue<int>("id");
                Name = reader.GetValue<string>("Name");
                Email = reader.GetValue<string>("Email");
                Phonenumber = reader.GetValue<string>("Phonenumber");
                Message = reader.GetValue<string>("Message");
                
            }


        }
    }
}
