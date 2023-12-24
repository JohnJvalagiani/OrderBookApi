using AutoMapper;
using DTOs;
using Entities;
using OrderBook.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, WriteOrderDto>().ReverseMap();
            CreateMap<Order, ReadOrderDto>().ReverseMap();
        }
    }
}
