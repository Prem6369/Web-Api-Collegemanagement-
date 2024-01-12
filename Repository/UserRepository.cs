using Collegemanagement.Model;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Collegemanagement.Repository
{
    public class UserRepository
    {
        private IConfiguration _configuration;
        private SqlConnection connect;

        public UserRepository(IConfiguration configuration)
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
        /// verify the user based on our credential and redirect to our respected page 
        /// </summary>
        /// <param name="registermodel"></param>
        /// <param name="id"></param>
        /// <param name="result"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool VerifySignIn(CredentialModel registermodel, out int id, out int result, out string name)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_LoginAdminUser", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", registermodel.Email);
                command.Parameters.AddWithValue("@Password", Encrypt(registermodel.Password));
                connect.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = (int)reader["Role"];
                    id = (int)reader["Id"];
                    name = (string)reader["FirstName"];

                    return true;
                }
                else
                {
                    result = 0;
                    id = 0;
                    name = "!!!!";
                    return true;
                }
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
        /// signup page create the new user 
        /// </summary>
        /// <param name="college"></param>
        /// <returns></returns>
        public bool NewUser(RegisterModel college)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_CheckEmail", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", college.Email);
                connect.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read() == true)
                {
                    return false;
                }
                else
                {
                    connect.Close();
                    SqlCommand command1 = new SqlCommand("SPI_Register", connect);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@Role", 1);
                    command1.Parameters.AddWithValue("@FirstName", college.FirstName);
                    command1.Parameters.AddWithValue("@LastName", college.LastName);
                    command1.Parameters.AddWithValue("@DateOfBirth", college.DateOfBirth);
                    command1.Parameters.AddWithValue("@Age", college.Age);
                    command1.Parameters.AddWithValue("@Gender", college.Gender);
                    command1.Parameters.AddWithValue("@PhoneNumber", college.PhoneNumber);
                    command1.Parameters.AddWithValue("@Address", college.Address);
                    command1.Parameters.AddWithValue("@State", college.State);
                    command1.Parameters.AddWithValue("@City", college.City);
                    command1.Parameters.AddWithValue("@Email", college.Email);
                    command1.Parameters.AddWithValue("@Password", Encrypt(college.Password));

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
        /// forgotpassword if user loss the password means changing password based on phonenumbber and email match
        /// </summary>
        /// <param name="forgot"></param>
        /// <returns></returns>
        public bool ForgotPassword(ForgotPasswordModel forgot)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_Forgetpasswoxrd", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", forgot.Email);
                command.Parameters.AddWithValue("@Phonenumber", forgot.PhoneNumber);
                command.Parameters.AddWithValue("@Password", Encrypt(forgot.Password));
                connect.Open();
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
        /// contact form user can contact the admin means show this page
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>

        public bool Contact(ContactModel contact)
        {
            try
            {
                Connection();
                SqlCommand command = new SqlCommand("SP_Contact", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Fullname", contact.Name);
                command.Parameters.AddWithValue("@Email", contact.Email);
                command.Parameters.AddWithValue("@Phonenumber", contact.Phonenumber);
                command.Parameters.AddWithValue("@Message", contact.Message);
                connect.Open();
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
    }

}
