using System;
using SMARTplanner.Entities.Domain;
using SMARTplanner.Models;

namespace SMARTplanner
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Project, ProjectListItemViewModel>()
                    .ForMember(dest => dest.CodeName,
                        opts => opts.Condition(src => !string.IsNullOrEmpty(src.CodeName)));


            });
        }
    }
}