using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using QuanLySanBong.Models;
using System.Net;

namespace QuanLySanBong.Data
{
    public class DbInitializer
    {
        public static void Initializer(IServiceProvider serviceProvider)
        {
            using (var context = new FootballContext(serviceProvider
                .GetRequiredService<DbContextOptions<FootballContext>>()))
            {
                context.Database.EnsureCreated(); // Kiểm tra xem có cơ sở dữ liệu chưa thì thêm vào
                // look for any learners
                if (context.User.Any())
                {
                    return;
                }     
            }
        }
    }
}
