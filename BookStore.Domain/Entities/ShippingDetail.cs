using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
   public class ShippingDetail
    {
        [Required(ErrorMessage ="Please enter a name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the first line")]
        [Display(Name="Address Line 1")]
        public string Line1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string Line2 { get; set; }
        [Required(ErrorMessage = "Please enter the city name")]
        public string City { get; set; }
        public string State { get; set; }
        [Required(ErrorMessage = "Please enter a country")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }

    }
}
