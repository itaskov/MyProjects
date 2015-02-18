using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Engine;
using Estates.Interfaces;

namespace Estates.Data
{
    class NewEstateEngine : EstateEngine
    {
        public override string ExecuteCommand(string cmdName, string[] cmdArgs)
        {
            switch (cmdName)
            {
                case "find-rents-by-location":
                    return this.ExecuteFindRentsByLocationCommand(cmdArgs[0]);
                case "find-rents-by-price":
                    return this.ExecuteFindRentsByLocationCommand(cmdArgs);
                default:
                    return base.ExecuteCommand(cmdName, cmdArgs);
            }
        }

        private string ExecuteFindRentsByLocationCommand(string location)
        {
            var rents = this.Offers.Where(o => o.Estate.Location == location && o.Type == OfferType.Rent)
                                   .OrderBy(o => o.Estate.Name);
            return this.FormatQueryResults(rents);
        }

        private string ExecuteFindRentsByLocationCommand(string[] cmdArgs)
        {
            decimal minPrice = decimal.Parse(cmdArgs[0]);
            decimal maxPrice = decimal.Parse(cmdArgs[1]);

            var rents = this.Offers.FindAll(o => (o.Type == OfferType.Rent
                                                 && minPrice <= ((IRentOffer)o).PricePerMonth && ((IRentOffer)o).PricePerMonth <= maxPrice))
                                   .OrderBy(o => ((IRentOffer)o).PricePerMonth).ThenBy(o => o.Estate.Name);
            return this.FormatQueryResults(rents);
        }
    }
}
