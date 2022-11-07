using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyExchange.Repositories.Entities;

public class Trade
{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set;}
        
        [Column(TypeName="timestamp with time zone")]
        public DateTime CreatedAt { get; set; }
        
        [Column(TypeName="timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }
        
        [Column(TypeName="timestamp with time zone")]
        public DateTime? ArchivedAt { get; set; }
        
        public string Base { get; set; }
        public string Symbol { get; set; }
        public double Amount { get; set; }
}