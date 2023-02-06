using AutoMapper;
using Server.Common.Classes.Models.Common;
using Server.Common.Classes.Models.EventModels;
using Server.Common.Classes.Models.UserModels;
using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<UserLogin, UserModel>().ReverseMap();
            CreateMap<NewUser, UserModel>().ReverseMap();
            CreateMap<User, NewUser>().ReverseMap();

            CreateMap<Event, EventModel>().ReverseMap();
            CreateMap<Event, NewEventWithUserId>().ReverseMap();
            CreateMap<Event, NewEventWithUsername>().ReverseMap();

            CreateMap<EventModel, NewEventWithUserId>().ReverseMap();
            CreateMap<EventModel, NewEventWithUsername>().ReverseMap();
        }
    }
}
