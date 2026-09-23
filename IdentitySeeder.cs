using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Employee", "Donor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }

        public static async Task SeedEmployeeAsync(
            UserManager<IdentityUser> userManager)
        {
            string email = "employee@giftofthegivers.org";
            string password = "Employee123!";

            var employee = await userManager.FindByEmailAsync(email);

            if (employee == null)
            {
                employee = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    employee,
                    password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        employee,
                        "Employee");
                }
            }
        }
    }
}