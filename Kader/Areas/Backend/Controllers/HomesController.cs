using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.DTOs.ReturnOrders;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Implementations.Order;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.ReturnOrders;
using Kader.Services.Implementations.ShippingPrice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using System.Text.Json;
using System.Security.Claims;
[Area("Backend")]

    public class HomesController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IShippingPriceService _ShippingPrice; private IReturnOrdersService _ReturnOrders;
    private IKeys _keys; private IOrderssService _Orderss;
    private IProductService _Product;
    public List<CatogryDto> Gov_codesDetail { get; set; }
        public HomesController(IShippingPriceService ShippingPriceService, IReturnOrdersService ReturnOrdersService, IOrderssService OrderssService, IProductService ProductService, IHttpContextAccessor httpContextAccessor)
    {
        _keys = new Keys();
        _ShippingPrice = ShippingPriceService; _ReturnOrders = ReturnOrdersService;
        _httpContextAccessor = httpContextAccessor;
        _Product = ProductService; _Orderss = OrderssService;
    }
        public ActionResult Indexs()
        {
        bool InsideShippingRole = HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.InsideShippingRole());
        if (InsideShippingRole)
        {
            return RedirectToAction("InnerunderShipent", "Orders");
        }
        return View();
        }
    public async Task<IActionResult> Index()
    {
        var HomeBackendDto = new HomeBackendDto();
        var httpContext = _httpContextAccessor.HttpContext;

        // Execute all database/API calls in parallel
        var tasks = new List<Task>();

        var productGomlaTask = _Product.ProductCount(httpContext, _keys.CatType_gomla());
        var productQta3Task = _Product.ProductCount(httpContext, _keys.CatType_qta3a());
        var returnStatuses = new List<int> { 1, 3, 4, 10, 2 };
        var returnOrdersTask = _ReturnOrders.GetCountOrdersByStatuses(httpContext, returnStatuses);

        var orderStatuses = new List<int> { 1, 3, 4, 15, _keys.AssignToStore(),14, _keys.AssignedToShipping() };
        var ordersTask = _Orderss.GetCountOrdersByStatuses(httpContext, orderStatuses);

        var checkQuantityGomlaTask = _Product.GetAllProductThatQuantityLess(httpContext, _keys.CatType_gomla(), 10);
        var checkQuantityQta3aTask = _Product.GetAllProductThatQuantityLess(httpContext, _keys.CatType_qta3a(), 10);

        // Wait for all tasks to complete
        await Task.WhenAll(returnOrdersTask, ordersTask, productGomlaTask, productQta3Task, checkQuantityGomlaTask, checkQuantityQta3aTask);

        if (returnOrdersTask.Result.isSuccess)
        {
            var returnCounts = returnOrdersTask.Result.Result;
            HomeBackendDto.NewReturnOrder = returnCounts.GetValueOrDefault(1, 0);
            HomeBackendDto.UnderCheckReturnOrder = returnCounts.GetValueOrDefault(3, 0);
            HomeBackendDto.underShipentReturnOrder = returnCounts.GetValueOrDefault(4, 0);
            HomeBackendDto.completedReturnOrder = returnCounts.GetValueOrDefault(10, 0);
            HomeBackendDto.RejectedReturnOrders = returnCounts.GetValueOrDefault(2, 0);
        }

        // ✅ Assign order counts
        if (ordersTask.Result.isSuccess)
        {
            var orderCounts = ordersTask.Result.Result;
            HomeBackendDto.NewOrder = orderCounts.GetValueOrDefault(1, 0);
            HomeBackendDto.UnderCheck = orderCounts.GetValueOrDefault(3, 0);
            HomeBackendDto.underShipent = orderCounts.GetValueOrDefault(4, 0);
            HomeBackendDto.completedOrders = orderCounts.GetValueOrDefault(15, 0);
            HomeBackendDto.AssignedToStores = orderCounts.GetValueOrDefault(_keys.AssignToStore(), 0);
            HomeBackendDto.AssignedToShipping = orderCounts.GetValueOrDefault(_keys.AssignedToShipping(), 0);
            HomeBackendDto.InnerunderShipent = orderCounts.GetValueOrDefault(14, 0);
            HomeBackendDto.NotcompletedOrders = orderCounts.GetValueOrDefault(16, 0);
        }

        // ✅ Assign product data
        if (productGomlaTask.Result.isSuccess) HomeBackendDto.ProductGomla = productGomlaTask.Result.Result;
        if (productQta3Task.Result.isSuccess) HomeBackendDto.ProductQta3 = productQta3Task.Result.Result;
        if (checkQuantityGomlaTask.Result.isSuccess) HomeBackendDto.HomeProductGomla = checkQuantityGomlaTask.Result.Result;
        if (checkQuantityQta3aTask.Result.isSuccess) HomeBackendDto.HomeProductQta3 = checkQuantityQta3aTask.Result.Result;

        return View(HomeBackendDto);
    }


    public ActionResult UnAuthorized()
        {
            return View();
        }

        
     
        
        
    }

