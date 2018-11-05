using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
    public class Book
    {
        [Key]
        //[HiddenInput(DisplayValue =false)]
        public int ISBN { get; set; }

        [Required(ErrorMessage ="Please, enter a book title")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please, enter a book description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please, enter a book price")]
        [Range(0.1,9999.99,ErrorMessage = "Please, enter a positive vallid price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please, enter a book specialization")]
        public string Specialization { get; set; }
    }
}
