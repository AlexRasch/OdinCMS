using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdinCMS.DataAccess.Data;
using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;

namespace OdinCMS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);

        }

        /* Create */

        public IActionResult Create()
        {
            return View();

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (!ModelState.IsValid)
                return View(obj);

            _unitOfWork.Category.Create(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";

            return RedirectToAction("Index");

        }

        /* Update */
        public IActionResult Update(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u=> u.Id == id);

            if(categoryFromDb == null)
                return NotFound();


            return View(categoryFromDb);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Category obj)
        {
            if (!ModelState.IsValid)
                return View(obj);


            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");

        }

        /* Delete */
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
                return NotFound();


            return View(categoryFromDb);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int?  id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return NotFound();


            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }

    }
}
