using EmployeeAttendanceSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeeAttendanceSystem.Data;

public class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();

        string[] roleNames = { "Admin", "Employee" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        // Seed Admin user
        var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
        if (adminUser == null)
        {
            adminUser = new Employee
            {
                FullName = "Admin",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };
            var createUserResult = await userManager.CreateAsync(adminUser, "Admin123");
            if (createUserResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
