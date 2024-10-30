using System.Diagnostics;
using api.Builder_Design_Pattern.AuctionBuilderDesingPattern;
using api.Dtos.Auction;
using Microsoft.VisualBasic;

namespace SA_Backend.api.Builder_Design_Pattern.AuctionBuilderDesingPattern
{
    public class Director
    {

        CarBuilder carBuilder;

        public Director(CarBuilder carBuilder)
        {
            this.carBuilder = carBuilder;
        }


        public Vehical construct(CreateAuctionDto dto, string userId)
        {
            carBuilder.buildBrand(dto.Brand);
            carBuilder.buildDescription(dto.Description);
            carBuilder.buildAuctionImage(dto.AuctionImage);
            carBuilder.buildYear(dto.Year);
            carBuilder.buildModel(dto.Model);
            carBuilder.buildMileage(dto.Mileage);
            carBuilder.buildStartingBid(dto.StartingBid);
            carBuilder.buildSellerId(userId);
            carBuilder.buildEndTime(dto.EndTime);

            return carBuilder.getVehical();
        }
        // Class implementation goes here
    }
}