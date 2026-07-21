using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;


namespace Kader.Services.Utilities.Mappers
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfiles(GetAllProfiles());
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
        private static IEnumerable<Profile> GetAllProfiles()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(Profile).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract && !x.FullName.Contains("AutoMapper.Configuration.MapperConfigurationExpression"))
                .Select(x => (Profile)Activator.CreateInstance(x)).ToList();
        }
        private static bool CheckProperty(PropertyInfo p)
        {
            return p.GetMethod != null && (p.GetMethod.IsPublic || p.GetMethod.IsAssembly);
        }
    }
}