namespace RelatedEntities.Models
{
    public class Order
    {
        public int Id  {get; set;}
         // Llave foránea hacia Product
        public int ProductId   {get; set;}
        
        public Product Product {get; set;}
         // Llave foránea hacia User
        public int UserId   {get; set;}
        
        public User User {get; set;}
    }
}