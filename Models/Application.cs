using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book_Management.Contexts;
using Book_Management.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Book_Management.Models
{
    public class Application : ICheckAuthors
    {
        private readonly Management _management;

        public Application(BookManagementDbContext context)
        {
            _management = new Management(context);
        }

        public async Task AddBook() 
        {
            Console.Clear();
            Console.WriteLine("Adding new book.");
            Console.WriteLine("*****************");

            Console.Write("Enter the name of the book: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("__You should enter the name.(-_-)");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the title of the book: ");
            string? title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("__You should enter the title.(-_-)");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the description of the book: ");
            string? description = Console.ReadLine();

            Console.Write("Enter the price of the book: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("__Invalid price format. Please enter a valid number.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter author id: ");
            if (!int.TryParse(Console.ReadLine(), out int authorId))
            {
                Console.WriteLine("__Invalid id format");
                Console.ReadKey();
                return;
            }

            if (CheckAuthor(authorId) is null) 
            {
                Console.WriteLine("__Author not found. Please enter a valid author ID.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the date of publishing (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime publishingDate))
            {
                Console.WriteLine("__Invalid date format. Please use yyyy-MM-dd.");
                Console.ReadKey();
                return;
            }

            var newBook = new Book()
            {
                Name = name,
                Title = title,
                Description = description,
                Price = price,
                AuthorID = authorId,
                PublishingDate = publishingDate
            };

            try
            {
                await _management.CreateAsync(newBook);
                Console.WriteLine("* The Book created successfully!");
            }
            catch (Exception ex)
            { 
                Console.WriteLine($">Error occured while Adding book: {ex.Message}");
            }

            Console.WriteLine("\nPress any key on keyboard...");
            Console.ReadKey();
        }
        public async Task DeleteBook() 
        {
            Console.Clear();
            Console.WriteLine("Choose the deleting options: ");
            Console.WriteLine("*****************************");
            Console.WriteLine("1) Delete a book.");
            Console.WriteLine("2) Delete all books.");
            Console.WriteLine("3) Exit.");
            Console.WriteLine("*****************************");
            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    Console.Write("Enter {id} of the book: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("__Invalid id format.");
                        Console.ReadKey();
                        return;
                    }

                    try
                    {
                        await _management.DeleteAsync(id);
                        Console.WriteLine("* The book deleted successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($">Error occured while deleting a book: {ex.Message}");
                    }
                    break;
                case "2":
                    try
                    {
                        await _management.DeleteAllAsync();
                        Console.WriteLine("* All books deleted successfully!");
                    }catch(Exception ex)
                    {
                        Console.WriteLine($">Error occured while deleting all books: {ex.Message}");          
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("__Invalid choice.(-_-)");
                    break;
            }

            Console.WriteLine("\nPress any key on keyboard...");
            Console.ReadKey();
        }
        public async Task UpdateBook() 
        {
            Console.Clear();
            Console.Write("Enter the {id} of the updating book: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("__Invalid id format.");
                Console.ReadKey();
                return;
            }

            var oldBook = await _management.Context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
            if (oldBook is null)
            {
                Console.WriteLine("__Book not found.(-_-)");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Updating book:");
            Console.WriteLine("****************");
            Console.Write("Enter the name of the book: ");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("__You should enter the name.(-_-)");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the title of the book: ");
            string? title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("__You should enter the title.(-_-)");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the description of the book: ");
            string? description = Console.ReadLine();

            Console.Write("Enter the price of the book: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("__Invalid price format. Please enter a valid number.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter author id: ");
            if (!int.TryParse(Console.ReadLine(), out int authorId))
            {
                Console.WriteLine("__Invalid id format");
                Console.ReadKey();
                return;
            }

            if (CheckAuthor(authorId) is null)
            {
                Console.WriteLine("__Author not found. Please enter a valid author ID.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the date of publishing (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime publishingDate))
            {
                Console.WriteLine("__Invalid date format. Please use yyyy-MM-dd.");
                Console.ReadKey();
                return;
            }

            var updatedBook = new Book
            {
                Id = oldBook.Id,
                Title = title,
                Description = description,
                Name = name,
                Price = price,
                AuthorID = authorId,
                PublishingDate = publishingDate
            };

            try
            {
                await _management.UpdateAync(id, updatedBook);
                Console.WriteLine("* Book updated succesfully!");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error occured while updating book: {ex.Message}");
            }

            Console.Clear();
            Console.WriteLine("Old book:");
            Console.WriteLine("***********");
            await PrintBook(oldBook);

            Console.WriteLine("*******************************");

            Console.WriteLine("Updated book:");
            Console.WriteLine("*************");
            await PrintBook(updatedBook);

            Console.WriteLine("\nPress any key on keyboard...");
            Console.ReadKey();
        }
        public async Task SearchBook() 
        {
            Console.Clear();

            Console.WriteLine("Choose the searching options: ");
            Console.WriteLine("*****************************");
            Console.WriteLine("1) Search a book.");
            Console.WriteLine("2) Search all books.");
            Console.WriteLine("3) Exit");
            Console.WriteLine("*****************************");
            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            switch(choice)
            {
                case "1":
                    Console.Write("Enter the {id} of the book: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("__Invalid id format.");
                        Console.ReadKey();
                        return;
                    }

                    try
                    {
                        var book = await _management.GetByIdAsync(id);

                        if (book is null)
                        {
                            Console.WriteLine("__Invalid book id. (-_-)");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine("\nThe Book Details");
                        Console.WriteLine("********************");

                        await PrintBook(book);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($">Error occured while getting a book: {ex.Message}");
                    }
                    break;
                case "2":
                    await SearchAllBooks();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("__Invalid choice.(-_-)");
                break;
            }

            Console.WriteLine("\nPress any key on keyboard...");
            Console.ReadKey();
        }
        private async Task SearchAllBooks()
        {
            Console.Clear();
            Console.WriteLine("Books Details:");
            Console.WriteLine("****************");

            try
            {
                var books = await _management.GetAllAsync();
                foreach (var book in books)
                {
                    await PrintBook(book);
                    Console.WriteLine("\n**************************");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error occured while getting all books: {ex.Message}");
            }
        }
        public Author? CheckAuthor(int id)
        {
            try
            {
               return _management.Context.Authors.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error occured while checking author id: {ex.Message}");
                return null;
            }
        }
        private async Task PrintBook(Book book)
        {
            Console.WriteLine($"* ID: {book.Id}");
            Console.WriteLine($"* Name: {book.Name}");
            Console.WriteLine($"* Title: {book.Title}");
            Console.WriteLine($"* Description: {book.Description}");
            Console.WriteLine($"* Price: {book.Price:C}");
            Console.WriteLine($"* Publication date: {book.PublishingDate:yyyy-MM-dd}");

            var author = await _management.Context.Authors.FirstOrDefaultAsync(a => a.Id == book.AuthorID);
            string authorName = author is null ? "Author not found." : author.Name;

            Console.WriteLine($"* Author: {authorName}");
        }
    }
}
