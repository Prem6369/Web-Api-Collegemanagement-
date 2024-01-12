using System.ComponentModel.DataAnnotations;

namespace Collegemanagement.Model
{
    public class CredentialModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
