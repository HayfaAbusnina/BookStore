using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IBookRepository repository;
        public AdminController(IBookRepository bookRep)
        {
            repository = bookRep;
        }
        public ViewResult Index()
        {
            return View(repository.Books);
        }
        public ViewResult Edit(int ISBN)
        {
            Book book = repository.Books.FirstOrDefault(b=>b.ISBN==ISBN);
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if(ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = book.Title + " has been saved."; //TempData=>   Redirect ينقل البيانات من صفحة الي صفحة وينتهي الي حين تخلص
                return RedirectToAction("Index");
            }
            else
            {
                // not complete
                return View(book);
            }
        }

        public ViewResult Create()
        {
            return View("Edit",new Book());
        }

        [HttpGet]
        public ActionResult Delete(int ISBN)
        {
            Book deletedBook = repository.DeleteBook(ISBN);
            if(deletedBook!=null)
            {
                TempData["message"] = deletedBook.Title + " was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}