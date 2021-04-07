using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.Services.Tools;
using APPartment.ORM.Framework.Declarations;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace APPartment.Infrastructure.Services.Base
{
    public abstract class MapperService
    {
        public T GetViewModel<T, U>(U serverModel)
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

        public List<T> GetViewModelCollection<T, U>(List<U> serverModels)
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

        public U GetServerModel<T, U>(T viewModel)
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

        public List<U> GetServerModelCollection<T, U>(List<T> viewModels)
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

        protected Type GetServerModelType<T>()
            where T : class, IBaseObject, new()
        {
            try
            {
                var serverModelType = (Type)typeof(T)
                .GetCustomAttributesData()
                .Where(x => x.AttributeType.Equals(typeof(IMapFromAttribute)))
                .FirstOrDefault()
                .ConstructorArguments
                .FirstOrDefault()
                .Value;

                return serverModelType;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Type '{0}' must have an IMapFrom attribute with a server model type value.", typeof(T).FullName));
            }
        }

        protected Type GetLookupServerModelType<T>()
            where T : class, ILookupObject, new()
        {
            try
            {
                var lookupServerModelType = (Type)typeof(T)
                .GetCustomAttributesData()
                .Where(x => x.AttributeType.Equals(typeof(IMapFromAttribute)))
                .FirstOrDefault()
                .ConstructorArguments
                .FirstOrDefault()
                .Value;

                return lookupServerModelType;
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Type '{0}' must have an IMapFrom attribute with a server model type value.", typeof(T).FullName));
            }
        }

        protected Expression<Func<U, bool>> ConvertExpression<T, U>(Expression<Func<T, bool>> source)
            where T : IBaseObject
            where U : IBaseObject
        {
            var result = source.ReplaceParameter<T, U>();
            return result;
        }
    }
}
