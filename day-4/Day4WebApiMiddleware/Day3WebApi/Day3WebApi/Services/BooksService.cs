using Day3WebApi.DTOs;
using Day3WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Day3WebApi.Services
{
    public class BooksService : IBooksService
    {
        private List<Book> _books = new()
        {
            new Book { Id = 1, Title = "Clean Code", Authors = [new Author
        {
            FirstName = "Angelo",
            LastName = "Virtucio"
        }], Genre = "Programming", Year = 2008, Available = true  },
            new Book { Id = 2, Title = "C# in Depth",     Genre = "Programming", Year = 2019, Available = false },
            new Book { Id = 3, Title = "Dune", Genre = "Fiction",      Year = 1965, Available = true  },
            new Book { Id = 4, Title = "The Pragmatic Programmer",  Genre = "Programming", Year = 1999, Available = false },
            new Book { Id = 5, Title = "Sapiens",  Genre = "History",     Year = 2011, Available = true  },
        };

        public List<Book> GetAll()
        {
            return _books;
        }

        public Book? GetById(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            return book;
        }

        public List<Book> Search(string? genre,bool? available)
        {
            var result = _books.AsEnumerable();
            if (!string.IsNullOrEmpty(genre))
                result = result.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
            if (available.HasValue)
                result = result.Where(b => b.Available == available.Value);
            return result.ToList();
        }

        public object GetStats()
        {
            var stats = new
            {
                Total = _books.Count,
                Available = _books.Count(b => b.Available),
                Borrowed = _books.Count(b => !b.Available),
                ByGenre = _books
                    .GroupBy(b => b.Genre)
                    .Select(g => new { Genre = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .ToList()
            };
            return stats;
        }

        public void Create(Book book)
        {
            book.Id = _books.Count == 0 ? 1 : _books.Max(b => b.Id) + 1;
            _books.Add(book);
        }

        public void FullUpdate(int id, Book updated)
        {
            var existing = _books.FirstOrDefault(b => b.Id == id);
            if (existing == null) return;
            existing.Title = updated.Title;
            existing.Genre = updated.Genre;
            existing.Year = updated.Year;
            existing.Available = updated.Available;
        }

        public void PartialUpdate(int id, [FromBody] BookPatchDto patch)
        {
            var existing = _books.FirstOrDefault(b => b.Id == id);
            if (existing == null) return;
            if (patch.Title != null) existing.Title = patch.Title;
            if (patch.Available != null) existing.Available = patch.Available.Value;
        }

        public void Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return;
            _books.Remove(book);
        }
    }
}
