using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pneumail.Models;

namespace Pneumail.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories;
        public DbSet<Message> Messages;
        public DbSet<EmailService> EmailServices;
        public DbSet<EmailAddress> EmailAddresses;
        public DbSet<Attachment> Attachments;
        public DbSet<EmailFolder> EmailFolder;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Category>(cat => {

            });
        }
    }
}
