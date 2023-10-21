using CampusDeal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusDeal.DataAccess.Data
{
    public class ApplicationDbContext:  IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options ) : base(options)
        {

        } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        



        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Goods", DisplayOrder = 1 },
                new Category { CategoryId = 2, Name = "Services", DisplayOrder = 2 }
                ) ;

            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1,
                              Name = "Tech", StreetName ="tole 1",
                              City="btm", PostalCode="125470", 
                              State="koshi", PhoneNumber="1425623570" },
                 new Company
                 {
                     Id = 2,
                     Name = "Tech1",
                     StreetName = "tole 11",
                     City = "btm",
                     PostalCode = "125470",
                     State = "koshi",
                     PhoneNumber = "1425623570"
                 },
                  new Company
                  {
                      Id = 3,
                      Name = "Tech2",
                      StreetName = "tole 12",
                      City = "btm",
                      PostalCode = "125470",
                      State = "koshi",
                      PhoneNumber = "1425623570"
                  }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id=1,
                    Title="Book",
                    Description="the maths book",
                    PNO = "A01",
                    Seller="john wick",
                    Price = 400,
                    PriceTotal =400,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Title = "notebook",
                    Description = "the maths notebook",
                    PNO = "A02",
                    Seller = "john snow",
                    Price = 200,
                    PriceTotal = 200,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Title = " Science lab kit",
                    Description = "the kit for science lab",
                    PNO = "A03",
                    Seller = "john cena",
                    Price = 150,
                    PriceTotal = 150,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 4,
                    Title = "Laptop",
                    Description = "laptop in a very good condition. acer aspire 5 250GB ssd, 1 tb HDD, intel5 7gen 4 GB graphics  ",
                    PNO = "A04",
                    Seller = "billie",
                    Price = 40000,
                    PriceTotal = 39000,
                    CategoryId = 2
                }

                );
        }

    }
}
