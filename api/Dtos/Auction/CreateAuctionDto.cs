using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Auction
{
    public class CreateAuctionDto
    {


        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AuctionImage { get; set; } = string.Empty;
        public string AuctionCategory { get; set; } = string.Empty;


        public DateTime EndTime { get; set; }


        public decimal StartingBid { get; set; }


        public string? Status { get; set; } = string.Empty;





    }
}