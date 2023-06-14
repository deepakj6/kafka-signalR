using System.ComponentModel.DataAnnotations;

namespace InventoryServer.Models
{
    public class BookCollection
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }               
    }

}
