using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;
using OdinCMS.Models.ViewModels;


namespace OdinCMS.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBased(cart.Count, cart.Product.Price);
                ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }

			return View(ShoppingCartVM);
        }

        private double GetPriceBased(double quantity, double price)
        {
            return price * quantity;
        }
    }
}
