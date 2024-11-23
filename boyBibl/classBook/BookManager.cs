using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boyBibl.classBook
{
    public class BookManager
    {
        private List<Book> books = new List<Book>();

        // Добавление книги
        public void AddBook(string title, string author, int year)
        {
            books.Add(new Book(title, author, year));
        }

        // Удаление книги по ID
        public bool RemoveBook(Guid id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                return true;
            }
            return false;
        }

        // Поиск книг по названию
        public List<Book> FindBookByName(string title)
        {
            return books.Where(b => b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Поиск книг по автору
        public List<Book> FindBookByAuthor(string author)
        {
            return books.Where(b => b.Author.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        }

        // Получение списка всех книг
        public List<Book> PrintAllBooks()
        {
            return books;
        }
    }
}