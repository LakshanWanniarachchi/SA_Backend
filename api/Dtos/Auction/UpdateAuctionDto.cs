using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Auction
{
    public class UpdateAuctionDto
    {


        public string Brand { get; set; }

        public string Year { get; set; }
        public string Description { get; set; }
        public string AuctionImage { get; set; }
        public string Model { get; set; }

        public int Mileage { get; set; }

        public decimal StartingBid { get; set; }

        public DateTime EndTime { get; set; }





    }
}