using System.Text.Json.Serialization;
namespace RelatedEntities.Models
{
    public class Product
    {
        public int Id  {get; set;}
        public string? Name  {get; set;} 
        public string? Description  {get; set;}
        public decimal Price { get; set; }
         // Relación: Un producto puede estar en muchas órdenes
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}