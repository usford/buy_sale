using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace buy_sale.database.Models
{
    [Table("sales_data")]
    public class SalesData
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("product_id")]
        [JsonPropertyName("productId")]
        public int ProductId {get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

        [Column("product_quantity")]
        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }

        [Column("product_amount")]
        [JsonPropertyName("productAmount")]
        public decimal ProductAmount{ get; set; }

        [Column("sale_id")]
        [JsonPropertyName("saleId")]
        public int SaleId { get; set; }
        [JsonIgnore]
        public Sale Sale { get; set; }
    }
}
