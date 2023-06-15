using ECinema.Domain.DTO;
using ECinema.Domain.Identity;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECinema.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ECinemaApplicationUser> userManager;
        private readonly SignInManager<ECinemaApplicationUser> signInManager;
        public AccountController(UserManager<ECinemaApplicationUser> userManager,
            SignInManager<ECinemaApplicationUser> signInManager)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            UserRegistrationDto model = new UserRegistrationDto();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new ECinemaApplicationUser
                    {
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCart = new Domain.DomainModels.ShoppingCart()
                    };
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        ECinemaApplicationUser tmp = await userManager.FindByEmailAsync(request.Email);
                        await userManager.AddToRoleAsync(tmp, "User");

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Tickets");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            ICollection<ECinemaApplicationUser> users = userManager.Users.ToList();
            UserToRole model = new UserToRole();
            model.userEmails = users;
            model.userRoles.Add("Admin");
            model.userRoles.Add("User");

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRole(UserToRole model)
        {
            ECinemaApplicationUser user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Dashboard");
            }

            var toDelete = string.Join(" ", await userManager.GetRolesAsync(user));

            await userManager.RemoveFromRoleAsync(user, toDelete);
            await userManager.AddToRoleAsync(user, model.selectedRole);

            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }

        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";
            bool status = true;
            List<UserImportDto> users = new List<UserImportDto>();

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        users.Add(new UserImportDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            PhoneNumber = "No Phone",
                            NormalizedUserName = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()
                        });
                    }
                }
            }

            foreach (UserImportDto tmp in users)
            {
                var userCheck = await userManager.FindByEmailAsync(tmp.Email);
                if (userCheck == null)
                {
                    var user = new ECinemaApplicationUser
                    {
                        UserName = tmp.Email,
                        NormalizedUserName = tmp.Email,
                        Email = tmp.Email,
                        PhoneNumber = tmp.PhoneNumber,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        UserCart = new Domain.DomainModels.ShoppingCart()
                    };
                    var result = await userManager.CreateAsync(user, tmp.Password);
                    if (result.Succeeded)
                    {
                        ECinemaApplicationUser usr = await userManager.FindByEmailAsync(tmp.Email);
                        await userManager.AddToRoleAsync(usr, tmp.Role);
                    }
                    else
                    {
                        status = false;
                    }
                }
            }

            if (status)
            {
                return RedirectToAction("Dashboard");
            }

            return NotFound();
        }
    }
}
