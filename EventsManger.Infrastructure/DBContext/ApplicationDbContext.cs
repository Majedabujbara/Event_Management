using EventManger.Core.Domain.Entites;
using EventManger.Core.Domain.Entities;
using EventManger.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace EventManger.Infrastructure.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Room> rooms { get; set; }
        public virtual DbSet<ApplicationUser> eventsAttendees { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<NewsArticle> NewsArticles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<BlogPost> blogPosts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        // Override OnConfiguring to enable lazy loading
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Get the configuration (e.g., from appsettings.json)
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                // Configure the database provider and enable lazy loading
                optionsBuilder
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLazyLoadingProxies(); // Enable lazy loading
            }
        }

        // Seeding with mock data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<Organization>().ToTable("Organizations");

            // Seed data (fixed)
            var roleAdminId = Guid.NewGuid();
            var roleAttendeeId = Guid.NewGuid();
            var roleGuestId = Guid.NewGuid();

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = roleAdminId, Name = "Admin", NormalizedName = "ADMIN" },
                new ApplicationRole { Id = roleAttendeeId, Name = "Attendee", NormalizedName = "ATTENDEE" },
                new ApplicationRole { Id = roleGuestId, Name = "Guest", NormalizedName = "GUEST" }
            );

            // Seed users
            var users = MockData.GetMockUsers();
            if (users.Count < 3)
            {
                throw new InvalidOperationException("MockData.GetMockUsers() must return at least 3 users.");
            }

            var hasher = new PasswordHasher<ApplicationUser>();
            foreach (var user in users)
            {
                user.PasswordHash = hasher.HashPassword(user, "User123!");
            }
            modelBuilder.Entity<ApplicationUser>().HasData(users);

            // Assign roles
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = users[0].Id, RoleId = roleAdminId },
                new IdentityUserRole<Guid> { UserId = users[1].Id, RoleId = roleAttendeeId },
                new IdentityUserRole<Guid> { UserId = users[2].Id, RoleId = roleAttendeeId }
            );

            // Seed rooms
            List<Room> roomList = MockData.GetMockRooms(); // Populate the rooms list
            modelBuilder.Entity<Room>().HasData(roomList);

            // Seed Organizations
            var organizations = MockData.GetMockOrganizations();
            modelBuilder.Entity<Organization>().HasData(organizations);


            // Seed events
            var events = MockData.GetMockEvents(roomList, organizations);
            if (events.Count == 0)
            {
                throw new InvalidOperationException("MockData.GetMockEvents() must return at least 1 event.");
            }
            modelBuilder.Entity<Event>().HasData(events);

            // Seed News Articles
            var newsArticles = MockData.GetMockNewsArticles(); // Populate the news articles list
            modelBuilder.Entity<NewsArticle>().HasData(newsArticles);

        }

        public void ClearExistingRelationships()
        {
            // Access the shared-type entity using the Set method
            var eventAttendees = Set<Dictionary<string, object>>("ApplicationUserEvent");
            eventAttendees.RemoveRange(eventAttendees);
            SaveChanges();
        }

        public void SeedRelationships()
        {
            // Clear existing relationships
            ClearExistingRelationships();
            // Add new relationships
            var event1 = Events.FirstOrDefault(e => e.Name == "Tech Conference 2023");
            var event2 = Events.FirstOrDefault(e => e.Name == "Team Meeting");
            var user1 = eventsAttendees.FirstOrDefault(u => u.UserName == "user1@example.com");
            var user2 = eventsAttendees.FirstOrDefault(u => u.UserName == "user2@example.com");

            if (event1 != null && user1 != null)
            {
                event1.Attendees.Add(user1);
            }

            if (event1 != null && user2 != null)
            {
                event1.Attendees.Add(user2);
            }

            if (event2 != null && user1 != null)
            {
                event2.Attendees.Add(user1);
            }

            if (event2 != null && user2 != null)
            {
                event2.Attendees.Add(user2);
            }

            SaveChanges();
        }
    }
}
