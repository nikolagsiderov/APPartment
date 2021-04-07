using APPartment.Data.Core;
using APPartment.Infrastructure.UI.Common.ViewModels;
using APPartment.ORM.Framework.Declarations;
using APPartment.ORM.Framework.Enums;
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

        public BaseCRUDService(long? currentUserID)
        {
            BaseFacade = new BaseFacade();
            CurrentUserID = currentUserID;
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
                .Last()
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
                .Last()
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
                    .Invoke(BaseFacade, new object[] { serverModel, CurrentUserID });

                var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

                var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

                AddUserAsParticipantToObject(result.ObjectID, (long)result.ModifiedByID, result.ObjectTypeID);

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
                    .Invoke(BaseFacade, new object[] { serverModel, CurrentUserID });

                var toViewModelFunc = typeof(MapperService)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(MapperService.GetViewModel)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

                var result = (T)toViewModelFunc.Invoke(this, new object[] { serverModel });

                AddUserAsParticipantToObject(result.ObjectID, (long)result.ModifiedByID, result.ObjectTypeID);

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

        // TODO: Complete function
        public T GetLookupEntity<T>(long ID)
            where T : class, ILookupObject, new()
        {
            // TODO: Implement mapping from server to view model

            var getLookupObjectByIDFunc = typeof(BaseFacade)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(BaseFacade.GetLookupObject)))
                .FirstOrDefault()
                .MakeGenericMethod(GetLookupServerModelType<T>());

            return (T)getLookupObjectByIDFunc
                .Invoke(BaseFacade, new object[] { ID });
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
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.Name.Equals(nameof(ConvertExpression)))
                .FirstOrDefault()
                .MakeGenericMethod(typeof(T), serverModelType);

            var convertedFilter = convertFilterFunc.Invoke(this, new object[] { filter });

            return (int)countFilteredFunc
                .Invoke(BaseFacade, new object[] { convertedFilter });
        }

        public void AddUserAsParticipantToObject(long targetObjectID, long userID, long objectTypeID)
        {
            if (userID != 0)
            {
                if (objectTypeID != (long)ObjectTypes.ObjectParticipant)
                {
                    if (!Any<ObjectParticipantPostViewModel>(x => x.TargetObjectID == targetObjectID && x.UserID == userID))
                    {
                        var participant = new ObjectParticipantPostViewModel() { TargetObjectID = targetObjectID, UserID = userID };
                        Save(participant);
                    }
                }
            }
        }
    }
}
