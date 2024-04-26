using Collegemanagement.DTOs;
using Collegemanagement.Model.token;
using System.Security.Claims;

namespace Collegemanagement.Service.Interface
{
    public interface ITokenService
    {

        public TokenResponseDto GenerateToken(UserModel user);
        public TokenResponseDto CreateToken(UserModel user);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public string? GetRefreshToken();
        public TokenResponseDto GetToken(IEnumerable<Claim> claim);
        public void InsertRefreshToken(UserRefreshTokenRequest userRefreshTokenRequest);
        public UserRefreshTokenRequest GetRefreshTokenByUserName(string userName);
        public void InsertUser(UserModel user);
        public string isUserExists(string userName);
        string? EncryptToCiberText(string combinedId);
    }
}
