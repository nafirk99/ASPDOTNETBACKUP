using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public class ProductManagementServices : IProductManagementServices
    {
        private readonly IProductUnitOfWork _productUnitOfWork;

        public ProductManagementServices(IProductUnitOfWork productUnitOfWork)
        {
            _productUnitOfWork = productUnitOfWork;
        }
        //public void CreateProduct()
        //{

        //}

        public void CreateProduct(Product product)
        {
            if (!_productUnitOfWork.ProductRepository.IsTitleDuplicate(product.ProductName))
            {
                _productUnitOfWork.ProductRepository.Add(product);
                _productUnitOfWork.Save();
            }
        }

        public void DeleteProduct(Guid id)
        {
            _productUnitOfWork.ProductRepository.Remove(id);
            _productUnitOfWork.Save();
        }

        public Product GetProduct(Guid id)
        {
            return _productUnitOfWork.ProductRepository.GetById(id);
        }

        public (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex,
            int pageSize, DataTablesSearch search, string? order)
        {
           return _productUnitOfWork.ProductRepository.GetPagedProducts(pageIndex, pageSize, search, order);
        }

        public void UpdateProduct(Product product)
        {
            if (!_productUnitOfWork.ProductRepository.IsTitleDuplicate(product.ProductName, product.Id))
            {
                _productUnitOfWork.ProductRepository.Edit(product);
                _productUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("ProductName Should be unique");
        }
    }
}
