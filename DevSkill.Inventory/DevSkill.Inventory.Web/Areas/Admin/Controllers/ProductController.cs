using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using DevSkill.Inventory.Infrastructure;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManagementServices _productManagementServices;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductManagementServices productManagementServices)
        {
            _productManagementServices = productManagementServices;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GerProductJsonData([FromBody] ProductListModel model)
        {
            var result = _productManagementServices.GetProducts(model.PageIndex, model.PageSize, model.Search, 
                model.FormatSortExpression("ProductName", "Id"));

            var productJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.ProductName),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };

            return Json(productJsonData);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product { Id = Guid.NewGuid() , ProductName = model.ProductName };

                try
                {
                    _productManagementServices.CreateProduct(product);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Inventory Item created successfuly",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Inventory Item creation failed",
                        Type = ResponseTypes.Success
                    });
                    _logger.LogError(ex, "Inventory Item creation failed");
                }
                
            }
            return View();
        }
    }
}
