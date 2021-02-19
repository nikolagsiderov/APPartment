using APPartment.Data.Core;
using APPartment.Data.Server.Declarations;
using System;
using System.Linq;
using APPartment.Data.Attributes;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using APPartment.ORM.Framework.Helpers;
using System.Reflection;
using System.Collections.Generic;

namespace APPartment.ORM.Framework.Core
{
    // TODO: More work here needs to be done...
    public class DaoContext
    {
        public DaoContext()
        {
        }

        #region CRUD Operations
        public T GetObject<T>(long id)
            where T : class, IIdentityBaseObject, new()
        {
            var result = new T();
            result = SelectGetObject<T>(result, id);
            return result;
        }

        public T GetObject<T>(Expression<Func<T, bool>> filter)
            where T : class, IIdentityBaseObject, new()
        {
            var result = new T();
            result = SelectFilterGetObject<T>(result, filter);
            return result;
        }

        // TODO: Finish this here...
        public List<T> GetObjects<T>()
            where T : class, IIdentityBaseObject, new()
        {
            return new List<T>();
        }

        // TODO: Finish this here...
        public List<T> GetObjects<T>(Expression<Func<T, bool>> filter)
            where T : class, IIdentityBaseObject, new()
        {
            return new List<T>();
        }

        public void Create<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            var objectId = SaveCreateBaseObject(businessObject);
            SaveCreateBusinessObject(businessObject, objectId);
        }

        public void Update<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            SaveUpdateBaseObject(businessObject);
            SaveUpdateBusinessObject(businessObject);
        }

        public void Delete<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            DeleteBusinessAndBaseObject(businessObject);
        }
        #endregion

        #region SQL
        private T SelectGetObject<T>(T result, long id)
            where T : class, IIdentityBaseObject, new()
        {
            var mainTableName = typeof(T).Name;
            var selectBusinessObjectSqlQuery = $"SELECT * FROM [dbo].[{mainTableName}] WHERE [Id] = '{id}'";

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

        private T SelectFilterGetObject<T>(T result, Expression<Func<T, bool>> filter)
            where T : class, IIdentityBaseObject, new()
        {
            var mainTableName = typeof(T).Name;
            var expressionTranslator = new ExpressionToSqlHelper();
            var sqlWhereClause = expressionTranslator.Translate(filter);

            var selectBusinessObjectSqlQuery = $"SELECT TOP(1) * FROM [dbo].[{mainTableName}] LEFT JOIN [dbo].[Object] ON [dbo].[{mainTableName}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE {sqlWhereClause}";

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

        private long SaveCreateBaseObject<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            object objectId = 0;
            businessObject.ObjectTypeId = ObjectTypeDeterminator.GetObjectTypeIdByName(businessObject.GetType().Name);
            businessObject.CreatedById = 0;
            businessObject.CreatedDate = DateTime.Now;
            businessObject.ModifiedById = 0;
            businessObject.ModifiedDate = DateTime.Now;

            if (string.IsNullOrEmpty(businessObject.Name))
            {
                businessObject.Name = "none";
            }

            if (string.IsNullOrEmpty(businessObject.Details))
            {
                businessObject.Details = "none";
            }

            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var propsNamesForMainTable = string.Join(", ", propsForMainTable.Select(x => $"[{x.Name}]"));
            var propValuesForMainTable = string.Join(", ", propsForMainTable
                .Select(x => x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?) ?
            $"'{x.GetValue(businessObject).ToString()}'" : $"{x.GetValue(businessObject).ToString()}"));

            string insertSqlQuery = $"INSERT INTO [dbo].[Object] ({propsNamesForMainTable}) VALUES ({propValuesForMainTable})" + "SELECT SCOPE_IDENTITY()";

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

        private void SaveCreateBusinessObject<T>(T businessObject, long objectId)
            where T : class, IIdentityBaseObject
        {
            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute)));
            var propsNamesForMainTable = string.Join(", ", propsForMainTable.Select(x => $"[{x.Name}]")) + ", [ObjectId]";
            var propValuesForMainTable = string.Join(", ", propsForMainTable
                .Select(x => x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?) ?
            $"'{x.GetValue(businessObject).ToString()}'" : $"{x.GetValue(businessObject).ToString()}")) + $", {objectId}";
            var mainTableName = businessObject.GetType().Name;

            string insertSqlQuery = $"INSERT INTO [dbo].[{mainTableName}] ({propsNamesForMainTable}) VALUES ({propValuesForMainTable})";

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(insertSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void SaveUpdateBaseObject<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            businessObject.ModifiedById = 0;
            businessObject.ModifiedDate = DateTime.Now;

            if (businessObject.ObjectTypeId == null || businessObject.ObjectTypeId == 0)
            {
                businessObject.ObjectTypeId = ObjectTypeDeterminator.GetObjectTypeIdByName(businessObject.GetType().Name);
            }

            if (string.IsNullOrEmpty(businessObject.Name))
            {
                businessObject.Name = "none";
            }

            if (string.IsNullOrEmpty(businessObject.Details))
            {
                businessObject.Details = "none";
            }

            if (businessObject.CreatedById == null || businessObject.CreatedById == 0)
            {
                businessObject.CreatedById = 0;
            }

            if (businessObject.CreatedDate == null)
            {
                businessObject.CreatedDate = DateTime.Now;
            }

            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForObjectTableAttribute)));
            var updateProps = string.Join(", ", propsForMainTable
                .Select(x => $"[{x.Name}] =" + (x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?) ? $"'{x.GetValue(businessObject).ToString()}'" : $"{x.GetValue(businessObject).ToString()}")));

            string updateSqlQuery = $"UPDATE [dbo].[Object] SET {updateProps} WHERE ObjectId = {businessObject.ObjectId}";

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(updateSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void SaveUpdateBusinessObject<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            var propsForMainTable = businessObject.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(FieldMappingForMainTableAttribute)));
            var updateProps = string.Join(", ", propsForMainTable
                .Select(x => $"[{x.Name}] =" + (x.PropertyType == typeof(string) || x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?) ? $"'{x.GetValue(businessObject).ToString()}'" : $"{x.GetValue(businessObject).ToString()}")));
            
            var mainTableName = businessObject.GetType().Name;

            string updateSqlQuery = $"UPDATE [dbo].[{mainTableName}] SET {updateProps} WHERE ObjectId = {businessObject.ObjectId}";

            using (SqlConnection conn = new SqlConnection(Configuration.DefaultConnectionString))
            using (SqlCommand cmd = new SqlCommand(updateSqlQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void DeleteBusinessAndBaseObject<T>(T businessObject)
            where T : class, IIdentityBaseObject
        {
            var mainTableName = businessObject.GetType().Name;

            string deleteBusinessObjectSqlQuery = $"DELETE FROM [dbo].[{mainTableName}] WHERE ObjectId = {businessObject.ObjectId}";
            string deleteBaseObjectSqlQuery = $"DELETE FROM [dbo].[Object] WHERE ObjectId = {businessObject.ObjectId}";

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
        #endregion
    }
}
