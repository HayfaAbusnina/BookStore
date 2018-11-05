using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.WebUI.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { set;get;  }
        public int CurrentPage { get; set; }
        public int TotalPage
        {
            get {return (int)Math.Ceiling((decimal)TotalItems/ItemsPerPage); }
        }
    }
}