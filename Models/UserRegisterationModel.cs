using FinalProject.EntityF;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class UserRegisterationModel
    {

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string phone { get; set; }

        public UserType Type { get; set; }


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Passsword { get; set; }
        [Compare(nameof(Passsword),ErrorMessage = "Password Doesn't correct")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}
