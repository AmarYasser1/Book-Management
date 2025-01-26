using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book_Management.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Book_Management.Models
{
    public class Management
    {
        public readonly BookManagementDbContext Context;
        public Management(BookManagementDbContext context)
        {
            Context = context;
        }

        public async Task CreateAsync(Book? book) 
        {
            if (book is null)  throw new ArgumentNullException(nameof(book));

            Context.Add(book);

           await Context.SaveChangesAsync();
        }
        public async Task UpdateAync(int id, Book updatedBook)
        {
            var book = await Context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) throw new InvalidOperationException("Book not found");

            book.Name = updatedBook.Name;
            book.Title = updatedBook.Title;
            book.AuthorID = updatedBook.AuthorID;
            book.PublishingDate = updatedBook.PublishingDate;
            book.Price = updatedBook.Price;
            book.Description = updatedBook.Description;

            await Context.SaveChangesAsync();
        } 
  
        public async Task DeleteAsync(int id)
        { 
             var book = await Context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null) throw new InvalidOperationException("Book not found.");

            Context.Books.Remove(book);

            await Context.SaveChangesAsync();
        }
        public async Task DeleteAllAsync() 
        {
           var books = await Context.Books.ToListAsync();

            Context.Books.RemoveRange(books);

            await Context.SaveChangesAsync();
        }
        public async Task<Book?> GetByIdAsync(int id) 
        {
            var book = await Context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null) throw new InvalidOperationException("Book not found.");

            return book;
        }
        public async Task<ICollection<Book>> GetAllAsync()
        {
            var books = await Context.Books.ToListAsync();

            return books;
        }
    }
}
