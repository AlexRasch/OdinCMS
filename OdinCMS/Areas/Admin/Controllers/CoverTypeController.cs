using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdinCMS.DataAccess.Data;
using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;

namespace OdinCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);

        }

        /* Create */

        public IActionResult Create()
        {
            return View();

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (!ModelState.IsValid)
                return View(obj);

            _unitOfWork.CoverType.Create(obj);
            _unitOfWork.Save();
            TempData["success"] = "Covertype created successfully";

            return RedirectToAction("Index");

        }

        /* Update */
        public IActionResult Update(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypeFromDb == null)
                return NotFound();


            return View(coverTypeFromDb);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(CoverType obj)
        {
            if (!ModelState.IsValid)
                return View(obj);


            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Covertype updated successfully";
            return RedirectToAction("Index");

        }

        /* Delete */
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var covertypFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (covertypFromDb == null)
                return NotFound();


            return View(covertypFromDb);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return NotFound();


            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Covertype deleted successfully";
            return RedirectToAction("Index");

        }

    }
}
