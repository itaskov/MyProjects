using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public class SaleOffer : Offer, ISaleOffer
    {
        public SaleOffer(decimal price = 0)
        {
            this.Price = price;
            
            this.Type = OfferType.Sale;
        }

        public override string ToString()
        {
            var result = string.Format("{0}, Price = {1}",
                                        base.ToString(), this.Price);
            return result;
        }

        #region ISaleOffer Members

        public decimal Price { get; set; }

        #endregion
    }
}
