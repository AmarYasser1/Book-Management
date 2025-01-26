using System.Reflection;
using Book_Management.Contexts;
using Book_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Book_Management
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var context = new BookManagementDbContext();

            await SeedData(context);

            var application = new Application(context);

            while (true)
            {
                Console.Clear();

                TitleApp();

                Console.WriteLine("Choose an option:");
                Console.WriteLine("******************");
                Console.WriteLine("1) Add");
                Console.WriteLine("2) Update");
                Console.WriteLine("3) Delete");
                Console.WriteLine("4) Search");
                Console.WriteLine("5) Exit");
                Console.WriteLine("******************");
                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();

                switch (choice) 
                {
                    case "1":
                        try
                        {
                            await application.AddBook();
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine($"Error occured while adding book main: {ex.Message}");
                        }
                        break;
                    case "2":
                        try
                        {
                            await application.UpdateBook();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error occured while adding book main: {ex.Message}");
                        }
                        break;
                    case "3":
                        try
                        {
                            await application.DeleteBook();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error occured while adding book main: {ex.Message}");
                        }
                        break;
                    case "4":
                        try
                        {
                            await application.SearchBook();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error occured while adding book main: {ex.Message}");
                        }
                        break;
                    case "5":
                        Console.WriteLine("\nExit successfully!");
                        return;
                    default:
                        Console.WriteLine("__Invalid choice. Please enter correct choice");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task SeedData(BookManagementDbContext context)
        {
            if (!await context.Authors.AnyAsync())
            {
                context.Authors.AddRange(
                    new Models.Author { Name = "Amar", Address = "Zagazig" },
                    new Models.Author { Name = "Yasser", Address = "Sharqia" },
                    new Models.Author { Name = "Mohamed", Address = "London" },
                    new Models.Author { Name = "Metwally", Address = "America" },
                    new Models.Author { Name = "Ahmed", Address = "Italia" }
                );

                await context.SaveChangesAsync();
            }
        }

        private static void TitleApp()
        {
            Console.WriteLine("\t\t\t\t\t ================================ ");
            Console.WriteLine("\t\t\t\t\t|                                |");
            Console.WriteLine("\t\t\t\t\t|      BooK Management App       |");
            Console.WriteLine("\t\t\t\t\t|                                |");
            Console.WriteLine("\t\t\t\t\t ================================ ");
        }
    }
}
