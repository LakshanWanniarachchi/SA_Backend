using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Bids
{
    public class CreateBidDto
    {



        public int AuctionId { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal BidAmount { get; set; }




    }
}