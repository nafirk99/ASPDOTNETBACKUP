using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repsitory_Contracts;
using DevSkill.Inventory.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructutre.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(ProductDbCntext context) : base(context)
        {
                
        }

        public bool IsTitleDuplicate(string productname, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.ProductName == productname) > 0;
            }
            else
            {
                return GetCount(x => x.ProductName == productname) > 0;
            }
        }

        public (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null , order, null, pageIndex, pageSize, true);
            else 
                return  GetDynamic(x => x.ProductName.Contains (search.Value), order, null, pageIndex, pageSize, true);
        }
    }
}
