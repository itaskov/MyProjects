using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TennisNetwork.Data;

namespace TennisNetwork.Models
{
    public class Single : IEvent
    {
        #region IEvent Members

        public bool IsEventTaken(int id, IUowData data)
        {
            return true;
        }

        #endregion
    }
}