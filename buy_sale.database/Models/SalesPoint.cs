using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace buy_sale.database.Models
{
    [Table("sales_points")]
    public class SalesPoint
    {
        public SalesPoint()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public List<ProvidedProducts> ProvidedProducts { get; set; } = new();
    }
}
