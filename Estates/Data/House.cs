using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public class House : Estate, IHouse
    {
        private const int MinFloor = 0;
        private const int MaxFloor = 10;
        
        private int floor;

        public House(int floor = 0)
        {
            this.Floors = floor;
        }

        public override string ToString()
        {
            var result = string.Format("{0}, Floor: {1}",
                base.ToString(), this.floor);
            return result;
        }
        
        #region IHouse Members

        public int Floors
        {
            get
            {
                return this.floor;
            }
            set
            {
                if (value < MinFloor || MaxFloor < value)
                {
                    throw new ArgumentOutOfRangeException("floor", "House floors should be in range [0…10].");
                }

                this.floor = value;
            }
        }

        #endregion
    }
}
