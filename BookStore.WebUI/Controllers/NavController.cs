using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBookRepository repository;

        //constructor
        public NavController(IBookRepository rep)
        {
            repository=rep;
        }
        public PartialViewResult Menu(string specialization=null)//,bool mobileLayout=false)
        {
            ViewBag.SelectedSpec = specialization;
            IEnumerable<string> spec = repository.Books.Select(b => b.Specialization)
                                                                 .Distinct();
            //string viewName = mobileLayout?"MenuHorizontal": "Menu";
            //return PartialView(viewName, spec);
            return PartialView("FlexibleMenu",spec);
        }
    }
}