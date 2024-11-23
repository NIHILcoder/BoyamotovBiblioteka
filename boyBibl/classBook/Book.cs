using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boyBibl.classBook
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public Book(string title, string author, int year)
        {
            Id = Guid.NewGuid();
            Title = title;
            Author = author;
            Year = year;
        }
    }
}