using APPartment.ORM.Framework.Declarations;
using AutoMapper;
using System.Collections.Generic;

namespace APPartment.Infrastructure.Tools
{
    public static class IBaseObjectExtensions
    {
        public static T GetViewModel<T, U>(this U serverModel)
            where T : IBaseObject
            where U : IBaseObject
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<U, T>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<U, T>(serverModel);
        }

        public static List<T> GetViewModelCollection<T, U>(this List<U> serverModels)
            where T : IBaseObject
            where U : IBaseObject
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<U, T>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<List<U>, List<T>>(serverModels);
        }

        public static U GetServerModel<T, U>(this T viewModel)
            where T : IBaseObject
            where U : IBaseObject
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, U>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<T, U>(viewModel);
        }

        public static List<U> GetServerModelCollection<T, U>(this List<T> viewModels)
            where T : IBaseObject
            where U : IBaseObject
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, U>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<List<T>, List<U>>(viewModels);
        }
    }
}
