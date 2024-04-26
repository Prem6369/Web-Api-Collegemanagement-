using Collegemanagement.DTOs;
using Collegemanagement.Model.token;
using Collegemanagement.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Collegemanagement.Service
{
    public class JWTService : ITokenService
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public JWTService(IConfiguration configuration) //, ILogger logger
        {
            _configuration = configuration;
            //_logger = logger;
        }

        public TokenResponseDto GenerateToken(UserModel user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                int expiry = Convert.ToInt32(_configuration["Jwt:Expiry"]);

                var claims = new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier,user.Username)
                    };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(expiry),
                    signingCredentials: credentials);

                return new TokenResponseDto()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TokenResponseDto CreateToken(UserModel user)
        {
            if (isValidUser(user))
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                //var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("cid", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                        new Claim(JwtRegisteredClaimNames.Email, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     }),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinute"])),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                    //SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    //your-256-bit-secret
                    //https://dotnettutorials.net/net-6-0-create-and-validate-jwt-tokens/
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);

                return (new TokenResponseDto
                {
                    AccessToken = stringToken,
                    Expiration = token.ValidTo,

                });
            }
            return null;
        }

        public String validate(string token)
        {

            string Token = token;

            if (Token == null)
            {
                return "Unable to fetch token or Missing Token";
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    tokenHandler.ValidateToken(Token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                    var jwtToken = (JwtSecurityToken)validatedToken;

                    return "Token Validated Successfully";
                }
                catch (SecurityTokenExpiredException)
                {
                    return "Token Expired";
                }
                catch (Exception)
                {
                    return "Invalid Token";
                }

            }
        }

        public string GetUsername(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return "";
            }
            else
            {
                // Parse the token without validation to extract the "sub" claim
                var jwtToken = new JwtSecurityToken(token);
                string username = jwtToken.Subject;
                return username ?? "";
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }


        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public TokenResponseDto GetToken(IEnumerable<Claim> claim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinute"])),
                claims: claim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponseDto { AccessToken = tokenString, Expiration = token.ValidTo };
        }

        public void InsertRefreshToken(UserRefreshTokenRequest userRefreshTokenRequest)
        {
            try
            {
                String SqlconString = _configuration.GetConnectionString("connect");
                using (SqlConnection connection = new SqlConnection(SqlconString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("CreateUpdateRefreshToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", userRefreshTokenRequest.UserName);
                        command.Parameters.AddWithValue("@RefreshToken", userRefreshTokenRequest.RefreshToken);
                        command.Parameters.AddWithValue("@RefreshTokenExpiry", userRefreshTokenRequest.RefreshTokenExpiry);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        public UserRefreshTokenRequest GetRefreshTokenByUserName(string userName)
        {
            string connectionString = _configuration.GetConnectionString("connect");
            string storedProcedureName = "GetRefreshTokenByUserName";

            UserRefreshTokenRequest userToken = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userToken = new UserRefreshTokenRequest
                            {
                                UserName = reader.GetString(1),
                                RefreshToken = reader.GetString(2),
                                RefreshTokenExpiry = Convert.ToDateTime(reader.GetDateTime(3))
                            };
                        }
                    }
                }
            }

            return userToken;
        }

        public bool isValidUser(UserModel user)
        {
            bool isValid = false;
            long strkey;
            try
            { 
                string key = DecryptToPlainText(user.Key);
                if (key.Split("##").Length > 0)
                    strkey = Convert.ToInt64(key.Split("##")[1]);
                else
                    throw new SecurityTokenException("Invalid token");
                // Calculate the difference in milliseconds between the current time and the stored timestamp
                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                long storedUnixTimestamp = strkey; // Convert milliseconds to seconds
                long differenceInSeconds = (currentTimestamp - storedUnixTimestamp) / 1000;

                int maxAllowedDifference = GetMaxAllowedDifferenceFromDatabase();

                if (differenceInSeconds >= 0 && differenceInSeconds <= maxAllowedDifference)
                {
                    // User is considered valid within the specified time range
                    // Check if the user exists in the database
                    UserModel validUser = GetUsersDetails(user.Username);
                    if (validUser != null && validUser.Username == user.Username)
                        isValid = true;
                    else
                        throw new Exception("User not found");
                }
                else
                    throw new Exception("The provided key has expired");
            }
            catch (Exception ex)
            {
                // Handle exceptions here
            }
            return isValid;
        }

        private int GetMaxAllowedDifferenceFromDatabase()
        {
            string connectionString = _configuration.GetConnectionString("connect");
            string storedProcedureName = "[dbo].[sp_APIProperties]";
            int value = 0; // Initialize to a default value

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Attempt to parse the retrieved value as an integer
                            if (int.TryParse(reader["Value"].ToString(), out value))
                            {
                                // Parsing successful
                                // The 'value' variable now holds the integer value retrieved from the database
                            }
                            else
                            {
                                // Parsing failed, handle the error or log a message
                            }
                        }
                    }
                }
            }

            return value;
        }

        void ITokenService.InsertUser(UserModel user)
        {
            try
            {
                String SqlconString = _configuration.GetConnectionString("connect");
                using (SqlConnection connection = new SqlConnection(SqlconString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("CreateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", user.Username);
                        //command.Parameters.AddWithValue("@Password", user.Password);
                        //command.Parameters.AddWithValue("@Role", user.Role);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception)
            {

            }
        }
        UserModel GetUsersDetails(string userName)
        {
            string connectionString = _configuration.GetConnectionString("connect");
            string storedProcedureName = "GetUserDetailByName";

            UserModel User = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User = new UserModel
                            {
                                Username = reader.GetString(0)
                            };
                        }
                    }
                }
            }

            return User;
        }

        string ITokenService.isUserExists(string userName)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string storedProcedureName = "IsUserExists";
            string UserName = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserName = reader.GetString(0);
                        }
                    }
                }
            }
            return UserName;
        }

        public string EncryptToCiberText(string plainText)
        {
            string passPhrase = Convert.ToString(_configuration["Key:PassPhrase"]);
            string saltValue = Convert.ToString(_configuration["Key:SaltValue"]);
            string hashAlgorithm = "SHA1";
            int passwordIterations = 3;
            string initVector = "@2B3c4D4e6F6g8H9";
            int keySize = 256;

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);


            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);


            PasswordDeriveBytes password = new PasswordDeriveBytes
            (
                passPhrase,
                saltValueBytes,
                hashAlgorithm,
                passwordIterations
            );
            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor
            (
                keyBytes,
                initVectorBytes
            );

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream
            (
                memoryStream,
                encryptor,
                CryptoStreamMode.Write
            );

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        public string DecryptToPlainText(string cipherText)
        {
            string passPhrase = Convert.ToString(_configuration["Key:PassPhrase"]);
            string saltValue = Convert.ToString(_configuration["Key:SaltValue"]);

            try
            {

                string hashAlgorithm = "SHA1";
                int passwordIterations = 3;
                string initVector = "@2B3c4D4e6F6g8H9";
                int keySize = 256;

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                PasswordDeriveBytes password = new PasswordDeriveBytes(
                    passPhrase,
                    saltValueBytes,
                    hashAlgorithm,
                    passwordIterations
                );

                byte[] keyBytes = password.GetBytes(keySize / 8);

                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                        keyBytes,
                        initVectorBytes
                    ))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(
                                memoryStream,
                                decryptor,
                                CryptoStreamMode.Read
                            ))
                            {
                                using (StreamReader streamReader = new StreamReader(cryptoStream))
                                {
                                    return streamReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return cipherText;
            }
        }
    }
}
