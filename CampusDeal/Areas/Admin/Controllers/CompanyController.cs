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

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unit;
        public CompanyController(IUnitOfWork db)
        {
            _unit = db;
            
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unit.Company.GetAll().ToList();
            return View(objCompanyList);
        }
        public IActionResult Upsert(int? id)
        {
           
            if(id== null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyobj = _unit.Company.Get(u=>u.Id == id);
                return View(companyobj);
            }
           
        }
        [HttpPost]
        public IActionResult Upsert(Company companyobj)
        {
           
            if (ModelState.IsValid)
            {
                
                if(companyobj.Id == 0)
                {
                    _unit.Company.Add(companyobj);
                }
                else
                {
                    _unit.Company.Update(companyobj);
                }

                _unit.Save();
                TempData["success"] = "Company created sucessfully!";
                return RedirectToAction("Index");
            }
            else
            {
               
                return View(companyobj);
            }         
        }

      /*  public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company? CompanyFromDb = _unit.Company.Get(u => u.Id == id);

            if (CompanyFromDb == null)
            {
                return NotFound();
            }
            return View(CompanyFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            if (obj.Title == obj.Description.ToString())
            {
                ModelState.AddModelError("Name", "The display order cannot be same.");
            }
            if (ModelState.IsValid)
            {
                _unit.Company.Update(obj);
                _unit.Save();
                TempData["success"] = "Company Edited sucessfully!";
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

            Company? CompanyFromDb = _unit.Company.Get(u => u.Id == id);

            if (CompanyFromDb == null)
            {
                return NotFound();
            }
            return View(CompanyFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company? obj = _unit.Company.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unit.Company.Remove(obj);
            _unit.Save();
            TempData["success"] = "Company Deleted sucessfully!";

            return RedirectToAction("Index");
        }*/

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unit.Company.GetAll().ToList();
            return Json(new {data = objCompanyList});

        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unit.Company.Get(u=>u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { sucess = false, message = "Error while Deleting" });
            }

            _unit.Company.Remove(CompanyToBeDeleted);
            _unit.Save();

            return Json(new { success = true, message = "Delete Successful"});

        }

        #endregion

    }
}
