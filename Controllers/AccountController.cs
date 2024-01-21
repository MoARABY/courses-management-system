using FinalProject.EntityF;

using FinalProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _mng;
        private SignInManager<User> _signInManager;
        private AppDbContext _dbContext;
        public AccountController(UserManager<User> UserMng, SignInManager<User> signInManager, AppDbContext dbContext)
        {
            _mng = UserMng;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(UserRegisterationModel R)
        {
            if (!ModelState.IsValid)
            { return View(R); }

            var NewUser = new User { User_Name = R.First_Name+" "+R.Last_Name,UserName = R.Email, Email = R.Email ,Type=R.Type,PhoneNumber=R.phone};
            var Result = _mng.CreateAsync(NewUser, R.Passsword).Result;

           Result= _mng.AddToRoleAsync(NewUser, Enum.GetName(typeof(UserType), R.Type)).Result;

            if (Result.Succeeded == false)
            {
                foreach (var item in Result.Errors)
                {
                    //ModelState.TryAddModelError("Error",error);
                }
                return View(R);
            }
            if (R.Type == UserType.Student)
            {
                var std = new Student { Student_Name = R.First_Name + " " + R.Last_Name, Phone = R.phone };
                _dbContext.Add(std);
                _dbContext.SaveChanges();
                NewUser.User_ID = std.Student_ID;
            }
            else 
            {
                var Ins = new Instuctor { Instructur_Name = R.First_Name + " " + R.Last_Name,Phone=R.phone };
                _dbContext.Add(Ins);
                _dbContext.SaveChanges();
                NewUser.User_ID = Ins.Instructur_ID;
            }
            Result= _mng.UpdateAsync(NewUser).Result;
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginModel _userlogin)
        {

            if (!ModelState.IsValid)
            { return View(_userlogin); }
            var user = _mng.FindByEmailAsync(_userlogin.Email).Result;
            if (user != null && _mng.CheckPasswordAsync(user, _userlogin.Passsword).Result == true)
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            var roles =_mng.GetRolesAsync(user).Result;

                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }
                identity.AddClaim(new Claim("User_ID", user.User_ID.ToString()));
                HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
               return  RedirectToAction("Index","Course");
            }
            else 
            {
                ModelState.AddModelError("Error", "Invalid UserName or Password");
                return View();
            }
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {

          await  _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
