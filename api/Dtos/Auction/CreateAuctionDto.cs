using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Auction
{
    public class CreateAuctionDto
    {


        public string Brand { get; set; } = string.Empty;

        public string Year { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AuctionImage { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public int Mileage { get; set; } = 0;

        public decimal StartingBid { get; set; }

        public DateTime EndTime { get; set; }





    }
}