using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication.Models
{
    public class AuthorsDBContext:DbContext
    {
        public AuthorsDBContext():this (false) { }
         public AuthorsDBContext (bool bFromScratch) : base()
        {
            if (bFromScratch)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        public AuthorsDBContext(DbContextOptions<AuthorsDBContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AuthorsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Author>().Property(b => b.LastName).IsRequired();
           
            modelBuilder.Entity<Author>().HasKey(b => b.Id);

            modelBuilder.Entity<Book>().Property(p => p.Title).IsRequired();
            modelBuilder.Entity<Book>().HasKey(p => p.Id);

            modelBuilder.Entity<Author>().HasOne<Book>(b => b.Book)
            .WithMany(a => a.Authors).HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Author>().HasData(
            new Author[]
            {
                new Author
                {
                    Id = 1,
                    Name = "Лев",
                    LastName = "Толстой",
                    Year = 1865,
                    BookId = 1,
                },
            });
            modelBuilder.Entity<Book>().HasData(
           new Book[]
           {
                new Book
                {
                    Id = 1,
                    Title = "Романы",
                    Pages = 1285,
                    Year = 1895
                },
          
           
                new Book
                {
                    Id = 2,
                    Title = "Сказки",
                    Pages = 1285,
                    Year = 1932
                },


                new Book
                {
                    Id = 3,
                    Title = "Сборник стихов",
                    Pages = 657,
                    Year = 1845
                },
                new Book
                {
                    Id = 4,
                    Title = "Повести",
                    Pages = 343,
                    Year = 1954
                },
});
        }

    }
}
