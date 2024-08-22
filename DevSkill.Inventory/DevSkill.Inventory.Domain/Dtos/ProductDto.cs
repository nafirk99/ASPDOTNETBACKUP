using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }  //Title
        public string Description { get; set; }  //Body 
        public DateTime ProductCreateDate { get; set; }  //PostDate

        public string CategoryName { get; set; }
    }
}
