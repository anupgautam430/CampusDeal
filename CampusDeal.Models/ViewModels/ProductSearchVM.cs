using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusDeal.Models.ViewModels
{
    public class ProductSearchVM
    {
        public string SearchQuery { get; set; }
        public List<Product> SearchResults { get; set; }
    }
}
