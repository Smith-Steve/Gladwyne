using Gladwyne.Models;
using Microsoft.EntityFrameworkCore;

namespace Gladwyne.API.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _configuration;
        public DataContextEF(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        // public virtual DbSet<OrganizationDTO> OrganizationsDTO { get; set;}
        public virtual DbSet<OrganizationAddress> OrganizationsAddress { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        // public virtual DbSet<ContactDTO> ContactDTO{ get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("GladwyneSchema");

            modelBuilder.Entity<User>()
                .ToTable("Users", "GladwyneSchema") //Our table name is different than the model. So we need to add this line.
                .HasKey(user => user.UserId);
            
            modelBuilder.Entity<Organization>()
                .HasKey(organization => organization.OrgId);
            
            modelBuilder.Entity<OrganizationAddress>()
                .HasKey(organization => organization.OrgId);
            
            modelBuilder.Entity<Contact>()
                .HasKey(contact => contact.ContactId);
        }
    }
}