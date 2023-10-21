using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusDeal.Models.ViewModels
{
    public class ReportVM
    {
        int ID { get; set; }
        public string Seller { get; set; }
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get; set; }
        public List<Product> Productss { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public ApplicationUser ApplicationUser { get; set; }



    }
}
