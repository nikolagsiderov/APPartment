using APPartment.ORM.Framework.Core;
using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace APPartment.Data.Core
{
    public class BaseFacade
    {
        private readonly DaoContext dao;

        public BaseFacade()
        {
            dao = new DaoContext();
        }

        public T GetObject<T>(long id)
            where T : class, IIdentityBaseObject, new()
        {
            var result = new T();
            result = dao.SelectGetObject<T>(result, id);
            return result;
        }

        public T GetObject<T>(Expression<Func<T, bool>> filter)
            where T : class, IIdentityBaseObject, new()
        {
            var result = new T();
            result = dao.SelectFilterGetObject<T>(result, filter);
            return result;
        }

        // TODO: Finish this here...
        public List<T> GetObjects<T>()
            where T : class, IIdentityBaseObject, new()
        {
            var result = new List<T>();
            result = dao.SelectGetObjects<T>(result);
            return result;
        }

        // TODO: Finish this here...
        public List<T> GetObjects<T>(Expression<Func<T, bool>> filter)
            where T : class, IIdentityBaseObject, new()
        {
            var result = new List<T>();
            result = dao.SelectGetObjects<T>(result, filter);
            return result;
        }

        public void Create<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            var objectId = dao.SaveCreateBaseObject(businessObject);
            dao.SaveCreateBusinessObject(businessObject, objectId);
        }

        public void Update<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            dao.SaveUpdateBaseObject(businessObject);
            dao.SaveUpdateBusinessObject(businessObject);
        }

        public void Delete<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            dao.DeleteBusinessAndBaseObject(businessObject);
        }
    }
}
