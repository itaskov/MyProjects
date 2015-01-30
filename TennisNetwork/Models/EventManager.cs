using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TennisNetwork.Data;

namespace TennisNetwork.Models
{
    public class EventManager
    {
        private IEvent eventType;

        private int id;

        private IUowData data;

        public EventManager(IUowData data, int id, IEvent eventType)
        {
            this.Data = data;
            this.id = id;
            
            if (eventType == null)
            {
                throw new ArgumentException("Null is not a valid.", "eventType");
            }
            
            this.eventType = eventType;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public IUowData Data
        {
            get { return data; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Null is not a valid data.", "data");
                }

                data = value;
            }
        }
        
        public bool IsEventTaken()
        {
            return this.eventType.IsEventTaken(this.id, this.data);
        }
    }
}