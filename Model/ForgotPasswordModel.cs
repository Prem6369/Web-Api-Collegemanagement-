using System.ComponentModel.DataAnnotations;

namespace Collegemanagement.Model
{
    public class ForgotPasswordModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

    }
}
