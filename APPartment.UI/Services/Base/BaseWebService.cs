﻿using APPartment.Data.Core;
using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace APPartment.UI.Services.Base
{
    public class BaseWebService : MapperService
    {
        BaseFacade BaseFacade;
        long? CurrentUserId;

        public BaseWebService(long? currentUserId)
        {
            BaseFacade = new BaseFacade();
            CurrentUserId = currentUserId;
        }

        public T GetEntity<T>(long id)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var getObjectByIdFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetObject)))
                .FirstOrDefault()
                .MakeGenericMethod(serverModelType);

            var serverModel = getObjectByIdFunc
                .Invoke(BaseFacade, new object[] { id });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

            return result;
        }

        public T GetEntity<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var getObjectFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetObject)))
                .Last()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            var serverModel = getObjectFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

            return result;
        }

        public List<T> GetCollection<T>()
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var getObjectsFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetObjects)))
                .FirstOrDefault()
                .MakeGenericMethod(serverModelType);

            var serverModels = getObjectsFunc
                .Invoke(BaseFacade, null);

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModelCollection)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = toViewModelFunc.Invoke(this, new object[] { serverModels }) as List<T>;

            return result;
        }

        public List<T> GetCollection<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var getObjectsFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetObjects)))
                .Last()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            var serverModels = getObjectsFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModelCollection)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = toViewModelFunc.Invoke(this, new object[] { serverModels }) as List<T>;

            return result;
        }

        public void Save<T>(T model)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            if (model.Id > 0)
            {
                var updateFunc = typeof(BaseFacade)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.Name.Equals(nameof(BaseFacade.Update)))
                    .FirstOrDefault()
                    .MakeGenericMethod(serverModelType);

                var toServerModelFunc = typeof(MapperService)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.Name.Equals(nameof(MapperService.GetServerModel)))
                    .FirstOrDefault()
                    .MakeGenericMethod(typeof(T), serverModelType);

                var serverModel = toServerModelFunc.Invoke(this, new object[] { model });

                updateFunc
                    .Invoke(BaseFacade, new object[] { serverModel, CurrentUserId });
            }
            else
            {
                var createFunc = typeof(BaseFacade)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.Name.Equals(nameof(BaseFacade.Create)))
                    .FirstOrDefault()
                    .MakeGenericMethod(serverModelType);

                var toServerModelFunc = typeof(MapperService)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.Name.Equals(nameof(MapperService.GetServerModel)))
                    .FirstOrDefault()
                    .MakeGenericMethod(typeof(T), serverModelType);

                var serverModel = toServerModelFunc.Invoke(this, new object[] { model });

                createFunc
                    .Invoke(BaseFacade, new object[] { serverModel, CurrentUserId });
            }
        }

        public void Delete<T>(T model)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var deleteFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.Delete)))
                .FirstOrDefault()
                .MakeGenericMethod(serverModelType);

            var toServerModelFunc = typeof(MapperService)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.Name.Equals(nameof(MapperService.GetServerModel)))
                    .FirstOrDefault()
                    .MakeGenericMethod(typeof(T), serverModelType);

            var serverModel = toServerModelFunc.Invoke(this, new object[] { model });

            deleteFunc
                .Invoke(BaseFacade, new object[] { serverModel });
        }

        // TODO: Complete function
        public T GetLookupEntity<T>(long id)
            where T : class, ILookupObject, new()
        {
            // TODO: Implement mapping from server to view model

            var getLookupObjectByIdFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObject)))
                .FirstOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            return (T)getLookupObjectByIdFunc
                .Invoke(BaseFacade, new object[] { id });
        }

        // TODO: Complete function
        public T GetLookupEntity<T>(Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            // TODO: Implement mapping from server to view model

            var getLookupObjectFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObject)))
                .Last()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            return (T)getLookupObjectFilteredFunc
                .Invoke(BaseFacade, new object[] { filter });
        }

        // TODO: Complete function
        public List<T> GetLookupObjects<T>()
            where T : class, ILookupObject, new()
        {
            // TODO: Implement mapping from server to view model

            var getLookupObjectsFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObjects)))
                .FirstOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            return (List<T>)getLookupObjectsFunc
                .Invoke(BaseFacade, new object[] { });
        }

        // TODO: Complete function
        public List<T> GetLookupObjects<T>(Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            // TODO: Implement mapping from server to view model

            var getLookupObjectsFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObjects)))
                .FirstOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            return (List<T>)getLookupObjectsFilteredFunc
                .Invoke(BaseFacade, new object[] { filter });
        }

        public bool Any<T>()
            where T : class, IBaseObject, new()
        {
            var anyFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.Any)))
                .FirstOrDefault()
                .MakeGenericMethod(GetServerModelType<T>());

            return (bool)anyFunc
                .Invoke(BaseFacade, new object[] { });
        }

        public bool Any<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var anyFilteredFunc = typeof(BaseFacade)
                .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.Any)))
                .Last()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            return (bool)anyFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });
        }

        public int Count<T>()
            where T : class, IBaseObject, new()
        {
            var countFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.Count)))
                .FirstOrDefault()
                .MakeGenericMethod(GetServerModelType<T>());

            return (int)countFunc
                .Invoke(BaseFacade, new object[] { });
        }

        public int Count<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var countFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.Count)))
                .Last()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            return (int)countFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });
        }
    }
}