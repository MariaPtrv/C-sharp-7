using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Book
    {
        public Book() => Authors = new HashSet<Author>();
        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public int Year { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
}
