using BookStore.Domain.Abstract; // for IBookRepository
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 4;
        public BookController(IBookRepository bookRep)
        {
            repository = bookRep;
        }

        public ViewResult List(string specialization,int pageno=1)
        {
            BookListViewModel model = new BookListViewModel {
                Books= repository.Books
                .Where(b=>specialization==null || b.Specialization==specialization)
                .OrderBy(b => b.ISBN)
                .Skip((pageno - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage=pageno,
                    ItemsPerPage=pageSize,
                    TotalItems=specialization==null?
                               repository.Books.Count():
                               repository.Books.Where(b=>b.Specialization==specialization).Count()
                },
                CurrentSpecialization=specialization
            };


            return View(model);
        }

        public ViewResult ListAll()
        {
            return View(repository.Books);
        }
    }
}