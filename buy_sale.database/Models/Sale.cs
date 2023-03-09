using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace buy_sale.database.Models
{
    [Table("sales")]
    public class Sale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("date", TypeName = "nvarchar(max)")]
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }

        [Column("time")]
        [JsonPropertyName("time")]
        public TimeOnly Time { get; set; }

        [Column("sales_point_id")]
        [JsonPropertyName("salesPointId")]
        public int SalesPointId { get; set; }
        [JsonIgnore]
        public SalesPoint SalesPoint { get; set; }

        [Column("buyer_id")]
        [JsonPropertyName("buyerId")]
        public int? BuyerId { get; set; }
        [JsonIgnore]
        public Buyer Buyer { get; set; }
        [JsonPropertyName("salesData")]
        public List<SalesData> SalesData { get; set; } = new();

        [Column("total_amount")]
        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }
    }
}
