using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MMUniGraduation.Data;
using MMUniGraduation.Models;
using System.Threading.Tasks;

namespace MMUniGraduation.Controllers
{

    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public RoleController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        [Authorize]
        public async Task<IActionResult> AddUserToAdmin()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "Admin"
                });
            }
            var user = await _userManager.GetUserAsync(this.User);

            await _userManager.AddToRoleAsync(user, "Admin");

            this.TempData["Message"] = "You are added to Admin role!";

            return this.Redirect("/");
        }
    }
}

