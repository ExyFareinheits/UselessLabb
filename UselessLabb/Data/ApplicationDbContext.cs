using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UselessLabb.Models;

namespace UselessLabb.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            // Seed data для Genres
            builder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Фантастика", Description = "Наукова фантастика та фентезі" },
                new Genre { Id = 2, Name = "Детектив", Description = "Детективні та кримінальні історії" },
                new Genre { Id = 3, Name = "Роман", Description = "Романтичні історії" },
                new Genre { Id = 4, Name = "Наукова література", Description = "Наукові та освітні книги" },
                new Genre { Id = 5, Name = "Біографія", Description = "Життєві історії реальних людей" }
            );

            // Seed data для Publishers
            builder.Entity<Publisher>().HasData(
                new Publisher 
                { 
                    Id = 1, 
                    Name = "Видавництво А-БА-БА-ГА-ЛА-МА-ГА", 
                    Address = "Київ, Україна",
                    Email = "info@ababahalamaha.com",
                    Website = "https://www.ababahalamaha.com",
                    FoundedDate = new DateTime(1992, 1, 1)
                },
                new Publisher 
                { 
                    Id = 2, 
                    Name = "Видавництво Старого Лева", 
                    Address = "Львів, Україна",
                    Email = "info@starylev.com.ua",
                    Website = "https://starylev.com.ua",
                    FoundedDate = new DateTime(2001, 1, 1)
                },
                new Publisher 
                { 
                    Id = 3, 
                    Name = "Наш Формат", 
                    Address = "Київ, Україна",
                    Email = "info@nashformat.ua",
                    Website = "https://nashformat.ua",
                    FoundedDate = new DateTime(2004, 1, 1)
                }
            );
        }
    }
}
