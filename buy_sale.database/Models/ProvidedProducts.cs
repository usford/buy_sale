using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace buy_sale.database.Models
{
    [Table("provided_products")]
    public class ProvidedProducts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Column("product_id")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Column("product_quantity")]
        public int ProductQuantity { get; set; }

        [Column("sales_point_id")]
        public int SalesPointId { get; set; }
        public SalesPoint SalesPoint { get; set; }
    }
}
