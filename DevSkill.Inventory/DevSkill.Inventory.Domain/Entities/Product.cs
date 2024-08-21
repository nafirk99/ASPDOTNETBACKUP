namespace DevSkill.Inventory.Domain.Entities
{
    public class Product : IEntity<Guid>
    {
        public Guid Id { get ; set; }
        public string ProductName { get; set; }  //Title
        public string Description { get; set; }  //Body 
        public DateTime ProductCreateDate { get; set; }  //PostDate

        public Category Category { get; set; }
       
    }
}
