using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public class Office : BuildingEstate, IOffice
    {
        public Office()
        {
            this.Type = EstateType.Office;
        }
    }
}
