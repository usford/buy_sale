using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace buy_sale.database.Models
{
    [Table("sales_data")]
    public class SalesData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Column("product_id")]
        public int ProductId {get; set; }
        public Product Product { get; set; }

        [Column("product_quantity")]
        public int ProductQuantity { get; set; }

        [Column("product_amount")]
        public decimal ProductAmount{ get; set; }

        [Column("sale_id")]
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
