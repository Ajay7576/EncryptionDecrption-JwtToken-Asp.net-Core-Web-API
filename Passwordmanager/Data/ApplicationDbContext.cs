using Microsoft.EntityFrameworkCore;
using Passwordmanager.Model;

namespace Passwordmanager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<PasswordManager> PasswordManagers  { get; set; }




    }
}
