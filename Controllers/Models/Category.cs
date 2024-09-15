using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product_Creation.Controllers.Models
{
    [Table("categories")]
    public class Category
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
