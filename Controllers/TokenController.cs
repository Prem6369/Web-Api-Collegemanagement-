using Collegemanagement.DTOs;
using Collegemanagement.Model.token;
using Collegemanagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Collegemanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly JWTSettings _jWTSettings;
        private readonly ITokenService _tokenService;
        public TokenController(JWTSettings jWTSettings, ITokenService tokenService)
        {
            _jWTSettings = jWTSettings;
            _tokenService = tokenService;
        }

        [HttpPost("GetToken")]
        public IActionResult GenerateToken([FromBody] UserModel userModel)
        {
            var token = _tokenService.CreateToken(userModel);
            var refreshToken = _tokenService.GetRefreshToken();

            UserRefreshTokenRequest userRefreshTokenRequest = new UserRefreshTokenRequest();
            userRefreshTokenRequest.UserName = userModel.Username;
            userRefreshTokenRequest.RefreshToken = refreshToken;

            if (token == null)
                return BadRequest("User not found");

            userRefreshTokenRequest.RefreshTokenExpiry = token.Expiration;

            _tokenService.InsertRefreshToken(userRefreshTokenRequest);

            return Ok(new TokenResponseDto
            {
                AccessToken = token.AccessToken,
                RefreshToken = refreshToken,
                Expiration = token.Expiration,
            });
        }


        [HttpPost]
        [Route("GetRefreshToken")]
        public IActionResult Refresh(RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                if (refreshTokenRequest is null)
                    return BadRequest("Invalid client request");

                string accessToken = refreshTokenRequest.AccessToken;
                string refreshToken = refreshTokenRequest.RefreshToken;


                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                var username = principal.Claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value;

                UserRefreshTokenRequest userRefreshTokenRequest = new UserRefreshTokenRequest();
                userRefreshTokenRequest = _tokenService.GetRefreshTokenByUserName(username);

                if (userRefreshTokenRequest.RefreshToken != refreshTokenRequest.RefreshToken)
                {
                    return BadRequest("Invalid Token");
                }

                userRefreshTokenRequest = new UserRefreshTokenRequest();

                var newAccessToken = _tokenService.GetToken(principal.Claims);
                var newRefreshToken = _tokenService.GetRefreshToken();

                userRefreshTokenRequest.UserName = username;
                userRefreshTokenRequest.RefreshToken = newRefreshToken;
                userRefreshTokenRequest.RefreshTokenExpiry = newAccessToken.Expiration;

                _tokenService.InsertRefreshToken(userRefreshTokenRequest);

                return Ok(new RefreshTokenRequest()
                {
                    AccessToken = newAccessToken.AccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid Request");
            }
        }

        [HttpPost]
        [Route("RevokeToken")]
        public IActionResult Revoke(string AccessToken)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(AccessToken);
                var username = principal.Claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value;

                UserRefreshTokenRequest userRefreshTokenRequest = new UserRefreshTokenRequest();
                userRefreshTokenRequest = _tokenService.GetRefreshTokenByUserName(username);

                if (userRefreshTokenRequest is null)
                    return BadRequest();
                userRefreshTokenRequest.RefreshToken = "";
                _tokenService.InsertRefreshToken(userRefreshTokenRequest);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("InstantKeyGenerate")]
        public IActionResult ShareKey()
        {
            try
            {
                Guid uuid = Guid.NewGuid();
                string shortUuid = uuid.ToString().Substring(0, 2);
                // Get Unix timestamp
                long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // Combine UUID and Unix timestamp
                string combinedId = $"{uuid}##{unixTimestamp}";

                // Encrypt the combinedId
                //string encryptedId = EncryptToCiberText(combinedId);

                return Ok("Key = " + combinedId + ", Encrypted Key = " + _tokenService.EncryptToCiberText(combinedId));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

