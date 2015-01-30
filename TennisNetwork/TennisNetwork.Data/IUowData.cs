using TennisNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisNetwork.Data;

namespace TennisNetwork.Data
{
    public interface IUowData
    {
        IRepository<Address> Addresses { get; }

        IRepository<Event> Events { get; }

        IRepository<UserLevel> UserLevels { get; }

        IRepository<ApplicationUser> Users { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
