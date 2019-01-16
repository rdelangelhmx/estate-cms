﻿using AutoMapper;
using Tti.Estate.Data;
using Tti.Estate.Web.Models;

namespace Tti.Estate.Web
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<IPagedResult, DataTableModel>().
                ForMember(x => x.FilteredItems, x => x.MapFrom(y => y.TotalItems)).
                ForMember(x => x.DrawCounter, x => x.Ignore());

            CreateMap(typeof(IPagedResult<>), typeof(DataTableModel<>)).
                IncludeBase(typeof(IPagedResult), typeof(DataTableModel));
        }
    }
}