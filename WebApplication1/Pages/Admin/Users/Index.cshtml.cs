using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Admin.Users
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserViewModel> Users { get; set; } = new();
        public List<IdentityRole> Roles { get; set; } = new();
        [BindProperty]
        public string? RoleName { get; set; }
        [BindProperty]
        public string? UserId { get; set; }
        public string? RoleMessage { get; set; }

        public class UserViewModel
        {
            public string Id { get; set; } = "";
            public string UserName { get; set; } = "";
            public string Email { get; set; } = "";
            public IList<string> Roles { get; set; } = new List<string>();
        }

        public async Task OnGetAsync()
        {
            await LoadData();
        }

        public async Task<IActionResult> OnPostCreateRoleAsync()
        {
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                RoleMessage = "Role name cannot be empty.";
                await LoadData();
                return Page();
            }

            if (await _roleManager.RoleExistsAsync(RoleName))
            {
                RoleMessage = "Role already exists.";
                await LoadData();
                return Page();
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
            RoleMessage = result.Succeeded ? "Role created successfully." : "Error creating role.";
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                RoleMessage = result.Succeeded ? "Role deleted successfully." : "Error deleting role.";
            }
            else
            {
                RoleMessage = "Role not found.";
            }
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToRoleAsync()
        {
            if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(RoleName))
            {
                RoleMessage = "User and role are required.";
                await LoadData();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                RoleMessage = "User not found.";
                await LoadData();
                return Page();
            }

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                RoleMessage = "Role does not exist.";
                await LoadData();
                return Page();
            }

            var result = await _userManager.AddToRoleAsync(user, RoleName);
            RoleMessage = result.Succeeded ? "Role added to user." : "Error adding role.";
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFromRoleAsync()
        {
            if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(RoleName))
            {
                RoleMessage = "User and role are required.";
                await LoadData();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                RoleMessage = "User not found.";
                await LoadData();
                return Page();
            }

            if (!await _roleManager.RoleExistsAsync(RoleName))
            {
                RoleMessage = "Role does not exist.";
                await LoadData();
                return Page();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
            RoleMessage = result.Succeeded ? "Role removed from user." : "Error removing role.";
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                RoleMessage = result.Succeeded ? "User deleted successfully." : "Error deleting user.";
            }
            else
            {
                RoleMessage = "User not found.";
            }
            await LoadData();
            return Page();
        }

        private async Task LoadData()
        {
            Roles = _roleManager.Roles.ToList();
            Users = new List<UserViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    Roles = roles
                });
            }
        }
    }
}