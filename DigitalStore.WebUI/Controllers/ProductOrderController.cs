using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity;
using DigitalStore.Repos.Interfaces;
using ReflectionIT.Mvc.Paging;
using DigitalStore.BusinessLogic.Interfaces;

namespace DigitalStore.WebUI.Controllers
{
    public class ProductOrderController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductOrderRepo _productOrderRepo;
        private const int categoriesPageSize = 10;
        private readonly IProductOrderLogic _productOrderLogic;

        public ProductOrderController(ICustomerRepo customerRepo, IOrderRepo orderRepo, IProductOrderRepo productOrderRepo,
            IProductOrderLogic productOrderLogic)
        {
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
            _productOrderRepo = productOrderRepo;
            _productOrderLogic = productOrderLogic;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserOrderList(int pageIndex = 1)
        {
            var userId = User.Identity.GetUserId();
            var customers = _customerRepo.GetCustomerIdByUserId(userId);
            //var orders = _productOrderRepo.GetCustomerOrdersList(customers.First().Id);
            var orders = _productOrderLogic.GetOrdersOfCustomers(customers);

            var model = PagingList.Create(orders, categoriesPageSize, pageIndex);
            model.Action = nameof(UserOrderList);
            return View(model);
        }
    }
}
