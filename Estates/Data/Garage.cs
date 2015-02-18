using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public class Garage : Estate, IGarage
    {
        private const int MinSize = 0;
        private const int MaxSize = 500;
        
        private int width;
        private int height;

        public Garage(int width = 0, int height = 0)
        {
            this.Width = width;
            this.Height = height;
        }

        public override string ToString()
        {
            var result = string.Format("{0}, Width: {1}, Height: {2}",
                base.ToString(), this.width, this.height);
            return result;
        }
        
        #region IGarage Members

        /// <summary>
        /// Measure in meters.
        /// </summary>
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.CheckSizeRange("width", value);
                this.width = value;
            }
        }

        /// <summary>
        /// Measure in meters.
        /// </summary>
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.CheckSizeRange("height", value);
                this.height = value;
            }
        }

        #endregion

        private void CheckSizeRange(string propName, int propValue)
        {
            if (propValue < MinSize || MaxSize < propValue)
            {
                throw new ArgumentOutOfRangeException(propName, propName + "should be in range [0…500].");
            }
        }
    }
}
