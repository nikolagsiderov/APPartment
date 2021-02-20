using System;
using System.Linq;
using System.Linq.Expressions;
using APPartment.ORM.Framework.Helpers;
using System.Reflection;
using APPartment.ORM.Framework.Declarations;
using System.Data.SqlClient;
using APPartment.ORM.Framework.Attributes;

namespace APPartment.ORM.Framework.Core
{
    public class DaoContext
    {
        public DaoContext()
        {
        }

        public T SelectGetObject<T>(T result, long id)
            where T : class, IIdentityBaseObject, new()
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
            where T : class, IIdentityBaseObject, new()
        {
            var mainTableName = typeof(T).Name;
            var expressionTranslator = new ExpressionToSqlHelper();
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

        public long SaveCreateBaseObject<T>(T businessObject)
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
            where T : class, IIdentityBaseObject
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

        public void SaveUpdateBaseObject<T>(T businessObject)
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
            where T : class, IIdentityBaseObject
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
            where T : class, IIdentityBaseObject
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
    }
}
