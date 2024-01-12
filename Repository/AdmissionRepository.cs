using Collegemanagement.Model;
using System.Data;
using System.Data.SqlClient;

namespace Collegemanagement.Repository
{
    public class AdmissionRepository
    {

        private IConfiguration _configuration;
        private SqlConnection connect;

        public AdmissionRepository(IConfiguration configuration)
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
        /// return the user list based on user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<UserAdmissionModel> UserProfile(int id)
        {
            try
            {
                Connection();
                connect.Open();

                List<UserAdmissionModel> UserList = new List<UserAdmissionModel>();
                SqlCommand cmd = new SqlCommand("SP_UserDetails", connect);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    UserList.Add(

                        new UserAdmissionModel
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            Email = Convert.ToString(dr["Email"]),
                            HighSchoolName = Convert.ToString(dr["HighSchoolName"]),
                            HighSchoolGroup = Convert.ToString(dr["HighSchoolGroup"]),
                            HighSchoolMark = Convert.ToInt32(dr["HighSchoolMark"]),
                            SecondarySchoolName = Convert.ToString(dr["SecondarySchoolName"]),
                            SecondarySchoolMark = Convert.ToInt32(dr["SecondarySchoolMark"]),
                            CommunityCertificate = (byte[])(dr["CommunityCertificate"]),
                            Photo = (byte[])(dr["Photo"]),
                            Status = Convert.ToInt32(dr["Status"])
                        });
                }
                return UserList;
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// return the user course details and shoe the status 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<UserAdmissionModel> Status(int ID)
        {
            try
            {
                Connection();
                connect.Open();
                List<UserAdmissionModel> user = new List<UserAdmissionModel>();
                SqlCommand cmd = new SqlCommand("SP_UserDetails", connect);
                cmd.Parameters.AddWithValue("ID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    user.Add(
                        new UserAdmissionModel
                        {

                            ID = Convert.ToInt32(dr["ID"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            Email = Convert.ToString(dr["Email"]),
                            Courseid = Convert.ToString(dr["Courseid"]),
                            Coursename = Convert.ToString(dr["Coursename"]),
                            Program = Convert.ToString(dr["Program"]),
                            Status = Convert.ToInt32(dr["Status"])
                        });

                }

                return user;
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// retuen the list for based on the user id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProfileModel> EditUserProfile(int id)
        {
            try
            {
                Connection();
                connect.Open();

                List<ProfileModel> profile = new List<ProfileModel>();
                SqlCommand cmd = new SqlCommand("SP_UserDetails", connect);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    profile.Add(

                        new ProfileModel
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            Email = Convert.ToString(dr["Email"]),
                            HighSchoolName = Convert.ToString(dr["HighSchoolName"]),
                            HighSchoolGroup = Convert.ToString(dr["HighSchoolGroup"]),
                            HighSchoolMark = Convert.ToInt32(dr["HighSchoolMark"]),
                            SecondarySchoolName = Convert.ToString(dr["SecondarySchoolName"]),
                            SecondarySchoolMark = Convert.ToInt32(dr["SecondarySchoolMark"])
                        }); ;
                }
                return profile;
            }
            finally
            {
                connect.Close();
            }

        }
        /// <summary>
        /// update the proper values means is updated
        /// </summary>
        /// <param name="admission"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Update(ProfileModel admission ,int id)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("SP_UpdateProfile", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@FirstName", admission.FirstName);
                cmd.Parameters.AddWithValue("@LastName", admission.LastName);
                cmd.Parameters.AddWithValue("@Gender", admission.Gender);
                cmd.Parameters.AddWithValue("@Email", admission.Email);
                cmd.Parameters.AddWithValue("@HighSchoolName", admission.HighSchoolName);
                cmd.Parameters.AddWithValue("@HighSchoolGroup", admission.HighSchoolGroup);
                cmd.Parameters.AddWithValue("@HighSchoolMark", admission.HighSchoolMark);
                cmd.Parameters.AddWithValue("@SecondarySchoolName", admission.SecondarySchoolName);
                cmd.Parameters.AddWithValue("@SecondarySchoolMark", admission.SecondarySchoolMark);
    

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
        /// get the list of user details based on id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<UserAdmissionModel> ApplyUser(int ID)
        {
            try
            {
                Connection();
                connect.Open();

                List<UserAdmissionModel> profile = new List<UserAdmissionModel>();
                SqlCommand cmd = new SqlCommand("SP_SelectRegister", connect);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    profile.Add(

                        new UserAdmissionModel
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            Email = Convert.ToString(dr["Email"]),

                        }); ;
                }

                return profile;
            }
            finally
            {
                connect.Close();
            }
        }
        /// <summary>
        /// then update the other field and fetch the course id,name,program this field are get from user and update form
        /// </summary>
        /// <param name="admission"></param>
        /// <param name="Program"></param>
        /// <param name="Courseid"></param>
        /// <param name="Coursename"></param>
        /// <returns></returns>
        public bool UpdateUserAdmission(UserAdmissionModel admissionobject, int id)
        {
            try
            {
                // Open the connection
                Connection();
                connect.Open();


                SqlCommand command = new SqlCommand("sp_idcheck",connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("id",id);

                SqlDataReader re = command.ExecuteReader();
                if (re.Read())
                {
                    return false;
                }
                else
                {
                    connect.Close() ;
                    connect.Open();
                    // Check the availability of sheets for the given course
                    SqlCommand cmd = new SqlCommand("SP_AvailableSheet", connect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Courseid", admissionobject.Courseid);
                    int count = (int)cmd.ExecuteScalar();

                    // If there are more than 4 available sheets, return false
                    if (count > 4)
                    {
                        return false;
                    }
                    else
                    {
                        connect.Close();
                        // Open a new connection
                        connect.Open();

                        // Create a command to insert or update user admission data
                        SqlCommand cmd1 = new SqlCommand("SPI_User", connect);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        // Parameters for the SPI_User stored procedure
                        cmd1.Parameters.AddWithValue("@Program", admissionobject.Program);
                        cmd1.Parameters.AddWithValue("@Courseid", admissionobject.Courseid);
                        cmd1.Parameters.AddWithValue("@Coursename", admissionobject.Coursename);

                        // Parameters for the UserAdmissionmodel properties
                        cmd1.Parameters.AddWithValue("@ID", admissionobject.ID);
                        cmd1.Parameters.AddWithValue("@FirstName", admissionobject.FirstName);
                        cmd1.Parameters.AddWithValue("@LastName", admissionobject.LastName);
                        cmd1.Parameters.AddWithValue("@Gender", admissionobject.Gender);
                        cmd1.Parameters.AddWithValue("@Email", admissionobject.Email);
                        cmd1.Parameters.AddWithValue("@HighSchoolName", admissionobject.HighSchoolName);
                        cmd1.Parameters.AddWithValue("@HighSchoolGroup", admissionobject.HighSchoolGroup);
                        cmd1.Parameters.AddWithValue("@HighSchoolMark", admissionobject.HighSchoolMark);
                        cmd1.Parameters.AddWithValue("@SecondarySchoolName", admissionobject.SecondarySchoolName);
                        cmd1.Parameters.AddWithValue("@SecondarySchoolMark", admissionobject.SecondarySchoolMark);
                        cmd1.Parameters.AddWithValue("@CommunityCertificate", admissionobject.CommunityCertificate);
                        cmd1.Parameters.AddWithValue("@Photo", admissionobject.Photo);
                        cmd1.Parameters.AddWithValue("@Status", 1);
                        int i = cmd1.ExecuteNonQuery();
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
            }
            finally
            {
                connect.Close();
            }
        }
 

    }
}
