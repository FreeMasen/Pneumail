using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pneumail.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System;


namespace Pneumail.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<EmailService> EmailServices { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<EmailFolder> EmailFolder { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public event EventHandler<PneumailDataEventArgs> Updated;
        protected virtual void OnRaiseUpdatedEvent(PneumailDataEventArgs e)
        {
            EventHandler<PneumailDataEventArgs> handler = Updated;
            if (handler != null)
            {
                handler(this, e);
            }
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
        public User GetUser(string userId)
        {
            var query = this.Users.Where(u => u.Id == userId)
                    .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Recipients)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.BlindCopied)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Copied)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Sender)
                        .Include(u => u.Categories)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Attachments)
                        .Include(u => u.Services)
                            .ThenInclude(s => s.Folders)
                        .Include(u => u.Rules);
            Console.WriteLine($"User query created");
            var user = query.First();
            Console.WriteLine("User captured");
            return user;
        }

        // public List<Category> GetCategories(string userId)
        // {

        // }

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            var changes = this.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();
            var categories = changes.Where(entity => IsType<Category>(entity)).Select(e => (Category)e.Entity).ToList();
            var msgs = changes.Where(entity => IsType<Message>(entity)).Select(e => (Message)e.Entity).ToList();
            var services = changes.Where(entity => IsType<EmailService>(entity)).Select(e => (EmailService)e.Entity).ToList();
            var rules = changes.Where(entity => IsType<Rule>(entity)).Select(e => (Rule)e.Entity).ToList();
            var jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"updateType\":");
            jsonBuilder.Append(0);
            jsonBuilder.Append(",\"");
            jsonBuilder.Append("categories");
            jsonBuilder.Append("\": [");

            for (var i = 0;i < categories.Count();i++)
            {
                var category = categories[i];
                jsonBuilder.Append(category.ToJson());
                if (i < categories.Count() - 1)
                    jsonBuilder.Append(",");
            }
            jsonBuilder.Append("],\"messages\":[");
            for (var i = 0;i < msgs.Count();i++)
            {
                var msg = msgs[i];
                jsonBuilder.Append(msg.ToJson());
                if (i < msgs.Count() - 1)
                    jsonBuilder.Append(",");
            }
            jsonBuilder.Append("],\"services\":[");
            for (var i = 0; i < services.Count();i++)
            {
                var s = services[i];
                jsonBuilder.Append(s.ToJson());
                if (i < services.Count() - 1)
                    jsonBuilder.Append(',');
            }
            jsonBuilder.Append("],\"rules\":[]}");
            var json = jsonBuilder.ToString();
            json.Replace("}{", "},{");
            Console.WriteLine(jsonBuilder.ToString());
            this.OnRaiseUpdatedEvent(new PneumailDataEventArgs(json));
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken))
        {
            this.ChangeTracker.DetectChanges();
            var changes = this.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();
            var categories = changes.Where(entity => IsType<Category>(entity)).Select(e => (Category)e.Entity).ToList();
            var msgs = changes.Where(entity => IsType<Message>(entity)).Select(e => (Message)e.Entity).ToList();
            var services = changes.Where(entity => IsType<EmailService>(entity)).Select(e => (EmailService)e.Entity).ToList();
            var rules = changes.Where(entity => IsType<Rule>(entity)).Select(e => (Rule)e.Entity).ToList();
            var json = $@"{{
                ""categories"": [{categories.Select(c => c.ToJson())}],
                ""messages"": [{msgs.Select(m => m.ToJson())}],
                ""services"": [{services.Select(s => s.ToJson())}],
                ""rules"": []
            }}";
            this.OnRaiseUpdatedEvent(new PneumailDataEventArgs(json));
            return await base.SaveChangesAsync(token);
        }

        public bool IsType<T>(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity)
        {
            return entity.CurrentValues.EntityType.Name == typeof(T).FullName;
        }

    }

    public class PneumailDataEventArgs: EventArgs
    {
        private string update;
        public string Update {
            get {
                return this.update;
            }
        }
        public PneumailDataEventArgs(string update)
        {
            this.update = update;
        }

    }
}
