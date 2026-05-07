using Day3WebApi.DTOs;
using Day3WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Day3WebApi.Services
{
    public interface IBooksService
    {
        public List<Book> GetAll();
        public Book? GetById(int id);
        public List<Book> Search(string? genre, bool? available);
        public object GetStats();
        public void Create(Book book);
        public void FullUpdate(int id, Book updated);
        public void PartialUpdate(int id, [FromBody] BookPatchDto patch);
        public void Delete(int id);
    }
}
