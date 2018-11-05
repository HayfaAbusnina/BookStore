using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Infrastructure.Binders
{
    // using System.Web.Mvc; //for  IModelBinder
    public class CartModelBinder : IModelBinder //class for building sessions
    {
        private const string sessionKey = "Cart";

        //controllerContext => CartController
        public object BindModel(ControllerContext controllerContext, 
                                ModelBindingContext bindingContext)
        {
            //get cart form session
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
                cart =(Cart) controllerContext.HttpContext.Session[sessionKey];
            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                    controllerContext.HttpContext.Session[sessionKey]= cart;
            }

            return cart;
        }

    }
}