using CampusDeal.DataAccess.Repository.IRepository;
using CampusDeal.Models;
using CampusDeal.Models.ViewModels;
using CampusDeal.Utility;
using Microsoft.AspNetCore.Authorization;
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
                productVM.Product = _unit.Product.Get(u=>u.Id == id);
                return View(productVM);
            }
           
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHost.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                     
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productVM.Product.Id == 0)
                {
                    _unit.Product.Add(productVM.Product);
                }
                else
                {
                    _unit.Product.Update(productVM.Product);
                }

                _unit.Save();
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
            var oldImagePath =
                            Path.Combine(_webHost.WebRootPath,
                            productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unit.Product.Remove(productToBeDeleted);
            _unit.Save();

            return Json(new { success = true, message = "Delete Successful"});

        }

        #endregion

    }
}
