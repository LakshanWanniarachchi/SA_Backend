using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public bool IsPaid { get; set; } = false;
    public string BuyerId { get; set; }


    [StringLength(255)]
    public string SellerId { get; set; }
    public int AuctionId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;  // e.g., "Stripe"

    [StringLength(50)]
    public string PaymentStatus { get; set; } = string.Empty;  // e.g., "Pending", "Completed"

    [StringLength(255)]
    public string TransactionId { get; set; } = string.Empty;  // Stripe's PaymentIntentId

    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
