using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Your_Ride.Models;
using Your_Ride.ViewModels;
using Your_Ride.ViewModels.Account;
using Your_Ride.Services.UniversityServ;
using Your_Ride.Services.CollegeServ;
using Microsoft.Identity.Client;
using Your_Ride.ViewModels.University;
using Your_Ride.ViewModels.College;
using Your_Ride.ViewModels.Account;
using Microsoft.AspNetCore.Identity.UI.Services;
using Your_Ride.Repository.ForgetPasswordRepo;
using System.Net.Mail;
using System.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Your_Ride.Services.WalletServ;
using Your_Ride.Services.BookServ;
using Your_Ride.Services.UserTransactionLogServ;
using Your_Ride.ViewModels.UserTransactionLogViewModel;
using Your_Ride.ViewModels.BookViewModel;
using Your_Ride.ViewModels.WalletViewModel;
using AutoMapper;

namespace Your_Ride.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUniversityService universityService;
        private readonly ICollegeService collegeService;
        private readonly IConfiguration configuration;
        private readonly IWalletService walletService;
        private readonly IBookService bookService;
        private readonly IUserTransactionLogService transactionLogService;
        private readonly IMapper automapper;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IUniversityService universityService,
                                 ICollegeService collegeService ,
                                 IConfiguration configuration , 
                                 IWalletService walletService ,
                                 IBookService bookService ,
                                 IUserTransactionLogService transactionLogService,
                                 IMapper automapper )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            this.universityService = universityService;
            this.collegeService = collegeService;
            this.configuration = configuration;
            this.walletService = walletService;
            this.bookService = bookService;
            this.transactionLogService = transactionLogService;
            this.automapper = automapper;
        }


        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            // Get the user from the database
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return RedirectToAction("Error", "Home");

            // Retrieve user booking data
            List<BookVM> bookVMs = await bookService.GetAllBooksOfUser(id);
            Wallet wallet = await walletService.getWalletByUserID(id);

            // Ensure Bookings is never null
            if (bookVMs == null || bookVMs.Count == 0)
                bookVMs = new List<BookVM>();  // Initialize if null or empty

            // Map the BookVM list to Book entities
            List<Book> books = automapper.Map<List<Book>>(bookVMs);
            user.Bookings = books ?? new List<Book>();  // Safeguard to ensure Bookings is always a list

            // Set Wallet (ensure it is not null)
            user.Wallet = wallet ?? new Wallet();

            return View("Profile" , user);
        }






        // /Account/Register
        // GET: Register View
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var universities = await universityService.GetAllUniversity();

            var model = new AccountCollegeUniversityVM
            {
                UniversitiesVM = universities.Select(u => new UniversityVM { Id = u.Id, Name = u.Name }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountCollegeUniversityVM model)
        {
            if (!ModelState.IsValid)
            {
                List<UniversityVM> universitiesVM = await universityService.GetAllUniversity();

                AccountCollegeUniversityVM AccountCollegeUniversityVM = new AccountCollegeUniversityVM
                {
                    UniversitiesVM = universitiesVM.Select(u => new UniversityVM { Id = u.Id, Name = u.Name }).ToList(),
                    RegisterVM = model.RegisterVM
                };
                return View(AccountCollegeUniversityVM);
            }
            var existingUser = await _userManager.FindByNameAsync(model.RegisterVM.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("RegisterVM.UserName", "This Username is already taken.");

                var universitiesVM = await universityService.GetAllUniversity();
                AccountCollegeUniversityVM CollegeUniversityVM = new AccountCollegeUniversityVM
                {
                    UniversitiesVM = universitiesVM.Select(u => new UniversityVM { Id = u.Id, Name = u.Name }).ToList(),
                    RegisterVM = model.RegisterVM
                };
                return View(CollegeUniversityVM);
            }
            // Check if email already exists
            var existingEmail = await _userManager.FindByEmailAsync(model.RegisterVM.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("RegisterVM.Email", "This Email is already in use.");

                var universitiesVM = await universityService.GetAllUniversity();
                AccountCollegeUniversityVM CollegeUniversityVM = new AccountCollegeUniversityVM
                {
                    UniversitiesVM = universitiesVM.Select(u => new UniversityVM { Id = u.Id, Name = u.Name }).ToList(),
                    RegisterVM = model.RegisterVM
                };
                return View(CollegeUniversityVM);
            }
            var user = new ApplicationUser
            {
                UserName = model.RegisterVM.UserName,
                FirstName = model.RegisterVM.FirstName,
                LastName = model.RegisterVM.LastName,
                Email = model.RegisterVM.Email,
                PhoneNumber = model.RegisterVM.PhoneNumber,
                NationalID = model.RegisterVM.NationalID,
                FavoriteColor = model.RegisterVM.FavoriteColor ,
                CollegeID = model.RegisterVM.CollegeID,
                batch = model.RegisterVM.Batch,
                Wallet = new Wallet { Amount = 0.0 }
            };

            var result = await _userManager.CreateAsync(user, model.RegisterVM.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful! You can now log in.";

                // Check if the Admin role exists, create if not
                var adminRoleExists = await _roleManager.RoleExistsAsync("Admin");
                if (!adminRoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // Assign Admin role to the first user or User role to others
                var hasAdmin = (await _userManager.GetUsersInRoleAsync("Admin")).Any();

                if (!hasAdmin)
                {
                    // If no user has the "User" role, assign it to this user
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    // Check if the Admin role exists, create if not
                    var userRoleExists = await _roleManager.RoleExistsAsync("User");
                    if (!userRoleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }
                    // If an User exists, assign "User" role to the new user
                    await _userManager.AddToRoleAsync(user, "User");
                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            var universities = await universityService.GetAllUniversity();

            AccountCollegeUniversityVM accountCollegeUniversityVM = new AccountCollegeUniversityVM
            {
                UniversitiesVM = universities.Select(u => new UniversityVM { Id = u.Id, Name = u.Name }).ToList(),
                RegisterVM = model.RegisterVM
            };
            return View(accountCollegeUniversityVM);
        }

        [HttpGet]
        public async Task<JsonResult> GetColleges(int universityId)
        {
            var colleges = await collegeService.GetCollegesByUniversityId(universityId);
            return Json(colleges);
        }

        [HttpGet]
        public async Task<JsonResult> GetBatches(int collegeId)
        {
            var college = await collegeService.GetCollegeById(collegeId);
            return Json(college?.Batches ?? new List<string>());
        }


        // Login an existing user
        // /Account/Login
        public IActionResult Login()

        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(loginUserVM loginUserVM)
        {
            if (ModelState.IsValid)
            {
                // Check if user exists
                ApplicationUser applicationUser = await _userManager.FindByNameAsync(loginUserVM.UserName);

                if (applicationUser != null)
                {
                    // Check password
                    bool isCorrect = await _userManager.CheckPasswordAsync(applicationUser, loginUserVM.Password);

                    if (isCorrect)
                    {
                        // Check if the user is in the Admin role
                        var roles = await _userManager.GetRolesAsync(applicationUser);
                        var isAdmin = roles.Contains("Admin");

                        // Add claims for the user
                        List<Claim> claims = new List<Claim>
                              {
                                  new Claim("Company_Employees", "Welcome"),
                                  new Claim(ClaimTypes.Name, applicationUser.UserName),
                                  new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
                                  new Claim(ClaimTypes.Email, applicationUser.Email)
                               };

                        // If the user is an Admin, add the role claim to the user's session
                        if (isAdmin)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "User"));
                        }

                        // Create cookie with claims
                        await _signInManager.SignInWithClaimsAsync(applicationUser, loginUserVM.RememberMe, claims);

                        // Get Info from cookie about the logged-in user
                        string userName = User.Identity.Name;
                        Claim claimId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        if (claimId != null)
                        {
                            string userId = claimId.Value;
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid Account");
            }

            return View(loginUserVM);
        }


        // Logout the current user
        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // List users with roles
        public async Task<IActionResult> UserRoles()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new UserRolesVM
            {
                Users = users.Select(user => new UserVM
                {
                    UserId = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = _userManager.GetRolesAsync(user).Result
                }).ToList(),
                Roles = roles
            };

            return View("UserRoles", model);
        }

        // Assign a role to a user
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(role))
            {
                return NotFound();
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(UserRoles));
            }

            ModelState.AddModelError("", "Failed to assign role.");
            return RedirectToAction(nameof(UserRoles));
        }

        // Remove a role from a user
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(role))
            {
                return NotFound();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(UserRoles));
            }

            ModelState.AddModelError("", "Failed to remove role.");
            return RedirectToAction(nameof(UserRoles));
        }
        // Acount/AddRole
        [HttpGet]
        public IActionResult AddRole()
        {
            return View("AddRole");
        }


        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleVM model)
        {
            if (string.IsNullOrWhiteSpace(model.RoleName))
            {
                ModelState.AddModelError("", "Role name is required.");
                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                var role = new IdentityRole(model.RoleName);
                await _roleManager.CreateAsync(role);
            }
            else
            {
                return Content("Already Existed");
            }

            return RedirectToAction(nameof(UserRoles));
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return NotFound();

            var model = new EditRoleVM { RoleName = role.Name };
            return View("EditRole", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM model)
        {
            if (model.RoleName == model.NewRoleName)
            {
                ModelState.AddModelError("", "The new role name cannot be the same as the old one.");
                return View(model);
            }

            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null) return NotFound();

            var CheckNewRoleDB = await _roleManager.FindByNameAsync(model.NewRoleName);

            if (CheckNewRoleDB != null)
            {
                return Content("Already Existed");
            }
            role.Name = model.NewRoleName;
            await _roleManager.UpdateAsync(role);

            return RedirectToAction(nameof(UserRoles));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return NotFound();

            var model = new DeleteRoleVM { RoleName = role.Name };
            return View("DeleteRole", model);
        }

        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteRoleConfirmed(DeleteRoleVM model)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null) return NotFound();

            await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(UserRoles));
        }
        [HttpGet]
        // Show Forgot Password View
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }
        [HttpGet]
        public async Task<IActionResult> GetUserMaskedDetails(string username)
        {
            if (string.IsNullOrEmpty(username))
                return Json(new { success = false });

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return Json(new { success = false });

            // Masking Email
            string maskedEmail = null;
            if (!string.IsNullOrEmpty(user.Email))
            {
                int atIndex = user.Email.IndexOf('@');
                if (atIndex > 4)
                {
                    maskedEmail = "****" + user.Email.Substring(atIndex - 4);
                }
                else
                {
                    maskedEmail = "****" + user.Email;
                }
            }

            // Masking Phone
            string maskedPhone = null;
            if (!string.IsNullOrEmpty(user.PhoneNumber) && user.PhoneNumber.Length > 4)
            {
                maskedPhone = "****" + user.PhoneNumber[^4..];
            }

            return Json(new { success = true, maskedEmail, maskedPhone });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || user.FavoriteColor.ToLower() != model.FavoriteColor.ToLower())
            {
                ModelState.AddModelError("", "Invalid username or favorite color.");
                return View(model);
            }

            return RedirectToAction("ResetPassword", new { userId = user.Id });
        }


        #region Old Forget Password
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        //{
        //    // Remove errors related to MaskedEmail and MaskedPhone
        //    ModelState.Remove(nameof(model.MaskedEmail));
        //    ModelState.Remove(nameof(model.MaskedPhone));
        //    ModelState.Remove(nameof(model.OTP));


        //    if (!ModelState.IsValid) return View(model);

        //    // Find User by Username
        //    var user = await _userManager.FindByNameAsync(model.UserName);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "User not found.");
        //        return View(model);
        //    }

        //    // Generate OTP & Expiry
        //    var otp = new Random().Next(100000, 999999).ToString();
        //    user.OTPCode = otp;
        //    user.OTPExpiry = DateTime.UtcNow.AddMinutes(5);
        //    await _userManager.UpdateAsync(user);

        //    // Masked Details
        //    model.MaskedEmail = "****" + user.Email.Substring(4);
        //    model.MaskedPhone = "+****" + user.PhoneNumber.Substring(user.PhoneNumber.Length - 4);

        //    // Send OTP via Email or SMS
        //    if (model.SelectedMethod == "Email")
        //    {
        //        SendEmail(user.Email, "Your OTP Code", $"Your OTP Code is: {otp}");
        //    }
        //    else
        //    {
        //        SendSMS(user.PhoneNumber, $"Your OTP Code is: {otp}");
        //    }

        //    // Redirect to Verify OTP Page
        //    return RedirectToAction("VerifyOTP", new { userId = user.Id, method = model.SelectedMethod });
        //} 
        #endregion

        // Verify OTP View
        public IActionResult VerifyOTP(string userId, string method)
        {
            return View("VerifyOTP", new ResetPasswordVM { UserId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOTP(ResetPasswordVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || user.OTPCode != model.OTP || user.OTPExpiry < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Invalid or expired OTP.");
                return View(model);
            }

            // OTP is correct → Redirect to Reset Password
            return RedirectToAction("ResetPassword", new { userId = user.Id });
        }

        // Show Reset Password View
        public IActionResult ResetPassword(string userId)
        {
            return View("ResetPassword", new ResetPasswordVM { UserId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {

            ModelState.Remove(nameof(model.OTP));
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            // Reset Password
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Error resetting password.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        // Send OTP via Email
        private void SendEmail(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password"),
                EnableSsl = true,
            };

            smtpClient.Send("your-email@gmail.com", toEmail, subject, body);
        }

        // Send OTP via Twilio SMS
        private void SendSMS(string phoneNumber, string message)
        {
            var accountSid = configuration["Twilio:AccountSid"];
            var authToken = configuration["Twilio:AuthToken"];
            var fromNumber = configuration["Twilio:FromNumber"];

            TwilioClient.Init(accountSid, authToken);

            MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }
    }
}