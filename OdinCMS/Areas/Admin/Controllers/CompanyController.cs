using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OdinCMS.DataAccess.Data;
using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;
using OdinCMS.Models.ViewModels;
using OdinCMS.Utility;
using System.Collections.Generic;

namespace OdinCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }

        /* Update */
        public IActionResult Upsert(int? id)
        {
            Company company = new();

            // Create
            if (id == 0 || id == null)
            {
                return View(company);
            }
            // Update
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(i=> i.Id == id);
                return View(company);
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (!ModelState.IsValid)
                return View(obj);

            // Add Company
            if (obj.Id == 0)
            {
                _unitOfWork.Company.Create(obj);
                TempData["success"] = "Company created successfully";
            }
            // Update existing
            else
            {
                _unitOfWork.Company.Update(obj);
                TempData["success"] = "Company updated successfully";
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");

        }

        #region API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList } );
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return Json(new { success = false, message = "Error: Product not found" });

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Successfully: deleted company" });
        }
        #endregion
    }
}
