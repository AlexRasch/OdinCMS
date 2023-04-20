using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;
using OdinCMS.Models.ViewModels;
using OdinCMS.Utility;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace OdinCMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult UpdateOrderDetail()
		{
			var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.Region = OrderVM.OrderHeader.Region;
            orderHeaderFromDb.PostalCode = orderHeaderFromDb.PostalCode;

            if(OrderVM.OrderHeader.Carrier != null)
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;

            if(OrderVM.OrderHeader.TrackingNumber != null)
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;

            // Save 
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order details updated successfully";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
		}

		[HttpPost, ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult StartProcessing()
		{
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.Order_Processing);
			_unitOfWork.Save();

			TempData["Success"] = "Order status updated successfully";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost, ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult ShipOrder()
		{
			var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeaderFromDb.OrderStatus = SD.Order_Shipped;
            orderHeaderFromDb.ShippingDate = DateTime.Now;

            if (orderHeaderFromDb.PaymentStatus == SD.Payment_ApprovedForDelayedPayment)
                orderHeaderFromDb.PaymentDueDate = DateTime.Now.AddDays(30);

            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_unitOfWork.Save();

			TempData["Success"] = "Order shipped successfully";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		[HttpPost, ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
		public IActionResult CancelOrder()
		{
			var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);

            if(orderHeaderFromDb.PaymentStatus == SD.Payment_Approved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderFromDb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, SD.Order_Cancelled, SD.Order_Refunded);
				TempData["Success"] = "Order refunded successfully";
			}
			else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, SD.Order_Cancelled, SD.Order_Cancelled);
				TempData["Success"] = "Order cancelled successfully";
			}
            _unitOfWork.Save();

			
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}

		public IActionResult Details(int orderId) 
        {
            
			OrderVM = new OrderVM()
			{
				OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
				OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),
			};
			return View(OrderVM);
        }

        [ActionName("Details")]
        [HttpPost, ValidateAntiForgeryToken]
		public IActionResult DetailsPayNow()
		{

            OrderVM.OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: "ApplicationUser");
            OrderVM.OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == OrderVM.OrderHeader.Id, includeProperties: "Product");
			// Stripe settings
			var domain = @"https://localhost:44344/";
			var options = new SessionCreateOptions
			{
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
				SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
				CancelUrl = domain + $"customer/order/details?orderId={OrderVM.OrderHeader.Id}",
			};

			foreach (var item in OrderVM.OrderDetail)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100), // Stripe price is in cents, so price * 100
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Name,
						},
					},
					Quantity = item.Count,
				};
				options.LineItems.Add(sessionLineItem);
			}

			var service = new SessionService();
			Session session = service.Create(options);
			_unitOfWork.OrderHeader.UpdateStripePaymentId(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
			_unitOfWork.Save();

			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303);
		}

		public IActionResult PaymentConfirmation(int orderHeaderId)
		{
			OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == orderHeaderId);
			if (orderHeader.PaymentStatus == SD.Payment_ApprovedForDelayedPayment)
			{
				// Check stripe status
				var service = new SessionService();
				Session session = service.Get(orderHeader.SessionId);
				if (session.PaymentStatus.ToLower() == "paid")
				{
					_unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, SD.Payment_Approved);
					_unitOfWork.Save();
				}
			}
			return View(orderHeaderId);
		}

		#region API 
		[HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            
            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(
                    u => u.ApplicationUserId == claim.Value,
                    includeProperties: "ApplicationUser"
                    );

            }

            switch (status)
            {
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.Order_Approved);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.Order_Shipped);
                    break;
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.Payment_ApprovedForDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.Order_Pending);
                    break;

                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}
