using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class UserLoginModel
    {


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Passsword { get; set; }
    }
}
