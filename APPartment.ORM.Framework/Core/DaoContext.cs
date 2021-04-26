using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using APPartment.ORM.Framework.Tools;
using APPartment.ORM.Framework.Declarations;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using APPartment.Common;
using APPartment.ORM.Framework.Enums;

namespace APPartment.ORM.Framework.Core
{
    public class DaoContext
    {
        private ExpressionToSql expressionToSql;

        public DaoContext()
        {
            expressionToSql = new ExpressionToSql();
        }

        public T SelectGetObject<T>(T result, long ID)
            where T : class, IBaseObject, new()
        {
            var table = GetTableName<T>();
            var query = SqlQueryProvider.SelectBusinessObjectByID(table, ID.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(result, value, null);
                        }
                    }
                }

                conn.Close();
            }

            return result;
        }

        public T SelectGetObject<T>(T result, Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var table = GetTableName<T>();
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.SelectBusinessObjectByClause(table, sqlClause);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(result, value, null);
                        }
                    }
                }

                conn.Close();
            }

            if (result.ObjectID == 0)
                result = null;

            return result;
        }

        public List<T> SelectGetObjects<T>(List<T> result)
            where T : class, IBaseObject, new()
        {
            var table = GetTableName<T>();
            var query = SqlQueryProvider.SelectBusinessObjects(table);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForObjectTablePrimaryKeyAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTablePrimaryKeyAttribute)))
                .Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(obj, value, null);
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
            var table = GetTableName<T>();
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.SelectBusinessObjectsByClause(table, sqlClause);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForObjectTablePrimaryKeyAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForMainTablePrimaryKeyAttribute)))
                .Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(obj, value, null);
                        }
                    }

                    result.Add(obj);
                }

                conn.Close();
            }

            return result;
        }

        public T SelectGetBusinessObject<T>(T result, long objectID)
            where T : class, IBaseObject, new()
        {
            var query = SqlQueryProvider.SelectBusinessObjectByID(objectID.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(result, value, null);
                        }
                    }
                }

                conn.Close();
            }

            return result;
        }

        public T SelectGetBusinessObject<T>(T result, Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.SelectBusinessObjectByClause(sqlClause);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(result, value, null);
                        }
                    }
                }

                conn.Close();
            }

            if (result.ObjectID == 0)
                result = null;

            return result;
        }

        public List<T> SelectGetBusinessObjects<T>(List<T> result, Expression<Func<T, bool>> filter)
            where T : class, IBaseObject, new()
        {
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.SelectBusinessObjectsByClause(sqlClause);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute))
                || Attribute.IsDefined(prop, typeof(FieldMappingForObjectTablePrimaryKeyAttribute)))
                .Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(obj, value, null);
                        }
                    }

                    result.Add(obj);
                }

                conn.Close();
            }

            return result;
        }

        public T SelectGetLookupObject<T>(T result, long ID)
            where T : class, ILookupObject, new()
        {
            var table = GetLookupTableName<T>();
            var query = SqlQueryProvider.SelectLookupObjectByID(table, ID.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(result, value, null);
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
            var table = GetLookupTableName<T>();
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.SelectLookupObjectByClause(table, sqlClause);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(result, value, null);
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
            var table = GetLookupTableName<T>();
            var query = SqlQueryProvider.SelectLookupObjects(table);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForLookupTableAttribute)))
                .Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(obj, value, null);
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
            var table = GetLookupTableName<T>();
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.SelectLookupObjectsByClause(table, sqlClause);
            T obj = null;
            var propertiesCount = typeof(T)
                .GetProperties()
                .Where(prop =>
                Attribute.IsDefined(prop, typeof(FieldMappingForLookupTableAttribute)))
                .Count();

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(obj, value, null);
                        }
                    }

                    result.Add(obj);
                }

                conn.Close();
            }

            return result;
        }

        public long SaveCreateBaseObject<T>(T businessObject, long userID, long homeID)
            where T : class, IBaseObject
        {
            object objectID = 0;
            businessObject.ObjectTypeID = GetObjectTypeID(businessObject.GetType().Name);
            businessObject.HomeID = homeID;
            businessObject.CreatedByID = userID;
            businessObject.CreatedDate = DateTime.Now;
            businessObject.ModifiedByID = userID;
            businessObject.ModifiedDate = DateTime.Now;

            if (string.IsNullOrEmpty(businessObject.Name))
                businessObject.Name = "none";

            if (string.IsNullOrEmpty(businessObject.Details))
                businessObject.Details = "none";

            var properties = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var propertyColumnNames = SqlQueryProvider.GetPropertyNamesForDBColumns(properties);
            var propertyValues = SqlQueryProvider.GetPropertyValues<T>(properties, businessObject);
            string query = SqlQueryProvider.InsertBaseObject(propertyColumnNames, propertyValues);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                objectID = cmd.ExecuteScalar();
                conn.Close();
            }

            if (objectID != null)
                return (int)(decimal)objectID;
            else
                return 0;
        }

        public T SaveCreateBusinessObject<T>(T businessObject, long objectID)
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            var properties = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute)));

            if (properties.Any())
            {
                var propertyColumnNames = SqlQueryProvider.GetPropertyNamesForDBColumns(properties) + ", [ObjectID]";
                var propertyValues = SqlQueryProvider.GetPropertyValues<T>(properties, businessObject) + $", {objectID}";
                var query = SqlQueryProvider.InsertBusinessObject(table, propertyColumnNames, propertyValues);

                using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                var propertyColumnNames = "[ObjectID]";
                var propertyValues = $"{objectID}";
                var query = SqlQueryProvider.InsertBusinessObject(table, propertyColumnNames, propertyValues);

                using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return SelectGetBusinessObjectAfterSave<T>(businessObject, table, objectID);
        }

        public void SaveUpdateBaseObject<T>(T businessObject, long userID, long homeID)
            where T : class, IBaseObject
        {
            businessObject.HomeID = homeID;
            businessObject.ModifiedByID = userID;
            businessObject.ModifiedDate = DateTime.Now;

            if (businessObject.ObjectTypeID == null || businessObject.ObjectTypeID == 0)
                businessObject.ObjectTypeID = GetObjectTypeID(businessObject.GetType().Name);

            if (string.IsNullOrEmpty(businessObject.Name))
                businessObject.Name = "none";

            if (string.IsNullOrEmpty(businessObject.Details))
                businessObject.Details = "none";

            var properties = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var propertyNamesAndValues = SqlQueryProvider.GetPropertyNamesForDBColumnsAndValuesForUpdate<T>(properties, businessObject);
            string query = SqlQueryProvider.UpdateBaseObject(propertyNamesAndValues, businessObject.ObjectID.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public T SaveUpdateBusinessObject<T>(T businessObject)
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            var properties = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute)));

            if (properties.Any())
            {
                var propertyNamesAndValues = SqlQueryProvider.GetPropertyNamesForDBColumnsAndValuesForUpdate<T>(properties, businessObject);
                string query = SqlQueryProvider.UpdateBusinessObject(table, propertyNamesAndValues, businessObject.ObjectID.ToString());

                using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            return SelectGetBusinessObjectAfterSave<T>(businessObject, table, businessObject.ObjectID);
        }

        private T SelectGetBusinessObjectAfterSave<T>(T businessObject, string table, long objectID)
            where T : class, IBaseObject
        {
            var selectQuery = SqlQueryProvider.SelectBusinessObjectByObjectID(table, objectID.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo property = businessObject.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);
                        if (null != property && property.CanWrite)
                        {
                            var value = reader.GetValue(i);

                            if (value == DBNull.Value)
                                continue;

                            property.SetValue(businessObject, value, null);
                        }
                    }
                }

                conn.Close();
            }

            return businessObject;
        }

        public void DeleteBusinessObjectAndAllItsBaseReferences<T>(T businessObject)
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            string deleteBusinessObjectSqlQuery = SqlQueryProvider.DeleteBusinessObjectAndAllItsBaseReferences(table, businessObject.ObjectID.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(deleteBusinessObjectSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public bool AnyBusinessObjects<T>(bool result)
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            var query = SqlQueryProvider.AnyCountBusinessObjects(table);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.AnyCountBusinessObjects(table, sqlClause);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            var query = SqlQueryProvider.AnyCountBusinessObjects(table);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            var sqlClause = expressionToSql.Translate(filter);
            var query = SqlQueryProvider.AnyCountBusinessObjects(table, sqlClause);
            object count = null;

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
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

        private string GetTableName<T>()
            where T : class, IBaseObject
        {
            var result = typeof(T)
                .GetCustomAttributesData()
                .Where(x => x.AttributeType.Equals(typeof(TableAttribute)))
                .FirstOrDefault()
                .ConstructorArguments
                .FirstOrDefault()
                .Value
                .ToString();

            return result;
        }

        private string GetLookupTableName<T>()
            where T : class, ILookupObject
        {
            var result = typeof(T)
                .GetCustomAttributesData()
                .Where(x => x.AttributeType.Equals(typeof(TableAttribute)))
                .FirstOrDefault()
                .ConstructorArguments
                .FirstOrDefault()
                .Value
                .ToString();

            return result;
        }

        public long GetObjectTypeID(string objectTypeName)
        {
            var objectType = (ObjectTypes)Enum.Parse(typeof(ObjectTypes), objectTypeName);
            return (long)objectType;
        }
    }
}
