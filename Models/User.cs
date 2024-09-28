using System.Text.Json.Serialization;

namespace RelatedEntities.Models
{
    public class User
    {
        public int Id  {get; set;}
        public string? Names  {get; set;}
        public string? LastNames  {get; set;}
        public string? Email  {get; set;}
         // Relación: Un usuario puede tener muchas órdenes
         [JsonIgnore]
        public ICollection<Order> Orders { get; set; }
    }
}