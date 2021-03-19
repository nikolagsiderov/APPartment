using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using APPartment.ORM.Framework.Helpers;
using APPartment.ORM.Framework.Declarations;
using APPartment.ORM.Framework.Attributes;

namespace APPartment.ORM.Framework.Core
{
    public class DaoContext
    {
        private ExpressionToSqlHelper expressionTranslator;

        public DaoContext()
        {
            expressionTranslator = new ExpressionToSqlHelper();
        }

        public T SelectGetObject<T>(T result, long id)
            where T : class, IBaseObject, new()
        {
            var mainTableName = typeof(T).Name;
            var selectBusinessObjectSqlQuery = SqlQueryProvider.SelectBusinessObjectById(mainTableName, id.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectBusinessObjectSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo property = result.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            property.SetValue(result, reader.GetValue(i), null);
                        }
                    }
                }

                conn.Close();
            }

            return result;
        }

        public T SelectFilterGetObject<T>(T result, Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var mainTableName = typeof(T).Name;
            var sqlClause = expressionTranslator.Translate(filter);

            var selectBusinessObjectSqlQuery = SqlQueryProvider.SelectBusinessObjectByClause(mainTableName, sqlClause);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectBusinessObjectSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo property = result.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            if (reader.GetValue(i) == DBNull.Value)
                                continue;

                            property.SetValue(result, reader.GetValue(i), null);
                        }
                    }
                }

                conn.Close();
            }

            if (result.ObjectId == 0)
                result = null;

            return result;
        }

        public List<T> SelectGetObjects<T>(List<T> result)
            where T : class, IBaseObject, new()
        {
            var mainTableName = typeof(T).Name;
            var selectBusinessObjectsSqlQuery = SqlQueryProvider.SelectBusinessObjects(mainTableName);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForObjectTablePrimaryKeyAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTablePrimaryKeyAttribute))).Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectBusinessObjectsSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj = (T)Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < propertiesCount; i++)
                    {
                        PropertyInfo property = obj.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            property.SetValue(obj, reader.GetValue(i), null);
                        }
                    }

                    result.Add(obj);
                }

                conn.Close();
            }

            return result;
        }

        public List<T> SelectGetObjects<T>(List<T> result, Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var mainTableName = typeof(T).Name;
            var sqlClause = expressionTranslator.Translate(filter);

            var selectBusinessObjectsSqlQuery = SqlQueryProvider.SelectBusinessObjectsByClause(mainTableName, sqlClause);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForObjectTablePrimaryKeyAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTablePrimaryKeyAttribute))).Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectBusinessObjectsSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj = (T)Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < propertiesCount; i++)
                    {
                        PropertyInfo property = obj.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            property.SetValue(obj, reader.GetValue(i), null);
                        }
                    }

                    result.Add(obj);
                }

                conn.Close();
            }

            return result;
        }

        public T SelectGetLookupObject<T>(T result, long id)
            where T : class, ILookupObject, new()
        {
            var mainTableName = typeof(T).Name;
            var selectLookupObjectSqlQuery = SqlQueryProvider.SelectLookupObjectById(mainTableName, id.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectLookupObjectSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo property = result.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            property.SetValue(result, reader.GetValue(i), null);
                        }
                    }
                }

                conn.Close();
            }

            return result;
        }

        public T SelectGetLookupObject<T>(T result, Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            var mainTableName = typeof(T).Name;
            var sqlClause = expressionTranslator.Translate(filter);

            var selectLookupObjectSqlQuery = SqlQueryProvider.SelectLookupObjectByClause(mainTableName, sqlClause);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectLookupObjectSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo property = result.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            property.SetValue(result, reader.GetValue(i), null);
                        }
                    }
                }

                conn.Close();
            }

            return result;
        }

        public List<T> SelectGetLookupObjects<T>(List<T> result)
            where T : class, ILookupObject, new()
        {
            var mainTableName = typeof(T).Name;
            var selectLookupObjectsSqlQuery = SqlQueryProvider.SelectLookupObjects(mainTableName);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForLookupTableAttribute))).Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectLookupObjectsSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj = (T)Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < propertiesCount; i++)
                    {
                        PropertyInfo property = obj.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            property.SetValue(obj, reader.GetValue(i), null);
                        }
                    }

                    result.Add(obj);
                }

                conn.Close();
            }

            return result;
        }

        public List<T> SelectGetLookupObjects<T>(List<T> result, Expression<Func<T, bool>> filter)
            where T : class, ILookupObject, new()
        {
            var mainTableName = typeof(T).Name;
            var sqlClause = expressionTranslator.Translate(filter);

            var selectLookupObjectsSqlQuery = SqlQueryProvider.SelectLookupObjectsByClause(mainTableName, sqlClause);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForLookupTableAttribute))).Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectLookupObjectsSqlQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    obj = (T)Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < propertiesCount; i++)
                    {
                        PropertyInfo property = obj.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            property.SetValue(obj, reader.GetValue(i), null);
                        }
                    }

                    result.Add(obj);
                }

                conn.Close();
            }

            return result;
        }

        public long SaveCreateBaseObject<T>(T businessObject, long userId)
            where T : class, IBaseObject
        {
            object objectId = 0;
            businessObject.ObjectTypeId = ObjectTypeDeterminator.GetObjectTypeId(businessObject.GetType().Name);
            businessObject.CreatedById = userId;
            businessObject.CreatedDate = DateTime.Now;
            businessObject.ModifiedById = userId;
            businessObject.ModifiedDate = DateTime.Now;

            if (string.IsNullOrEmpty(businessObject.Name))
                businessObject.Name = "none";

            if (string.IsNullOrEmpty(businessObject.Details))
                businessObject.Details = "none";

            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var propsNamesForMainTable = string.Join(", ", propsForMainTable.Select(x => $"[{x.Name}]"));
            var propValuesForMainTableList = new List<string>();
            var propValuesForMainTable = string.Empty;

            foreach (var prop in propsForMainTable)
            {
                if (prop.PropertyType == typeof(string))
                    propValuesForMainTableList.Add($"'{prop.GetValue(businessObject).ToString()}'");
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    var datetimeVal = (DateTime)prop.GetValue(businessObject);
                    propValuesForMainTableList.Add($"'{datetimeVal.ToString("yyyy-MM-dd HH:mm:ss.fff")}'");
                }
                else
                    propValuesForMainTableList.Add($"{prop.GetValue(businessObject).ToString()}");
            }

            propValuesForMainTable = string.Join(", ", propValuesForMainTableList);

            string insertSqlQuery = SqlQueryProvider.InsertBaseObject(propsNamesForMainTable, propValuesForMainTable);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(insertSqlQuery, conn))
            {
                conn.Open();
                objectId = cmd.ExecuteScalar();
                conn.Close();
            }

            if (objectId != null)
            {
                return (int)(decimal)objectId;
            }
            else
            {
                return 0;
            }
        }

        public void SaveCreateBusinessObject<T>(T businessObject, long objectId)
            where T : class, IBaseObject
        {
            var mainTableName = businessObject.GetType().Name;
            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute)));
            var propsNamesForMainTable = string.Join(", ", propsForMainTable.Select(x => $"[{x.Name}]")) + ", [ObjectId]";
            var propValuesForMainTable = string.Join(", ", propsForMainTable
                .Select(x => x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?) ?
            $"'{x.GetValue(businessObject).ToString()}'" : $"{x.GetValue(businessObject).ToString()}")) + $", {objectId}";

            string insertSqlQuery = SqlQueryProvider.InsertBusinessObject(mainTableName, propsNamesForMainTable, propValuesForMainTable);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(insertSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void SaveUpdateBaseObject<T>(T businessObject, long userId)
            where T : class, IBaseObject
        {
            businessObject.ModifiedById = userId;
            businessObject.ModifiedDate = DateTime.Now;

            if (businessObject.ObjectTypeId == null || businessObject.ObjectTypeId == 0)
                businessObject.ObjectTypeId = ObjectTypeDeterminator.GetObjectTypeId(businessObject.GetType().Name);

            if (string.IsNullOrEmpty(businessObject.Name))
                businessObject.Name = "none";

            if (string.IsNullOrEmpty(businessObject.Details))
                businessObject.Details = "none";

            if (businessObject.CreatedById == null || businessObject.CreatedById == 0)
                businessObject.CreatedById = userId;

            if (businessObject.CreatedDate == null)
                businessObject.CreatedDate = DateTime.Now;

            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var updateProps = string.Join(", ", propsForMainTable
                .Select(x => $"[{x.Name}] =" + (x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?) ? $"'{x.GetValue(businessObject).ToString()}'" : $"{x.GetValue(businessObject).ToString()}")));

            string updateSqlQuery = SqlQueryProvider.UpdateBaseObject(updateProps, businessObject.ObjectId.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(updateSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void SaveUpdateBusinessObject<T>(T businessObject)
            where T : class, IBaseObject
        {
            var mainTableName = businessObject.GetType().Name;
            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute)));
            var updateProps = string.Join(", ", propsForMainTable
                .Select(x => $"[{x.Name}] =" + (x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?) ? $"'{x.GetValue(businessObject).ToString()}'" : $"{x.GetValue(businessObject).ToString()}")));
            
            string updateSqlQuery = SqlQueryProvider.UpdateBusinessObject(mainTableName, updateProps, businessObject.ObjectId.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(updateSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DeleteBusinessAndBaseObject<T>(T businessObject)
            where T : class, IBaseObject
        {
            var mainTableName = businessObject.GetType().Name;

            string deleteBusinessObjectSqlQuery = SqlQueryProvider.DeleteBusinessObject(mainTableName, businessObject.ObjectId.ToString());
            string deleteBaseObjectSqlQuery = SqlQueryProvider.DeleteBaseObject(businessObject.ObjectId.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(deleteBusinessObjectSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(deleteBaseObjectSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public bool AnyBusinessObjects<T>(bool result)
        {
            var mainTableName = typeof(T).Name;
            var selectCountBusinessObjectsSqlQuery = SqlQueryProvider.AnyCountBusinessObjects(mainTableName);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectCountBusinessObjectsSqlQuery, conn))
            {
                conn.Open();
                count = cmd.ExecuteScalar();
                conn.Close();
            }

            if (count != null)
            {
                if ((int)count > 0)
                {
                    result = true;
                    return result;
                }
                else
                    return result;
            }

            return result;
        }

        public bool AnyBusinessObjects<T>(bool result, Expression<Func<T, bool>> filter)
        {
            var mainTableName = typeof(T).Name;
            var sqlClause = expressionTranslator.Translate(filter);

            var selectCountBusinessObjectsSqlQuery = SqlQueryProvider.AnyCountBusinessObjects(mainTableName, sqlClause);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectCountBusinessObjectsSqlQuery, conn))
            {
                conn.Open();
                count = cmd.ExecuteScalar();
                conn.Close();
            }

            if (count != null)
            {
                if ((int)count > 0)
                {
                    result = true;
                    return result;
                }
                else
                    return result;
            }

            return result;
        }

        public int CountBusinessObjects<T>(int result)
        {
            var mainTableName = typeof(T).Name;
            var selectCountBusinessObjectsSqlQuery = SqlQueryProvider.AnyCountBusinessObjects(mainTableName);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectCountBusinessObjectsSqlQuery, conn))
            {
                conn.Open();
                count = cmd.ExecuteScalar();
                conn.Close();
            }

            if (count != null)
            {
                if ((int)count > 0)
                {
                    result = (int)count;
                    return result;
                }
                else
                    return result;
            }

            return result;
        }

        public int CountBusinessObjects<T>(int result, Expression<Func<T, bool>> filter)
        {
            var mainTableName = typeof(T).Name;
            var sqlClause = expressionTranslator.Translate(filter);

            var selectCountBusinessObjectsSqlQuery = SqlQueryProvider.AnyCountBusinessObjects(mainTableName, sqlClause);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectCountBusinessObjectsSqlQuery, conn))
            {
                conn.Open();
                count = cmd.ExecuteScalar();
                conn.Close();
            }

            if (count != null)
            {
                if ((int)count > 0)
                {
                    result = (int)count;
                    return result;
                }
                else
                    return result;
            }

            return result;
        }
    }
}
