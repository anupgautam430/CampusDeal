using CampusDeal.DataAccess.Repository;
using CampusDeal.DataAccess.Repository.IRepository;
using CampusDeal.Models;
using CampusDeal.Models.ViewModels;
using CampusDeal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.IO;

namespace CampusDeal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _webHost;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHost)
        {
            _unit = db;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unit.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unit.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()

                }),
                Product = new Product()
            };
            if(id== null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unit.Product.Get(u=>u.Id == id, includeProperties:"ProductImages");
                return View(productVM);
            }
           
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {
           
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unit.Product.Add(productVM.Product);
                }
                else
                {
                    _unit.Product.Update(productVM.Product);
                }

                _unit.Save();

                string wwwRootPath = _webHost.WebRootPath;
                if (files != null)
                {

                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                            productVM.Product.ProductImages = new List<ProductImage>();

                        productVM.Product.ProductImages.Add(productImage);
                    }

                    _unit.Product.Update(productVM.Product);
                    _unit.Save();




                }


                TempData["success"] = "Product created sucessfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unit.Category.GetAll()
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.CategoryId.ToString()

                    });
                return View(productVM);
            }         
        }

        /*  public IActionResult Edit(int? id)
          {
              if (id == null || id == 0)
              {
                  return NotFound();
              }
              Product? ProductFromDb = _unit.Product.Get(u => u.Id == id);

              if (ProductFromDb == null)
              {
                  return NotFound();
              }
              return View(ProductFromDb);
          }
          [HttpPost]
          public IActionResult Edit(Product obj)
          {
              if (obj.Title == obj.Description.ToString())
              {
                  ModelState.AddModelError("Name", "The display order cannot be same.");
              }
              if (ModelState.IsValid)
              {
                  _unit.Product.Update(obj);
                  _unit.Save();
                  TempData["success"] = "Product Edited sucessfully!";
                  return RedirectToAction("Index");
              }
              return View();
          }*/

        /* public IActionResult Delete(int? id)
         {
             if (id == null || id == 0)
             {
                 return NotFound();
             }

             Product? ProductFromDb = _unit.Product.Get(u => u.Id == id);

             if (ProductFromDb == null)
             {
                 return NotFound();
             }
             return View(ProductFromDb);
         }
         [HttpPost, ActionName("Delete")]
         public IActionResult DeletePOST(int? id)
         {
             Product? obj = _unit.Product.Get(u => u.Id == id);
             if (obj == null)
             {
                 return NotFound();
             }
             _unit.Product.Remove(obj);
             _unit.Save();
             TempData["success"] = "Product Deleted sucessfully!";

             return RedirectToAction("Index");
         }*/

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unit.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHost.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unit.ProductImage.Remove(imageToBeDeleted);
                _unit.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unit.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductList});

        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unit.Product.Get(u=>u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { sucess = false, message = "Error while Deleting" });
            }
            //var oldImagePath =
            //               Path.Combine(_webHost.WebRootPath,
            //                productToBeDeleted.ImageUrl.TrimStart('\\'));

            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}

            
            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHost.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(finalPath);
            }
                

            _unit.Product.Remove(productToBeDeleted);
            _unit.Save();

            return Json(new { success = true, message = "Delete Successful"});

        }

        #endregion

    }
}
