using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace buy_sale.database.Models
{
    [Table("sales")]
    public class Sale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("date", TypeName = "nvarchar(max)")]
        public DateOnly Date { get; set; }

        [Column("time")]
        public TimeOnly Time { get; set; }

        [Column("sales_point_id")]
        public int SalesPointId { get; set; }
        public SalesPoint SalesPoint { get; set; }

        [Column("buyer_id")]
        public int? BuyerId { get; set; }
        public Buyer Buyer { get; set; }

        public List<SalesData> SalesData { get; set; } = new();

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }
    }
}
