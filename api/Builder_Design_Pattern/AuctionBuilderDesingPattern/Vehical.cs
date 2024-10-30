

namespace api.Builder_Design_Pattern.AuctionBuilderDesingPattern
{
    public class Vehical
    {

        private string Brand;
        private string Description;

        private string AuctionImage;

        private string Year;

        private string Model;

        private int Mileage;

        private decimal StartingBid;

        private string SellerId;

        private DateTime EndTime;

        public void setBrand(string brand)
        {
            this.Brand = brand;
        }

        public void setDescription(string description)
        {
            this.Description = description;
        }

        public void setAuctionImage(string auctionImage)
        {
            this.AuctionImage = auctionImage;
        }

        public void setYear(string year)
        {
            this.Year = year;
        }


        public void setModel(string model)
        {
            this.Model = model;
        }


        public void setMileage(int mileage)
        {
            this.Mileage = mileage;
        }


        public void setStartingBid(decimal startingBid)
        {
            this.StartingBid = startingBid;
        }

        public void setSellerId(string sellerId)
        {
            this.SellerId = sellerId;
        }

        public void setEndTime(DateTime endTime)
        {
            this.EndTime = endTime;
        }







    }
}
