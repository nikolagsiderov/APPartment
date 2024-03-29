﻿using APPartment.Data.Server.Base;
using APPartment.Infrastructure.UI.Common.ViewModels;
using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace APPartment.Infrastructure.Services.Base
{
    public class BaseCRUDService : MapperService
    {
        private BaseFacade BaseFacade;
        protected long? CurrentUserID;
        protected long? CurrentHomeID;

        public BaseCRUDService(long? currentUserID, long? currentHomeID)
        {
            BaseFacade = new BaseFacade();
            CurrentUserID = currentUserID;
            CurrentHomeID = currentHomeID;
        }

        public T GetEntity<T>(long ID)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            var getObjectByIDFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetObject)))
                .FirstOrDefault()
                .MakeGenericMethod(serverModelType);

            var serverModel = getObjectByIDFunc
                .Invoke(BaseFacade, new object[] { ID });

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
                .LastOrDefault()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
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
                .LastOrDefault()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
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

        public BusinessObjectDisplayViewModel GetEntity(long objectID)
        {
            var serverModelType = GetServerModelType<BusinessObjectDisplayViewModel>();

            var getObjectByIDFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetBusinessObject)))
                .FirstOrDefault();

            var serverModel = getObjectByIDFunc
                .Invoke(BaseFacade, new object[] { objectID });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(BusinessObjectDisplayViewModel), serverModelType);

            var result = (BusinessObjectDisplayViewModel)toViewModelFunc.Invoke(this, new object[] { serverModel });

            return result;
        }

        public BusinessObjectDisplayViewModel GetEntity(Expression<Func<BusinessObjectDisplayViewModel, bool>> filter)
        {
            var serverModelType = GetServerModelType<BusinessObjectDisplayViewModel>();

            var getObjectFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetBusinessObject)))
                .LastOrDefault();

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(BusinessObjectDisplayViewModel), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            var serverModel = getObjectFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(BusinessObjectDisplayViewModel), serverModelType);

            var result = (BusinessObjectDisplayViewModel)toViewModelFunc.Invoke(this, new object[] { serverModel });

            return result;
        }

        public List<BusinessObjectDisplayViewModel> GetCollection(Expression<Func<BusinessObjectDisplayViewModel, bool>> filter)
        {
            var serverModelType = GetServerModelType<BusinessObjectDisplayViewModel>();

            var getObjectsFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetBusinessObjects)))
                .LastOrDefault();

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(BusinessObjectDisplayViewModel), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            var serverModels = getObjectsFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModelCollection)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(BusinessObjectDisplayViewModel), serverModelType);

            var result = toViewModelFunc.Invoke(this, new object[] { serverModels }) as List<BusinessObjectDisplayViewModel>;

            return result;
        }

        public T GetLookupEntity<T>(long ID)
            where T : class, ILookupObject, new()
        {
            var serverModelType = GetLookupServerModelType<T>();

            var getLookupObjectByIDFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObject)))
                .FirstOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            var serverModel = getLookupObjectByIDFunc
                .Invoke(BaseFacade, new object[] { ID });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetLookupViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

            return result;
        }

        public T GetLookupEntity<T>(Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            var serverModelType = GetLookupServerModelType<T>();

            var getLookupObjectFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObject)))
                .LastOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            var serverModel = getLookupObjectFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetLookupViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

            return result;
        }

        public List<T> GetLookupCollection<T>()
            where T : class, ILookupObject, new()
        {
            var serverModelType = GetLookupServerModelType<T>();

            var getLookupObjectsFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObjects)))
                .FirstOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            var serverModels = getLookupObjectsFunc
                .Invoke(BaseFacade, null);

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetLookupViewModelCollection)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = toViewModelFunc.Invoke(this, new object[] { serverModels }) as List<T>;

            return result;
        }

        public List<T> GetLookupCollection<T>(Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            var serverModelType = GetLookupServerModelType<T>();

            var getLookupObjectsFilteredFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObjects)))
                .LastOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertLookupExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            var serverModels = getLookupObjectsFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });

            var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetLookupViewModelCollection)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var result = toViewModelFunc.Invoke(this, new object[] { serverModels }) as List<T>;

            return result;
        }

        public T Save<T>(T model)
            where T : class, IBaseObject, new()
        {
            var serverModelType = GetServerModelType<T>();

            if (model.ID > 0)
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

                serverModel = updateFunc
                    .Invoke(BaseFacade, new object[] { serverModel, CurrentUserID, CurrentHomeID });

                var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

                var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

                return result;
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

                serverModel = createFunc
                    .Invoke(BaseFacade, new object[] { serverModel, CurrentUserID, CurrentHomeID });

                var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

                var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

                return result;
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

        public bool Any<T>()
            where T : class, IBaseObject, new()
        {
            var anyFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(this.Any)))
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
                .Where(x => x.Name.Equals(nameof(this.Any)))
                .LastOrDefault()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
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
                .Where(x => x.Name.Equals(nameof(this.Count)))
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
                .Where(x => x.Name.Equals(nameof(this.Count)))
                .LastOrDefault()
                .MakeGenericMethod(serverModelType);

            var convertFilterFunc = typeof(MapperService)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            return (int)countFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });
        }

        public string GetObjectTypeName(long objectTypeID)
        {
            return BaseFacade.GetObjectTypeName(objectTypeID);
        }

        public void AddUserAsParticipantToObjectIfNecessary(long targetObjectID, long userID)
        {
            BaseFacade.AddUserAsParticipantToObjectIfNecessary(targetObjectID, userID);
        }
    }
}
