using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using DevSkill.Inventory.Domain.Repsitory_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application
{
    public interface IProductUnitOfWork : IUnitOfWork
    {
         IProductRepository ProductRepository { get; }
         Task<(IList<Product> data, int total, int totalDisplay)> GetPagedProductsUsingSPAsync(int pageIndex,
            int pageSize, DataTablesSearch search, string? order);
    }
}
