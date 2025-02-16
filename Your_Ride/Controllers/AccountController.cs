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

namespace Your_Ride.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUniversityService universityService;
        private readonly ICollegeService collegeService;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IUniversityService universityService ,
                                 ICollegeService collegeService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            this.universityService = universityService;
            this.collegeService = collegeService;
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

                AccountCollegeUniversityVM  AccountCollegeUniversityVM = new AccountCollegeUniversityVM
                {
                    UniversitiesVM = universitiesVM.Select(u => new UniversityVM { Id = u.Id, Name = u.Name }).ToList(),
                    RegisterVM= model.RegisterVM
                };
                return View(AccountCollegeUniversityVM);
            }

            var user = new ApplicationUser
            {
                UserName = model.RegisterVM.UserName,
                FirstName = model.RegisterVM.FirstName,
                LastName = model.RegisterVM.LastName,
                Email = model.RegisterVM.Email,
                MobileNumber = model.RegisterVM.MobileNumber,
                NationalID = model.RegisterVM.NationalID,
                CollegeID = model.RegisterVM.CollegeID,
                batch = model.RegisterVM.Batch,
                Wallet = new Wallet { Amount = 0.0 }
            };

            var result = await _userManager.CreateAsync(user, model.RegisterVM.Password);

            if (result.Succeeded)
            {
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
                    UserName=user.UserName,
                    Roles = _userManager.GetRolesAsync(user).Result
                }).ToList(),
                Roles = roles
            };

            return View("UserRoles",model);
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
            }else
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


    }
}