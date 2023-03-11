using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdinCMS.DataAccess.Data;
using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;

namespace OdinCMS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _db;

        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = _db.GetAll();
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

            _db.Create(obj);
            _db.Save();
            TempData["success"] = "Category created successfully";

            return RedirectToAction("Index");

        }

        /* Update */
        public IActionResult Update(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var categoryFromDb = _db.GetFirstOrDefault(u=> u.Id == id);

            if(categoryFromDb == null)
                return NotFound();


            return View(categoryFromDb);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Category obj)
        {
            if (!ModelState.IsValid)
                return View(obj);


            _db.Update(obj);
            _db.Save();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");

        }

        /* Delete */
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var categoryFromDb = _db.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
                return NotFound();


            return View(categoryFromDb);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int?  id)
        {
            var obj = _db.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return NotFound();


            _db.Remove(obj);
            _db.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }

    }
}
