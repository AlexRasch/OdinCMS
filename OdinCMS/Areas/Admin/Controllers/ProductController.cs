using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdinCMS.DataAccess.Data;
using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;

namespace OdinCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
            return View(objProductList);

        }

        /* Create */

        public IActionResult Create()
        {
            return View();

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            if (!ModelState.IsValid)
                return View(obj);

            _unitOfWork.Product.Create(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";

            return RedirectToAction("Index");

        }

        /* Update */
        public IActionResult Update(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (productFromDb == null)
                return NotFound();


            return View(productFromDb);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Product obj)
        {
            if (!ModelState.IsValid)
                return View(obj);


            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product updated successfully";
            return RedirectToAction("Index");

        }

        /* Delete */
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
                return NotFound();

            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (productFromDb == null)
                return NotFound();


            return View(productFromDb);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
                return NotFound();


            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");

        }

    }
}
