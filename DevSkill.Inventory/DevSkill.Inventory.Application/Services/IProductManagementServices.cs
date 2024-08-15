
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public interface IProductManagementServices
    {
        void CreateProduct(Product product);
        (IList<Product> data, int total, int totalDisplay) GetProducts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
    }
}