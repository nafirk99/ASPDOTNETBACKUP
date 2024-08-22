
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public interface IProductManagementServices
    {
        void CreateProduct(Product product);
        void DeleteProduct(Guid id);
        Product GetProduct(Guid id);
        (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);

        Task<(IList<Product> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        void UpdateProduct(Product product);
    }
}