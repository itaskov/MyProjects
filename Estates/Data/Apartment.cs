using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public class Apartment : BuildingEstate, IApartment
    {
        public Apartment()
            : base()
        {
            this.Type = EstateType.Apartment;
        }
    }
}
