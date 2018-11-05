using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.HtmlHelper
{
    //    Note:
    //    Html static: put in views
    //    Html dynamic: create new folder to put Html dynamic classes in it

    public static class PagingHelper
    {
        //using System.Web.Mvc; // for MvcHtmlString
        //using BookStore.WebUI.Models; // for PagingInfo
        public static MvcHtmlString PageLinks(
            this System.Web.Mvc.HtmlHelper html ,PagingInfo pageInfo,Func<int,string> pageUrl)
        {
            StringBuilder result=new StringBuilder();
            for (int i = 1; i <= pageInfo.TotalPage; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}