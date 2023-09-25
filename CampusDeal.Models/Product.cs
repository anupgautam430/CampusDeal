using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusDeal.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Seller { get; set; }
        [Required]
        [Display (Name ="Phone Number")]
        public string PNO { get; set; }
        [Required]
        [Display(Name ="Price")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Revised Price")]
        public double PriceTotal { get; set;}

        //adding foreign key for the connection between category and product
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]

        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
