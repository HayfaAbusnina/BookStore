using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IBookRepository rep, IOrderProcessor proc)
        {
            repository = rep;
            orderProcessor = proc;
        }

        //public ActionResult Index(string returnUrl)
        //{
        //    return View(new CartIndexViewModel
        //    {
        //        Cart = GetCart(),
        //        ReturnCart = returnUrl
        //    });
        //}

        public ViewResult Index(Cart cart,string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        //public RedirectToRouteResult AddToCart(int isbn, string returnUrl)
        public RedirectToRouteResult AddToCart(Cart cart, int isbn, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b=>b.ISBN==isbn);
            if(book!=null)
            {
                //GetCart().AddItem(book);
                cart.AddItem(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        //public RedirectToRouteResult RemoveFromCart(int isbn, string returnUrl)
        public RedirectToRouteResult RemoveFromCart(Cart cart, int isbn, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                //GetCart().RemoveLine(book);
                cart.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetail());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetail shippingDetail)
        {
            if (cart.lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty");
            if(ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetail);
                cart.Clear();
                return View("Completed");
            }
            else
                return View(shippingDetail);
        }

        //private Cart GetCart()
        //{
        //    Cart cart =(Cart) Session["Cart"];
        //    if(cart==null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

    }
}