using OdinCMS.Data;
using OdinCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OdinCMS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Categories.ToListAsync());
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

            _db.Categories.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully";

            return RedirectToAction("Index");

        }

        /* Update */
        public IActionResult Update(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var categoryFromDb = _db.Categories.Find(id);

            if(categoryFromDb == null)
                return NotFound();


            return View(categoryFromDb);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Category obj)
        {
            if (!ModelState.IsValid)
                return View(obj);


            _db.Categories.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");

        }

        /* Delete */
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
                return NotFound();


            return View(categoryFromDb);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int?  id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
                return NotFound();


            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }

    }
}
