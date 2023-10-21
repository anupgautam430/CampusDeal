using CampusDeal.DataAccess.Data;
using CampusDeal.DataAccess.Repository.IRepository;
using CampusDeal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusDeal.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>,IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(Product ob)
        {
            var objFromDb = _db.products.FirstOrDefault(u=> u.Id == ob.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = ob.Title;
                objFromDb.Description = ob.Description;
                objFromDb.CategoryId = ob.CategoryId;
                objFromDb.Seller=ob.Seller;
                objFromDb.Price = ob.Price;
                objFromDb.PNO = ob.PNO;
                objFromDb.PriceTotal= ob.PriceTotal;
                objFromDb.ProductImages= ob.ProductImages;
                //if(ob.ImageUrl!=null)
                //{
                //    objFromDb.ImageUrl = ob.ImageUrl;
                //}

            }
        }
        
    }
}
