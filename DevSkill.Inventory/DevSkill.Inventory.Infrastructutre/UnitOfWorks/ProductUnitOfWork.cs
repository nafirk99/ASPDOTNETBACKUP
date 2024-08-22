using DevSkill.Inventory.Application;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Domain.Repsitory_Contracts;
using DevSkill.Inventory.Infrastructure.UnitOfWorks;
using DevSkill.Inventory.Infrastructutre;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public class ProductUnitOfWork : UnitOfWork, IProductUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }

       // public IProductRepository productRepository => throw new NotImplementedException();           //Extra----------

        public ProductUnitOfWork(ProductDbCntext dbContext, 
            IProductRepository productRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
        }

        public async Task<(IList<Product> data, int total, int totalDisplay)> GetPagedProductsUsingSPAsync(int pageIndex,
            int pageSize, DataTablesSearch search, string? order)
        {
            var procedureName = "GetProducts";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<Product>(procedureName, 
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "ProductName", search.Value }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}
