using EventManger.Core.Domain.Entites;
using EventManger.Core.Enums;
using Microsoft.AspNetCore.Identity;
using EventManger.Core.Domain.Identity;
using EventManger.Core.Domain.Entities;

namespace EventManger.Infrastructure
{
    public class MockData
    {
        public static List<ApplicationUser> GetMockUsers()
        {
            return new List<ApplicationUser>
            {
                // Admin User
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin@example.com",
                    PersonName = "admin",
                    Email = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed=true
                },

                // Registered Users
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "user1@example.com",
                    PersonName = "user1",
                    Email = "user1@example.com",
                    NormalizedUserName = "USER1@EXAMPLE.COM",
                    NormalizedEmail = "USER1@EXAMPLE.COM",
                    EmailConfirmed=true
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "user2@example.com",
                    PersonName = "user2",
                    Email = "user2@example.com",
                    NormalizedUserName = "USER2@EXAMPLE.COM",
                    NormalizedEmail = "USER2@EXAMPLE.COM",
                    EmailConfirmed=true
                }
            };
        }

        public static List<Room> GetMockRooms()
        {
            return new List<Room>
            {
                new Room
                {
                    RoomID = Guid.NewGuid(),
                    Name = "Conference Room A",
                    Description = "A large conference room with 50 seats.",
                    Seats = 50
                },
                new Room
                {
                    RoomID = Guid.NewGuid(),
                    Name = "Meeting Room B",
                    Description = "A small meeting room with 10 seats.",
                    Seats = 10
                }
            };
        }

        public static List<Event> GetMockEvents(List<Room> rooms, List<Organization> organization)
        {
            return new List<Event>
            {
                new Event
                {
                    EventID = Guid.NewGuid(),
                    Name = "Tech Conference 2023",
                    Description = "Annual technology conference.",
                    StartTime = DateTime.Now.AddDays(7),
                    EndTime = DateTime.Now.AddDays(8),
                    RoomID = rooms[0].RoomID,
                    OrganizationID = organization[0].OrganizationID,
                    Status = EventStatus.Scheduled,
                    PhotoUrl = "\\events-photos\\58147433-411f-47bc-88bb-c70b7ff2e68d.jpeg",
                    Views= 0
                },
                new Event
                {
                    EventID = Guid.NewGuid(),
                    Name = "Team Meeting",
                    Description = "Monthly team meeting.",
                    StartTime = DateTime.Now.AddDays(1),
                    EndTime = DateTime.Now.AddDays(1).AddHours(2),
                    RoomID = rooms[1].RoomID,
                    OrganizationID = organization[1].OrganizationID,
                    Status = EventStatus.Scheduled,
                    PhotoUrl= "\\events-photos\\4f289cfc-708c-4e78-b721-240be036d302.jpeg",
                    Views= 0
                },
                new Event
                {
                    EventID = Guid.NewGuid(),
                    Name = "Problem Solving Competition",
                    Description = "A competition to solve real-world problems.",
                    StartTime = DateTime.Now.AddDays(14),
                    EndTime = DateTime.Now.AddDays(14).AddHours(3),
                    RoomID = rooms[0].RoomID,
                    OrganizationID = organization[2].OrganizationID,
                    Status = EventStatus.Scheduled,
                    PhotoUrl = "\\events-photos\\69f744c9-6ce8-4802-bf62-83b92e22babc.jpeg",
                    Views= 0
                }
            };
        }

        // New method to generate mock organizations
        public static List<Organization> GetMockOrganizations()
        {
            return new List<Organization>
            {
                new Organization
                {
                    OrganizationID = Guid.NewGuid(),
                    Name = "Tech Innovators",
                    Description = "A leading technology innovation company.",
                    College = "Technology",
                    Email = "contact@techinnovators.com",
                    LogoUrl = "\\logos\\3a75a9ba-1079-4854-92c9-a17f064e54af.jpg"
                },
                new Organization
                {
                    OrganizationID = Guid.NewGuid(),
                    Name = "Health Solutions",
                    Description = "Providing healthcare solutions worldwide.",
                    College = "Medicine",
                    Email = "info@healthsolutions.com",
                    LogoUrl = "\\logos\\0ce4ea06-ddf0-4b94-b3f1-e37b5e63ae3a.jpg"
                },
                new Organization
                {
                    OrganizationID = Guid.NewGuid(),
                    Name = "IEEE",
                    Description = "Institute of Electrical and Electronics Engineers.",
                    College = "Engineering",
                    Email = "IEEE@gmail.com",
                    LogoUrl = "\\logos\\4b210b7b-3a14-43fd-af40-752431129fb0.png"
                }
            };
        }
        public static List<NewsArticle> GetMockNewsArticles()
        {
            return new List<NewsArticle>
    {
        new NewsArticle
        {
            Id = Guid.NewGuid(),
            Title = "Tech Conference 2023 Announced",
            Content = "<p><strong>Exciting news!</strong> The annual Tech Conference will be held on " +
                      DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy") + ".</p>" +
                      "<p>This year's conference will feature <em>cutting-edge</em> presentations " +
                      "on AI, blockchain, and quantum computing.</p>",
            CreatedDate = DateTime.Now.AddDays(-5),
            UpdatedDate = DateTime.Now.AddDays(-1),
            PhotoUrl = "\\news-photos\\74pCXBPyfCvb7HWEdm57HE.jpg"
        },
        new NewsArticle
        {
            Id = Guid.NewGuid(),
            Title = "New Research Partnership Formed",
            Content = "<p>The university has partnered with <strong>Tech Innovators</strong> to " +
                      "establish a new research center.</p>" +
                      "<p><em>\"This collaboration will accelerate innovation\"</em>, said the " +
                      "dean of the Technology college.</p>",
            CreatedDate = DateTime.Now.AddDays(-10),
            UpdatedDate = null,
            PhotoUrl = "\\news-photos\\How-to-Build-Strategic-Partnerships-that-Boost-Sales-1.jpg"
        },
        new NewsArticle
        {
            Id = Guid.NewGuid(),
            Title = "Campus Renovation Plans Unveiled",
            Content = "<p>The administration has announced plans for a <strong>$20 million</strong> " +
                      "renovation of the main campus buildings.</p>" +
                      "<p>Construction is expected to begin in " +
                      DateTime.Now.AddMonths(2).ToString("MMMM") + ".</p>",
            CreatedDate = DateTime.Now.AddDays(-3),
            UpdatedDate = DateTime.Now.AddDays(-2),
            PhotoUrl = "\\news-photos\\BGU-NORTH-CAMPUS-02.jpg"
        },
        new NewsArticle
        {
            Id = Guid.NewGuid(),
            Title = "Student Team Wins National Competition",
            Content = "<p><em>Congratulations</em> to our computer science students for winning " +
                      "first place in the national coding competition!</p>" +
                      "<p>The team developed an innovative <strong>AI-powered</strong> " +
                      "healthcare solution.</p>",
            CreatedDate = DateTime.Now.AddDays(-1),
            UpdatedDate = null,
            PhotoUrl = "\\news-photos\\1716833964113.jpg"
        }
    };
        }
    }
}