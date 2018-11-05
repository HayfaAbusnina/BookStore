using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Abstract
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get;  }
        void SaveBook(Book book);
        Book DeleteBook(int ISBN);
    }
}
