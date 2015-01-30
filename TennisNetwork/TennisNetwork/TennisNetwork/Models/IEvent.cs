using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisNetwork.Data;

namespace TennisNetwork.Models
{
    public interface IEvent
    {
        bool IsEventTaken(int id, IUowData data);
    }
}
