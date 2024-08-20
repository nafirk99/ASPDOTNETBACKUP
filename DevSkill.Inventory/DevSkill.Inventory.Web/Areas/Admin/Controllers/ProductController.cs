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
            var model = new ProductCreateModel();
            return View(model);
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
        public IActionResult Update(Guid Id)
        {
            var model = new ProductUpdateModel();
            Product product = _productManagementServices.GetProduct(Id);
            model.ProductName = product.ProductName;
            model.Id = product.Id;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(ProductUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product { Id = model.Id, ProductName = model.ProductName };

                try
                {
                    _productManagementServices.UpdateProduct(product);
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Inventory Item Updated successfuly",
                        Type = ResponseTypes.Success
                    });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Inventory Item Update failed",
                        Type = ResponseTypes.Success
                    });
                    _logger.LogError(ex, "Inventory Item Update failed");
                }

            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Guid Id)
        {
            try
            {
                _productManagementServices.DeleteProduct(Id);
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Inventory Item Deleted successfuly",
                    Type = ResponseTypes.Success
                });
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Inventory Item Delete failed",
                    Type = ResponseTypes.Success
                });
                _logger.LogError(ex, "Inventory Item Update failed");
            }
            return View();
        }
    }
}
