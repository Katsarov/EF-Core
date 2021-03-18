namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);


            //Console.WriteLine(GetBooksByAgeRestriction(db, "miNor"));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db, 2000));
            //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));
            //Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, "dy"));
            //Console.WriteLine(GetBookTitlesContaining(db, "sK"));
            //Console.WriteLine(GetBooksByAuthor(db, "po"));
            //Console.WriteLine(CountBooks(db, 40));
            //Console.WriteLine(CountCopiesByAuthor(db));
            //Console.WriteLine(GetTotalProfitByCategory(db));
            //Console.WriteLine(GetMostRecentBooks(db));
            //IncreasePrices(db);  // Това е войд метод и нищо не принтира
            Console.WriteLine(RemoveBooks(db));
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);

            context.SaveChanges();
            return books.Count;
        }


        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }


        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoryBooks = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks.Select(b => new
                    {
                        b.Book.Title,
                        b.Book.ReleaseDate.Value
                    })
                    .OrderByDescending(b => b.Value)
                    .Take(3)
                    .ToList()
                })
                .OrderBy(c => c.CategoryName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var name in categoryBooks)
            {
                sb.AppendLine($"--{name.CategoryName}");

                foreach (var bookname in name.Books)
                {
                    sb.AppendLine($"{bookname.Title} ({bookname.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }


        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Name)
                .ToList();

            var result = string.Join(Environment.NewLine, categories.Select(c => $"{c.Name} ${c.Profit:F2}"));
            return result;
        }


        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var autors = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.TotalCopies)
                .ToList();

            var result = string.Join(Environment.NewLine, autors.Select(a => $"{a.FirstName} {a.LastName} - {a.TotalCopies}"));
            return result;
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck);

            return books.Count();
        }


        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                //.Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Where(a => EF.Functions.Like(a.Author.LastName, $"{input}%"))
                .Select(an => new
                {
                    AutorName = an.Author.FirstName + " " + an.Author.LastName,
                    an.Title,
                    an.BookId
                })
                .OrderBy(b => b.BookId)
                .ToList();

            var result = string.Join(Environment.NewLine, books.Select(a => $"{a.Title} ({a.AutorName})"));
            return result;
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b =>b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;

        }


        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var autors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(an => new
                {
                    an.FirstName,
                    an.LastName
                })
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToList();

            var result = string.Join(Environment.NewLine, autors.Select(a => $"{a.FirstName} {a.LastName}"));
            return result;
        }


        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var targetDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate.Value < targetDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();


            // -----------------------------Variant 1 - with StringBuilder ------------------------------
            //var sb = new StringBuilder();
            //foreach (var book in books)
            //{
            //    sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            //}
            //return sb.ToString().TrimEnd();



            // -----------------------------Variant 2 - string.Join ------------------------------
            var result = string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}"));
            return result;
        }


        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToList();

            // ----------------------- Variant 1 -------------------------------------------------------
            //var books = context.Books
            //    .Where(b => b.BookCategories
            //        .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
            //    .Select(t => t.Title)
            //    .OrderBy(t => t)
            //    .ToList();

            //var result = string.Join(Environment.NewLine, books);
            //return result;


            // ----------------------- Variant 2 -------------------------------------------------------
            var books = context.BooksCategories
                .Where(bc => categories.Contains(bc.Category.Name.ToLower()))
                .Select(b => b.Book.Title)
                .OrderBy(t => t)
                .ToList();
            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(x => x.BookId)
                .ToList();

            var result = string.Join(Environment.NewLine, books.Select(x => x.Title));
            return result;
        }


        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .Select(x => new 
                    {
                        x.Title,
                        x.Price
                    })
                .OrderByDescending(x => x.Price)
                .ToList();

            var sb = new StringBuilder();

            //var result = string.Join(Environment.NewLine, books.Select(x => $"{x.Title} - ${x.Price}"));
            //return result;

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();

        }


        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new 
                {
                    b.BookId,
                    b.Title
                })
                .OrderBy(x => x.BookId)
                .ToList();

            var result = string.Join(Environment.NewLine, books.Select(x => x.Title));

            return result;
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(books => books.AgeRestriction == ageRestriction)
                .Select(books => books.Title)
                .OrderBy(title => title)
                .ToArray();

            var result = string.Join(Environment.NewLine, books);

            return result;
        }
    }
}
