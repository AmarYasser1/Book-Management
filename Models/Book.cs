using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Management.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime PublishingDate { get; set; }
        public int AuthorID { get; set; }
        public Author Author { get; set; } = null!;
    }
}
