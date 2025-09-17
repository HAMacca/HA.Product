using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HA.Product.Models
{
    public class Item
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        public decimal Price { get; set; }
        public string? urlImage { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }

}
