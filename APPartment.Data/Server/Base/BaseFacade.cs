using APPartment.Data.Server.Models;
using APPartment.ORM.Framework.Core;
using APPartment.ORM.Framework.Declarations;
using APPartment.ORM.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace APPartment.Data.Server.Base
{
    public class BaseFacade
    {
        private readonly DaoContext dao;

        public BaseFacade()
        {
            dao = new DaoContext();
        }

        public T GetObject<T>(long ID)
            where T : class, IBaseObject, new()
        {
            var result = new T();
            result = dao.SelectGetObject<T>(result, ID);
            return result;
        }

        public T GetObject<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var result = new T();
            result = dao.SelectGetObject<T>(result, filter);
            return result;
        }

        public List<T> GetObjects<T>()
            where T : class, IBaseObject, new()
        {
            var result = new List<T>();
            result = dao.SelectGetObjects<T>(result);
            return result;
        }

        public List<T> GetObjects<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var result = new List<T>();
            result = dao.SelectGetObjects<T>(result, filter);
            return result;
        }

        public BusinessObject GetBusinessObject(long objectID)
        {
            var result = new BusinessObject();
            result = dao.SelectGetBusinessObject<BusinessObject>(result, objectID);
            return result;
        }

        public BusinessObject GetBusinessObject(Expression<Func<BusinessObject, bool>> filter)
        {
            var result = new BusinessObject();
            result = dao.SelectGetBusinessObject<BusinessObject>(result, filter);
            return result;
        }

        public List<BusinessObject> GetBusinessObjects(Expression<Func<BusinessObject, bool>> filter)
        {
            var result = new List<BusinessObject>();
            result = dao.SelectGetBusinessObjects<BusinessObject>(result, filter);
            return result;
        }

        public T GetLookupObject<T>(long ID)
            where T : class, ILookupObject, new()
        {
            var result = new T();
            result = dao.SelectGetLookupObject<T>(result, ID);
            return result;
        }

        public T GetLookupObject<T>(Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            var result = new T();
            result = dao.SelectGetLookupObject<T>(result, filter);
            return result;
        }

        public List<T> GetLookupObjects<T>()
            where T : class, ILookupObject, new()
        {
            var result = new List<T>();
            result = dao.SelectGetLookupObjects<T>(result);
            return result;
        }

        public List<T> GetLookupObjects<T>(Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            var result = new List<T>();
            result = dao.SelectGetLookupObjects<T>(result, filter);
            return result;
        }

        public T Create<T>(T businessObject, long userID, long homeID)
            where T : class, IBaseObject
        {
            var objectID = dao.SaveCreateBaseObject(businessObject, userID, homeID);
            return dao.SaveCreateBusinessObject(businessObject, objectID);
        }

        public T Update<T>(T businessObject, long userID, long homeID)
            where T : class, IBaseObject
        {
            dao.SaveUpdateBaseObject(businessObject, userID, homeID);
            return dao.SaveUpdateBusinessObject(businessObject);
        }

        public void Delete<T>(T businessObject)
            where T : class, IBaseObject
        {
            dao.DeleteBusinessObjectAndAllItsBaseReferences(businessObject);
        }

        public bool Any<T>()
            where T : class, IBaseObject
        {
            var result = false;
            return dao.AnyBusinessObjects<T>(result);
        }

        public bool Any<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject
        {
            var result = false;
            return dao.AnyBusinessObjects<T>(result, filter);
        }

        public int Count<T>()
            where T : class, IBaseObject
        {
            var result = 0;
            return dao.CountBusinessObjects<T>(result);
        }

        public int Count<T>(Expression<Func<T, bool>> filter)
            where T : class, IBaseObject
        {
            var result = 0;
            return dao.CountBusinessObjects<T>(result, filter);
        }

        public string GetObjectTypeName(long objectTypeID)
        {
            var objectType = (ObjectTypes)objectTypeID;
            return objectType.ToString();
        }
    }
}
