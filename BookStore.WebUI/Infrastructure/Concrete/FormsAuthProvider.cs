using BookStore.WebUI.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BookStore.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        //using System.Web.Security;
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username,password);
            if (result)
                FormsAuthentication.SetAuthCookie(username,false);
            return result;
        }
    }
}