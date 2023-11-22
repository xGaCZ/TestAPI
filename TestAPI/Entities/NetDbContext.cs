using Microsoft.EntityFrameworkCore;
using TestNetAPI.Entities;

namespace TestNetAPI.Entities
{
    public class NetDbContext : DbContext
    {
       public NetDbContext(DbContextOptions<NetDbContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
       
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<Role>().Property(z => z.Name).IsRequired();
            modelBuilder.Entity<Contact>().HasOne(c => c.Detail).WithOne(x => x.Contact).HasForeignKey<Detail>(x => x.ContactID);
            modelBuilder.Entity<Contact>().Property(a => a.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Contact>().Property(a => a.Phone).IsRequired();


        }

    }
}
