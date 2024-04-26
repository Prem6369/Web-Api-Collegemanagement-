using System.ComponentModel.DataAnnotations;

namespace Collegemanagement.Model.token
{
    public class UserModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        public string Key { get; set; }
    }
}
