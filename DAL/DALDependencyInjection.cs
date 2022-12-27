

using DAL.DataContext;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL
{
    public static class DALDependencyInjection
    {
        public static IServiceCollection AddDALDependencyInjection (this IServiceCollection services, IConfiguration configuration)
        {
           services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectioin"));
            });
            //This is for the usermanager 
            

            return services;
        }
    }
}
