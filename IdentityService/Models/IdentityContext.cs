using Microsoft.EntityFrameworkCore;

namespace IdentityService.Models
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        //пользователи
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Scope> Scope { get; set; }
        public DbSet<RoleScope> RoleScope { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //создаем индексы по полям поиска
            modelBuilder.Entity<User>().HasIndex(u => u.login).IsUnique();            

            modelBuilder.Entity<Role>().HasData(
                new Role[]
                {
                    new Role() { id=1, name="admin", authority="all", description="Администратор" },
                    new Role() { id=2, name="user", authority="statements/write", description="Пользователь" }                    
                });

            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User() { id=1, name="admin", login="admin@gmail.com", password="12345", roleid = 1 },
                    new User() { id=2, name="qwerty", login="qwerty@gmail.com", password="55555", roleid = 2 }
                });
        }
    }
}
