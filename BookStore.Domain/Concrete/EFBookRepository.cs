using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Domain.Entities;

namespace BookStore.Domain.Concrete
{
    //using BookStore.Domain.Abstract;
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get
            {
                return context.Books;
            }
        }

        public void SaveBook(Book book)
        {
            Book dbEntity = context.Books.Find(book.ISBN);
            if (dbEntity == null)
                context.Books.Add(book);
            else
            {
                dbEntity.Title = book.Title;
                dbEntity.Specialization = book.Specialization;
                dbEntity.Price = book.Price;
                dbEntity.Description = book.Description;
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int ISBN)
        {
            Book dbEntity = context.Books.Find(ISBN);
            if (dbEntity != null)
            {
                context.Books.Remove(dbEntity);
                context.SaveChanges();
            }
            return dbEntity;
        }
    }
}
