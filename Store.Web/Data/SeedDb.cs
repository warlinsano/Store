using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Web.Data.Entities;
using Store.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Data
{
    public class SeedDb
    {
        private static  DataContext _dataContext;
        private static RoleManager<IdentityRole> _roleManager;

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            //var context = serviceProvider.GetRequiredService<DataContext>();
            //context.Database.Migrate();

            SeedDb._roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            using (var context = new DataContext(serviceProvider
                .GetRequiredService<DbContextOptions<DataContext>>()))
            {
                context.Database.Migrate();
                _dataContext = context;
                await CheckCategories();
                await CheckRolesAsync();
            }

        }


        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    //initializing custom roles 
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] roleNames = { "Admin", "Manager", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            //create the roles and seed them to the database: Question 1
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }

        //    //Here you could create a super user who will maintain the web app
        //    var poweruser = new ApplicationUser
        //    {

        //        UserName = Configuration["AppSettings:UserName"],
        //        Email = Configuration["AppSettings:UserEmail"],
        //    };
        //    //Ensure you have these values in your appsettings.json file
        //    string userPWD = Configuration["AppSettings:UserPassword"];
        //    var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

        //    if (_user == null)
        //    {
        //        var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
        //        if (createPowerUser.Succeeded)
        //        {
        //            //here we tie the new user to the role
        //            await UserManager.AddToRoleAsync(poweruser, "Admin");

        //        }
        //    }
        //}

        private static async Task CheckRolesAsync()
        {

            //initializing custom roles 
            //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Manager", "Customer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //if (_dataContext.Roles.Any())
            //{
            //    return;
            //}
            //_roleManager.RoleExistsAsync()
            //await _dataContext.Roles.AddAsync(new IdentityRole("Customer"));
            //await _dataContext.Roles.AddAsync(new IdentityRole("Admin"));

            //await _userHelper.CheckRoleAsync("Admin");
            //await _userHelper.CheckRoleAsync("Customer");
        }

        private static async Task CheckCategories()
        {
            if (_dataContext.Categories.Any())
            {
                return;  
            }

            //await context.Database.EnsureCreatedAsync();
            await _dataContext.Categories.AddAsync(
                 new Categories
                 {
                     CategoryName = "vegetales",
                     Description = "vegetales"
                  });

            await _dataContext.SaveChangesAsync();
        }

        //public async Task SeedAsync()
        //{
        //    await _dataContext.Database.EnsureCreatedAsync();
        //    //await CheckRoles();
        //    //var manager = await CheckUserAsync("1010", "Juan", "Zuluaga", "jzuluaga55@gmail.com", "350 634 2747", "Calle Luna Calle Sol", "Admin");
        //    //var customer = await CheckUserAsync("2020", "Juan", "Zuluaga", "jzuluaga55@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", "Customer");
        //    //await CheckPetTypesAsync();
        //    //await CheckServiceTypesAsync();
        //    //await CheckOwnerAsync(customer);
        //    //await CheckManagerAsync(manager);
        //    //await CheckPetsAsync();
        //    //await CheckAgendasAsync();
        //}
    }
}
