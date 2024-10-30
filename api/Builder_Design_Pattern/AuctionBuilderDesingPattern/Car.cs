


namespace api.Builder_Design_Pattern.AuctionBuilderDesingPattern
{
    public class Car : CarBuilder
    {

        Vehical vehical;

        public Car()
        {
            vehical = new Vehical();
        }
        public void buildAuctionImage(string auctionImage)
        {
            vehical.setAuctionImage(auctionImage);
        }

        public void buildBrand(string brand)
        {
            vehical.setBrand(brand);
        }

        public void buildDescription(string description)
        {
            vehical.setDescription(description);
        }

        public void buildEndTime(DateTime endTime)
        {
            vehical.setEndTime(endTime);
        }

        public void buildMileage(int mileage)
        {
            vehical.setMileage(mileage);
        }

        public void buildModel(string model)
        {
            vehical.setModel(model);
        }

        public void buildSellerId(string sellerId)
        {
            vehical.setSellerId(sellerId);
        }

        public void buildStartingBid(decimal startingBid)
        {
            vehical.setStartingBid(startingBid);
        }

        public void buildYear(string year)
        {
            vehical.setYear(year);
        }

        public Vehical getVehical()
        {
            return vehical;
        }
    }
}
