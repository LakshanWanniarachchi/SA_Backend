
namespace api.Builder_Design_Pattern.AuctionBuilderDesingPattern
{
    public interface CarBuilder
    {

        public void buildBrand(string brand);

        public void buildDescription(string description);

        public void buildAuctionImage(string auctionImage);

        public void buildYear(string year);

        public void buildModel(string model);

        public void buildMileage(int mileage);


        public void buildStartingBid(decimal startingBid);

        public void buildSellerId(string sellerId);

        public void buildEndTime(DateTime endTime);


        public Vehical getVehical();







    }
}