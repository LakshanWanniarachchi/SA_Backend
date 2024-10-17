
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;


namespace api.Models
{
    public class Bid
    {


        [Key]
        public int BidId { get; set; }
        public int AuctionId { get; set; }
        public String BidderId { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = string.Empty;


        [Column(TypeName = "decimal(18, 2)")]
        public decimal BidAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }
}