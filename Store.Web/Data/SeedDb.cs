using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Data
{
    public class SeedDb
    {
        private static  DataContext _dataContext;

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            //var context = serviceProvider.GetRequiredService<DataContext>();
            //context.Database.Migrate();

            using (var context = new DataContext(serviceProvider
                .GetRequiredService<DbContextOptions<DataContext>>()))
            {
                context.Database.Migrate();
                _dataContext = context;
                await CheckCategories();
            }
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
