using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public abstract class Offer : IOffer
    {
        private IEstate estate;

        public Offer(IEstate estate = null)
        {
            this.estate = estate;
        }

        public override string ToString()
        {
            var result = string.Format("{0}: Estate = {1}, Location = {2}", 
                                        this.Type, this.estate.Name, this.estate.Location);
            return result;
        }
        
        #region IOffer Members

        public OfferType Type { get; set; }

        public IEstate Estate
        {
            get
            {
                return this.estate;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("estate");
                }

                this.estate = value;
            }
        }

        #endregion
    }
}
