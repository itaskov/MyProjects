using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estates.Interfaces;

namespace Estates.Data
{
    public abstract class Estate : IEstate
    {
        private const double MinArea = 0;
        private const double MaxArea = 10000;
        
        private string name;
        private double area;
        private string location;

        public Estate(string name = null, double area = 0, string location = null, bool isFurnished = false)
        {

        }

        public override string ToString()
        {
            var result = string.Format("{0}: Name = {1}, Area = {2}, Location = {3}, Furnitured = {4}", 
                                        this.GetType().Name, this.name, this.area, this.location, this.IsFurnished);
            return result;
        }
        
        #region IEstate Members

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("name");
                }
                
                this.name = value;
            }
        }

        public EstateType Type { get; set; }

        
        /// <summary>
        /// Measure in square meters.
        /// </summary>
        public double Area
        {
            get
            {
                return this.area;
            }
            set
            {
                if (value < MinArea || MaxArea < value)
                {
                    throw new ArgumentOutOfRangeException("area", "Estate area should be in range [0…10000].");
                }

                this.area = value;
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("location");
                }

                this.location = value;
            }
        }

        public bool IsFurnished { get; set; }

        #endregion
    }
}
