using Collegemanagement.extension;
using Collegemanagement.Repository.Interface;
using System.Data;
using System.Net;
using System.Reflection;

namespace Collegemanagement.Model
{
    public class CourseModel : IMapper
    {
        public int ID { get; set; }
        public string Program { get; set; }
        public string Courseid { get; set; }
        public string Coursename { get; set; }
        public string Description { get; set; }
        public String Duration { get; set; }
        public int Availablesheet { get; set; }


        public void Map(IDataReader reader)
        {
            // Mapping Resorts properties
            if (reader.FieldCount > 0)
            {
                ID = reader.GetValue<int>("ID");
                Program = reader.GetValue<string>("Program");
                Courseid = reader.GetValue<string>("Courseid");
                Coursename = reader.GetValue<string>("Coursename");
                Description = reader.GetValue<string>("Description");
                Duration = reader.GetValue<string>("Duration");
                Availablesheet = reader.GetValue<int>("Availablesheet");
                
            }


        }
    }
}
