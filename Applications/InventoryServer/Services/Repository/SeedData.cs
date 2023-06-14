namespace InventoryServer.Services.Repository
{
    using InventoryServer.Models;
    using System;
    using System.Collections.Generic;

    public static class SeedData
    {
        public static Inventory InitializeData()
        {
            var books = GenerateBooks();
            var bookCollections = GenerateBookCollections(books);

            return new Inventory
            {
                Books = books,
                BookCollections = bookCollections
            };
        }

        private static List<Book> GenerateBooks()
        {
            return new List<Book>
        {
            new Book
            {
                Id = "1",
                Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                Author = "Robert C. Martin",
                Description = "Clean Code is a book about the craft of software development and writing clean code.",
                Price = 29.99,
                Quantity = 10,
                BookCollectionId = "1"
            },
            new Book
            {
                Id = "2",
                Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                Description = "Design Patterns is a classic book on software design patterns that provides reusable solutions to common problems in software design.",
                Price = 39.99,
                Quantity = 5,
                BookCollectionId = "2"
            },
            new Book
            {
                Id = "3",
                Title = "The Pragmatic Programmer: Your Journey to Mastery",
                Author = "Andrew Hunt, David Thomas",
                Description = "The Pragmatic Programmer is a practical guide that provides tips, techniques, and practices for becoming a better programmer.",
                Price = 24.99,
                Quantity = 8,
                BookCollectionId = "1"
            }
        };
        }

        private static List<BookCollection> GenerateBookCollections(List<Book> books)
        {
            var bookCollections = new List<BookCollection>();

            if (books.Count >= 2)
            {
                bookCollections.Add(new BookCollection
                {
                    Id = "1",
                    Name = "Best Sellers"
                });
            }

            if (books.Count >= 3)
            {
                bookCollections.Add(new BookCollection
                {
                    Id = "2",
                    Name = "New Releases"
                });
            }

            // Add more book collections as needed

            return bookCollections;
        }
    }


}
