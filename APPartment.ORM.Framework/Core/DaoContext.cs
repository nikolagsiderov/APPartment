using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using APPartment.ORM.Framework.Helpers;
using APPartment.ORM.Framework.Declarations;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

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
            var table = GetTableName<T>();
            var query = SqlQueryProvider.SelectBusinessObjectById(table, id.ToString());

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
            var sqlClause = expressionTranslator.Translate(filter);
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

            if (result.ObjectId == 0)
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
            var sqlClause = expressionTranslator.Translate(filter);
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

        public T SelectGetLookupObject<T>(T result, long id)
            where T : class, ILookupObject, new()
        {
            var table = GetLookupTableName<T>();
            var query = SqlQueryProvider.SelectLookupObjectById(table, id.ToString());

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
            var sqlClause = expressionTranslator.Translate(filter);
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
            var sqlClause = expressionTranslator.Translate(filter);
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

            var properties = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var propertyColumnNames = SqlQueryProvider.GetPropertyNamesForDBColumns(properties);
            var propertyValues = SqlQueryProvider.GetPropertyValues<T>(properties, businessObject);
            string query = SqlQueryProvider.InsertBaseObject(propertyColumnNames, propertyValues);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                objectId = cmd.ExecuteScalar();
                conn.Close();
            }

            if (objectId != null)
                return (int)(decimal)objectId;
            else
                return 0;
        }

        public T SaveCreateBusinessObject<T>(T businessObject, long objectId)
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            var properties = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute)));
            var propertyColumnNames = SqlQueryProvider.GetPropertyNamesForDBColumns(properties) + ", [ObjectId]";
            var propertyValues = SqlQueryProvider.GetPropertyValues<T>(properties, businessObject) + $", {objectId}";
            var query = SqlQueryProvider.InsertBusinessObject(table, propertyColumnNames, propertyValues);

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return SelectGetBusinessObjectAfterSave<T>(businessObject, table, objectId);
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

            var properties = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var propertyNamesAndValues = SqlQueryProvider.GetPropertyNamesForDBColumnsAndValuesForUpdate<T>(properties, businessObject);
            string query = SqlQueryProvider.UpdateBaseObject(propertyNamesAndValues, businessObject.ObjectId.ToString());

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
            var propertyNamesAndValues = SqlQueryProvider.GetPropertyNamesForDBColumnsAndValuesForUpdate<T>(properties, businessObject);
            string query = SqlQueryProvider.UpdateBusinessObject(table, propertyNamesAndValues, businessObject.ObjectId.ToString());

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return SelectGetBusinessObjectAfterSave<T>(businessObject, table, businessObject.ObjectId);
        }

        private T SelectGetBusinessObjectAfterSave<T>(T businessObject, string table, long objectId)
            where T : class, IBaseObject
        {
            var selectQuery = SqlQueryProvider.SelectBusinessObjectByObjectId(table, objectId.ToString());

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

        public void DeleteBusinessAndBaseObject<T>(T businessObject)
            where T : class, IBaseObject
        {
            var table = GetTableName<T>();
            string deleteBusinessObjectSqlQuery = SqlQueryProvider.DeleteBusinessObject(table, businessObject.ObjectId.ToString());
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
            var sqlClause = expressionTranslator.Translate(filter);
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
            var sqlClause = expressionTranslator.Translate(filter);
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
    }
}
