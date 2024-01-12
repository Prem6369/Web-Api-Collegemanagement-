using Collegemanagement.Model;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Collegemanagement.Repository
{
    public class AdminRepository
    {
        private IConfiguration _configuration;
        private SqlConnection connect;

        public AdminRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            Connection();
        }

        private void Connection()
        {
            string connects = _configuration.GetConnectionString("connect");
            connect = new SqlConnection(connects);
        }
        /// <summary>
        /// Add new Admin page logic
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool AddAdmin(AdminModel admin)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_CheckEmail", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", admin.Email);
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    return false;
                }
                else
                {
                    Connection();

                    SqlCommand command1 = new SqlCommand("SP_AddAdmin", connect);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("Role", 2);
                    command1.Parameters.AddWithValue("@FirstName", admin.FirstName);
                    command1.Parameters.AddWithValue("@LastName", admin.LastName);
                    command1.Parameters.AddWithValue("@Gender", admin.Gender);
                    command1.Parameters.AddWithValue("@PhoneNumber", admin.PhoneNumber);
                    command1.Parameters.AddWithValue("@Address", admin.Address);
                    command1.Parameters.AddWithValue("@Email", admin.Email);
                    command1.Parameters.AddWithValue("@Password", Encrypt(admin.Password));

                    connect.Open();

                    int i = command1.ExecuteNonQuery();

                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// show the admin list
        /// </summary>
        /// <returns></returns>
        public List<RegisterModel> AdminList()
        {
            try
            {
                Connection();
                connect.Open();

                List<RegisterModel> adminList = new List<RegisterModel>();

                SqlCommand command = new SqlCommand("SP_AdminList", connect);
                command.Parameters.AddWithValue("Role", 2);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                foreach (DataRow list in dt.Rows)
                {
                    adminList.Add(
                        new RegisterModel
                        {
                            Id = Convert.ToInt32(list["Id"]),
                            FirstName = Convert.ToString(list["FirstName"]),
                            LastName = Convert.ToString(list["LastName"]),
                            Gender = Convert.ToString(list["Gender"]),
                            PhoneNumber = Convert.ToString(list["PhoneNumber"]),
                            Address = Convert.ToString(list["Address"]),
                            Email = Convert.ToString(list["Email"]),
                            Password = Convert.ToString(list["Password"]),

                        });
                }
                return adminList;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// show the register people list
        /// </summary>
        /// <returns></returns>
        public List<RegisterModel> RegisterList()
        {
            try
            {
                Connection();
                connect.Open();

                List<RegisterModel> userList = new List<RegisterModel>();

                SqlCommand command = new SqlCommand("SP_AdminList", connect);
                command.Parameters.AddWithValue("Role", 1);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                foreach (DataRow list in dt.Rows)
                {
                    userList.Add(
                        new RegisterModel
                        {
                            Id = Convert.ToInt32(list["Id"]),
                            FirstName = Convert.ToString(list["FirstName"]),
                            LastName = Convert.ToString(list["LastName"]),
                            Gender = Convert.ToString(list["Gender"]),
                            DateOfBirth = Convert.ToDateTime(list["DateOfBirth"]),
                            Age = Convert.ToInt32(list["Age"]),
                            PhoneNumber = Convert.ToString(list["PhoneNumber"]),
                            State = Convert.ToString(list["State"]),
                            City = Convert.ToString(list["city"]),
                            Address = Convert.ToString(list["Address"]),
                            Email = Convert.ToString(list["Email"]),
                            Password = Convert.ToString(list["Password"]),

                        });
                }
                return userList;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// detele the admin 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAdmin(int id)
        {
            try
            {
                Connection();
                connect.Open();
                SqlCommand command = new SqlCommand("SP_DeleteAdmin", connect);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", id);


                int i = command.ExecuteNonQuery();

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// add new PG course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool AddPgCourse(CourseModel course)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_CheckCourseidname", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Courseid", course.Courseid);
                command.Parameters.AddWithValue("@Coursename", course.Coursename);
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    return false;
                }
                else
                {
                    Connection();
                    SqlCommand command1 = new SqlCommand("SPI_Course", connect);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@Program", course.Program);
                    command1.Parameters.AddWithValue("@Courseid", course.Courseid);
                    command1.Parameters.AddWithValue("@Coursename", course.Coursename);
                    command1.Parameters.AddWithValue("@Description", course.Description);
                    command1.Parameters.AddWithValue("@Duration", course.Duration);
                    command1.Parameters.AddWithValue("@Availablesheet", 4);

                    connect.Open();

                    int i = command1.ExecuteNonQuery();

                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// adding new PC course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool AddPcCourse(CourseModel course)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_CheckCourseidname", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Courseid", course.Courseid);
                command.Parameters.AddWithValue("@Coursename", course.Coursename);
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    return false;
                }
                else
                {
                    Connection();
                    SqlCommand command1 = new SqlCommand("SPI_Course", connect);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@Program", course.Program);
                    command1.Parameters.AddWithValue("@Courseid", course.Courseid);
                    command1.Parameters.AddWithValue("@Coursename", course.Coursename);
                    command1.Parameters.AddWithValue("@Description", course.Description);
                    command1.Parameters.AddWithValue("@Duration", course.Duration);
                    command1.Parameters.AddWithValue("@Availablesheet", 4);

                    connect.Open();

                    int i = command1.ExecuteNonQuery();

                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// adding new UG course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool AddUgCourse(CourseModel course)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_CheckCourseidname", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Courseid", course.Courseid);
                command.Parameters.AddWithValue("@Coursename", course.Coursename);
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    return false;
                }
                else
                {
                    connect.Close();
                    Connection();
                    SqlCommand command1 = new SqlCommand("SPI_Course", connect);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@Program", course.Program);
                    command1.Parameters.AddWithValue("@Courseid", course.Courseid);
                    command1.Parameters.AddWithValue("@Coursename", course.Coursename);
                    command1.Parameters.AddWithValue("@Description", course.Description);
                    command1.Parameters.AddWithValue("@Duration", course.Duration);
                    command1.Parameters.AddWithValue("@Availablesheet", 4);

                    connect.Open();

                    int i = command1.ExecuteNonQuery();


                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            finally
            {
                connect.Close();
            }

        }
         /// <summary>
         /// show the ug list
         /// </summary>
         /// <returns></returns>
        public List<CourseModel> UgList()
        {
            try
            {
                Connection();
                connect.Open();

                List<CourseModel> courseList = new List<CourseModel>();

                SqlCommand command = new SqlCommand("SPS_Courseprogram", connect);
                command.Parameters.AddWithValue("@Program", "UG");
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                foreach (DataRow list in dt.Rows)
                {
                    courseList.Add(
                        new CourseModel
                        {
                            ID = Convert.ToInt32(list["ID"]),
                            Program = Convert.ToString(list["Program"]),
                            Courseid = Convert.ToString(list["Courseid"]),
                            Coursename = Convert.ToString(list["Coursename"]),
                            Description = Convert.ToString(list["Description"]),
                            Duration = Convert.ToString(list["Duration"]),
                            Availablesheet = Convert.ToInt32(list["Availablesheet"])
                        });
                }
                return courseList;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// show the PG program list
        /// </summary>
        /// <returns></returns>
        public List<CourseModel> PgProgram()
        {
            try
            {
                Connection();
                connect.Open();

                List<CourseModel> courseList = new List<CourseModel>();

                SqlCommand cmd = new SqlCommand("SPS_Courseprogram", connect);
                cmd.Parameters.AddWithValue("@Program", "PG");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);


                foreach (DataRow list in dt.Rows)
                {
                    courseList.Add(
                        new CourseModel
                        {
                            ID = Convert.ToInt32(list["ID"]),
                            Program = Convert.ToString(list["Program"]),
                            Courseid = Convert.ToString(list["Courseid"]),
                            Coursename = Convert.ToString(list["Coursename"]),
                            Description = Convert.ToString(list["Description"]), // Change this to Convert.ToString
                            Duration = Convert.ToString(list["Duration"]),
                            Availablesheet = Convert.ToInt32(list["Availablesheet"])
                        });
                }
                return courseList;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// return thr PC course list
        /// </summary>
        /// <returns></returns>
        public List<CourseModel> PcProgram()
        {
            try
            {
                Connection();
                connect.Open();

                List<CourseModel> courseList = new List<CourseModel>();

                SqlCommand command = new SqlCommand("SPS_Courseprogram", connect);
                command.Parameters.AddWithValue("@Program", "PC");
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);


                foreach (DataRow list in dt.Rows)
                {
                    courseList.Add(
                        new CourseModel
                        {
                            ID = Convert.ToInt32(list["ID"]),
                            Program = Convert.ToString(list["Program"]),
                            Courseid = Convert.ToString(list["Courseid"]),
                            Coursename = Convert.ToString(list["Coursename"]),
                            Description = Convert.ToString(list["Description"]), // Change this to Convert.ToString
                            Duration = Convert.ToString(list["Duration"]),
                            Availablesheet = Convert.ToInt32(list["Availablesheet"])
                        });
                }
                return courseList;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// delete the course by courseid
        /// </summary>
        /// <param name="Courseid"></param>
        /// <returns></returns>
        public bool DeleteData(string Courseid)
        {
            try
            {
                Connection();
                connect.Open();
                SqlCommand command = new SqlCommand("SPD_Course", connect);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Courseid", Courseid);


                int i = command.ExecuteNonQuery();


                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// get the courseby courseid to feteching the page
        /// </summary>
        /// <param name="Courseid"></param>
        /// <returns></returns>
        public List<CourseModel> GetCourseById(string Courseid)
        {
            try
            {
                Connection();
                connect.Open();

                List<CourseModel> Courselist = new List<CourseModel>();
                SqlCommand cmd = new SqlCommand("SP_FetchByCourseId", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Courseid", Courseid);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                foreach (DataRow list in dt.Rows)
                {
                    Courselist.Add(

                      new CourseModel
                      {
                          ID = Convert.ToInt32(list["ID"]),
                          Program = Convert.ToString(list["Program"]),
                          Courseid = Convert.ToString(list["Courseid"]),
                          Coursename = Convert.ToString(list["Coursename"]),
                          Description = Convert.ToString(list["Description"]),
                          Duration = Convert.ToString(list["Duration"])
                      }
                    ); 
                }

                return Courselist;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// all fetchting value once fill and update the cousre list
        /// </summary>
        /// <param name="course"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Update(CourseModel course, string id)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("SPU_Course", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Program", course.Program);
                cmd.Parameters.AddWithValue("@Courseid", course.Courseid);
                cmd.Parameters.AddWithValue("@Coursename", course.Coursename);
                cmd.Parameters.AddWithValue("@Description", course.Description);
                cmd.Parameters.AddWithValue("@Duration", course.Duration);
                cmd.Parameters.AddWithValue("@Availablesheet", 4);


                connect.Open();
                int i = cmd.ExecuteNonQuery();


                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// show the userdetails who are apply the course means show here
        /// </summary>
        /// <returns></returns>
        public List<UserAdmissionModel> UserDetails()
        {
            try
            {
                Connection();
                connect.Open();

                List<UserAdmissionModel> userList = new List<UserAdmissionModel>();

                SqlCommand cmd = new SqlCommand("SP_FullUserDetails", connect);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);


                foreach (DataRow list in dt.Rows)
                {
                    userList.Add(
                        new UserAdmissionModel
                        {
                            Program = Convert.ToString(list["Program"]),
                            Courseid = Convert.ToString(list["Courseid"]),
                            Coursename = Convert.ToString(list["Coursename"]),
                            ID = Convert.ToInt32(list["ID"]),
                            FirstName = Convert.ToString(list["FirstName"]),
                            LastName = Convert.ToString(list["LastName"]),
                            Gender = Convert.ToString(list["Gender"]),
                            Email = Convert.ToString(list["Email"]),
                            HighSchoolName = Convert.ToString(list["HighSchoolName"]),
                            HighSchoolGroup = Convert.ToString(list["HighSchoolGroup"]),
                            HighSchoolMark = Convert.ToInt32(list["HighSchoolMark"]),
                            SecondarySchoolName = Convert.ToString(list["SecondarySchoolName"]),
                            SecondarySchoolMark = Convert.ToInt32(list["SecondarySchoolMark"]),
                            CommunityCertificate = (byte[])(list["CommunityCertificate"]),
                            Photo = (byte[])(list["Photo"]),
                            Status = Convert.ToInt32(list["Status"])

                        });
                }
                return userList;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// return the contact form list 
        /// </summary>
        /// <returns></returns>
        public List<ContactModel> ContactForm()
        {
            try
            {
                Connection();
                connect.Open();
                List<ContactModel> contctList = new List<ContactModel>();
                SqlCommand cmd = new SqlCommand("SP_Contactdetails", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    contctList.Add(new ContactModel
                    {
                        id = Convert.ToInt32(row["ID"]),
                        Name = Convert.ToString(row["Fullname"]),
                        Email = Convert.ToString(row["Email"]),
                        Phonenumber = Convert.ToString(row["Phonenumber"]),
                        Message = Convert.ToString(row["Message"])
                    });
                }
                return contctList;
            }
            finally
            {
                connect.Close();
            }


        }
        /// <summary>
        /// Encrpt the password using AES algorithm
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        private string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        /// <summary>
        /// admin can update the status if user page also change the that field get the status and perfor the that 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool StatusChange(int ID, int Status)
        {
            try
            {
                Connection();

                SqlCommand cmd = new SqlCommand("SP_UpdateStatus", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ID", ID);
                cmd.Parameters.AddWithValue("Status", Status);
                connect.Open();
                int i = cmd.ExecuteNonQuery();

                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                connect.Close();
            }

        }
    }
}
