using HMS.Models;
using Microsoft.AspNetCore.Identity;

namespace HMS.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                //Добавляем роли
                if (await roleManager.FindByNameAsync("Admin") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (await roleManager.FindByNameAsync("Editor") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Editor"));
                }
                if (await roleManager.FindByNameAsync("Guest") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Guest"));
                }
            }

            if (!userManager.Users.Any())
            {
                //Добавляем пользователей, через анонимный массив
                var users = new[]
                {
                new { Email = "sofusiay04@gmail.com", FirstName = "admin", LastName = "admin", Password = "qwerty", PhoneNumber = "+380862684823" }
                //new { Email = "alex@gmail.com", FirstName = "Alex", LastName = "Yefimova", Password = "DS(A)DS"  },
                //new { Email = "marry@in.ua", FirstName = "Marry", LastName = "Yefimova", Password = "q1d561SD"  },
                //new { Email = "tom@ukr.net", FirstName = "Tom", LastName = "Yefimova", Password = "12312DSAss"  },
                //new { Email = "john@gmail.com", FirstName = "John", LastName = "Yefimova", Password = "ds0012sd"  }
                };

                foreach (var user in users)
                {
                    if (await userManager.FindByEmailAsync(user.Email) == null)
                    {
                        User currentUser = new User { Email = user.Email, UserName = user.Email, FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber };
                        IdentityResult result = await userManager.CreateAsync(currentUser, user.Password);
                        if (result.Succeeded)
                        {
                            if (currentUser.Email.Equals("sofusiay04@gmail.com"))
                            {
                                await userManager.AddToRoleAsync(currentUser, "Admin");
                            }
                            else
                            {
                                await userManager.AddToRoleAsync(currentUser, "Editor");
                            }
                        }
                    }
                }
            }
        }

    }
}
