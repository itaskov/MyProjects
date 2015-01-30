using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TennisNetwork.Data;

namespace TennisNetwork.Models
{
    public class Double : IEvent
    {
        public const int Players = 4;
        
        #region IEvent Members

        public bool IsEventTaken(int id, IUowData data)
        {
            bool taken;
            int joinedPlayers = data.Users.All().Where(u => u.Events.Any(e => e.Id == id)).Count();

            if (joinedPlayers < Players)
            {
                taken = false;
            }
            else
            {
                taken = true;
            }

            return taken;
        }

        #endregion
    }
}