using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidBack.Enities
{
    public static class DBSeeder
    {
        public static void SeedData(IServiceProvider services,
           IHostingEnvironment env,
           IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                //var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<DbRole>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                //  SeedUsers(manager, managerRole);
            }
        }
        private static void SeedProducts(ApplicationDBContext dBContext)
        {

            List<Product> products = new List<Product>()
            {
                new Product {Title="SSD",Price="1299",Url="1.jpg" },
                new Product {Title="Memory",Price="33",Url="2.jpg" },
                new Product {Title="Monitor",Price="255",Url="3.jpg" },
                new Product {Title="Videocard",Price="888",Url="4.jpg" },
                new Product {Title="DDR4",Price="88888",Url="5.jpg" },
                new Product {Title="LG",Price="1299",Url="6.jpg" },
                new Product {Title="DELL",Price="345",Url="7.jpg" },
                new Product {Title="HP",Price="87888",Url="8.jpg" },
                new Product {Title="Apple",Price="999",Url="9.jpg" },
                new Product {Title="Iphone",Price="1111",Url="10.jpg" },
                new Product {Title="Processor",Price="4444",Url="11.jpg" },
            };



        }


        //private static void SeedUsers(UserManager<DbUser> userManager,
        //    RoleManager<DbRole> roleManager)
        //{
        //    var roleName = "Admin";
        //    if (roleManager.FindByNameAsync(roleName).Result == null)
        //    {
        //        var result = roleManager.CreateAsync(new DbRole
        //        {
        //            Name = roleName
        //        }).Result;
        //    }

        //    if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
        //    {
        //        string email = "admin@gmail.com";
        //        var user = new DbUser
        //        {
        //            Email = email,
        //            UserName = email,
        //            PhoneNumber = "+11(111)111-11-11"
        //        };
        //        var result = userManager.CreateAsync(user, "8Ki9x9-3of+s222").Result;
        //        result = userManager.AddToRoleAsync(user, roleName).Result;
        //    }
        //    if (userManager.FindByEmailAsync("novakvova@gmail.com").Result == null)
        //    {
        //        string email = "novakvova@gmail.com";
        //        var user = new DbUser
        //        {
        //            Email = email,
        //            UserName = email,
        //            PhoneNumber = "+21(111)111-11-11"
        //        };
        //        var result = userManager.CreateAsync(user, "R2-=x*x1PxsE2219").Result;
        //        result = userManager.AddToRoleAsync(user, roleName).Result;
        //    }
        // }
    }
}
