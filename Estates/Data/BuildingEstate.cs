using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Estates.Interfaces;

namespace Estates.Data
{
    public abstract class BuildingEstate : Estate, IBuildingEstate
    {
        private const int MinRooms = 0;
        private const int MaxRooms = 20;
        
        private int rooms;
        
        public BuildingEstate(int rooms = 0, bool hasElevator = false)
        {
            this.Rooms = rooms;
            this.HasElevator = hasElevator;
        }

        public override string ToString()
        {
            var result = string.Format("{0}, Rooms: {1}, Elevator: {2}",
                base.ToString(), this.rooms, (this.HasElevator ? "Yes" : "No"));
            return result;
        }
        
        #region IBuildingEstate Members

        public int Rooms
        {
            get
            {
                return this.rooms;
            }
            set
            {
                if (value < MinRooms || MaxRooms < value)
                {
                    throw new ArgumentOutOfRangeException("rooms", this.GetType().Name + " rooms should be in range [0…20].");
                }

                this.rooms = value;
            }
        }

        public bool HasElevator { get; set; }

        #endregion
    }
}
