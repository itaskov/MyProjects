using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public class RentOffer : Offer, IRentOffer
    {
        public RentOffer(decimal pricePerMonth = 0)
        {
            this.PricePerMonth = pricePerMonth;
            
            this.Type = OfferType.Rent;
        }

        public override string ToString()
        {
            var result = string.Format("{0}, Price = {1}",
                                        base.ToString(), this.PricePerMonth);
            return result;
        }

        #region IRentOffer Members

        public decimal PricePerMonth { get; set; }

        #endregion
    }
}
