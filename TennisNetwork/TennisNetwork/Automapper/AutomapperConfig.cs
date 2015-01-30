using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TennisNetwork.Models;

namespace TennisNetwork.Automapper
{
    public static class AutomapperConfig
    {
        public static void Map()
        {
            Mapper.CreateMap<Event, AutomapperEventViewModel>()
                .ForMember(d => d.EndDate, opt => opt.MapFrom(s => s.EndDateTime));

        }
    }
}