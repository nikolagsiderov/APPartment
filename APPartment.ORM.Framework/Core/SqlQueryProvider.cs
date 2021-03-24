using APPartment.ORM.Framework.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace APPartment.ORM.Framework.Core
{
    public static class SqlQueryProvider
    {
        public static string SelectBusinessObjectById(string table, string id)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE [Id] = {1}", table, id);
        }

        public static string SelectBusinessObjectByObjectId(string table, string objectId)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE [dbo].[{0}].[ObjectId] = {1}", table, objectId);
        }

        public static string SelectBusinessObjectByClause(string table, string sqlClause)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE {1}", table, sqlClause);
        }

        public static string SelectBusinessObjects(string table)
        {
            return string.Format("SELECT * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId]", table);
        }

        public static string SelectBusinessObjectsByClause(string table, string sqlClause)
        {
            return string.Format("SELECT * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE {1}", table, sqlClause);
        }

        public static string SelectLookupObjectById(string table, string id)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] WHERE [Id] = {1}", table, id);
        }

        public static string SelectLookupObjectByClause(string table, string sqlClause)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] WHERE {1}", table, sqlClause);
        }

        public static string SelectLookupObjects(string table)
        {
            return string.Format("SELECT * FROM [dbo].[{0}]", table);
        }

        public static string SelectLookupObjectsByClause(string table, string sqlClause)
        {
            return string.Format("SELECT * FROM [dbo].[{0}] WHERE {1}", table, sqlClause);
        }

        public static string InsertBaseObject(string propertyNames, string propertyValues)
        {
            return string.Format("INSERT INTO [dbo].[Object] ({0}) VALUES ({1})" + "SELECT SCOPE_IDENTITY()", propertyNames, propertyValues);
        }

        public static string InsertBusinessObject(string table, string propertyNames, string propertyValues)
        {
            return string.Format("INSERT INTO [dbo].[{0}] ({1}) VALUES ({2})", table, propertyNames, propertyValues);
        }

        public static string UpdateBaseObject(string properties, string objectId)
        {
            return string.Format("UPDATE [dbo].[Object] SET {0} WHERE ObjectId = {1}", properties, objectId);
        }

        public static string UpdateBusinessObject(string table, string properties, string objectId)
        {
            return string.Format("UPDATE [dbo].[{0}] SET {1} WHERE ObjectId = {2}", table, properties, objectId);
        }

        public static string DeleteBusinessObject(string table, string objectId)
        {
            return string.Format("DELETE FROM [dbo].[{0}] WHERE ObjectId = {1}", table, objectId);
        }

        public static string DeleteBaseObject(string objectId)
        {
            return string.Format("DELETE FROM [dbo].[Object] WHERE ObjectId = {0}", objectId);
        }

        public static string AnyCountBusinessObjects(string table)
        {
            return string.Format("SELECT COUNT(*) FROM [dbo].[{0}]", table);
        }

        public static string AnyCountBusinessObjects(string table, string sqlClause)
        {
            return string.Format("SELECT COUNT(*) FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE {1}", table, sqlClause);
        }

        public static string GetPropertyNamesForDBColumns(IEnumerable<PropertyInfo> properties)
        {
            return string.Join(", ", properties.Select(x => $"[{x.Name}]"));
        }

        public static string GetPropertyValues<T>(IEnumerable<PropertyInfo> properties, T businessObject)
            where T : class, IBaseObject
        {
            var propertyValuesList = new List<string>();
            var propertyValues = string.Empty;
            var nullValue = "NULL";

            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyValuesList.Add(nullValue);
                    else
                        propertyValuesList.Add($"'{value.ToString()}'");

                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    var datetimeValue = (DateTime)prop.GetValue(businessObject);
                    propertyValuesList.Add($"'{datetimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff")}'");
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyValuesList.Add(nullValue);
                    else
                    {
                        var datetimeValue = (DateTime)value;
                        propertyValuesList.Add($"'{datetimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff")}'");
                    }
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    var boolValue = (bool)prop.GetValue(businessObject);

                    // Converting the value to integer is neccessary for DB to handle
                    // For instance, value 'true' is converted to '1'
                    var boolToIntValue = Convert.ToInt32(boolValue);
                    propertyValuesList.Add($"{boolToIntValue.ToString()}");
                }
                else if (prop.PropertyType == typeof(bool?))
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyValuesList.Add(nullValue);
                    else
                    {
                        var boolValue = (bool)value;

                        // Converting the value to integer is neccessary for DB to handle
                        // For instance, value 'true' is converted to '1'
                        var boolToIntValue = Convert.ToInt32(boolValue);
                        propertyValuesList.Add($"{boolToIntValue.ToString()}");
                    }
                }
                else
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyValuesList.Add(nullValue);
                    else
                        propertyValuesList.Add($"{prop.GetValue(businessObject).ToString()}");
                }
            }

            propertyValues = string.Join(", ", propertyValuesList);
            return propertyValues;
        }

        public static string GetPropertyNamesForDBColumnsAndValuesForUpdate<T>(IEnumerable<PropertyInfo> properties, T businessObject)
            where T : class, IBaseObject
        {
            var propertyNamesAndValuesList = new List<string>();
            var propertyNamesAndValues = string.Empty;
            var nullValue = "NULL";

            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = " + nullValue);
                    else
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = '{value.ToString()}'");

                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    var datetimeValue = (DateTime)prop.GetValue(businessObject);
                    propertyNamesAndValuesList.Add($"[{prop.Name}] = '{datetimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff")}'");
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = " + nullValue);
                    else
                    {
                        var datetimeValue = (DateTime)value;
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = '{datetimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff")}'");
                    }
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    var boolValue = (bool)prop.GetValue(businessObject);

                    // Converting the value to integer is neccessary for DB to handle
                    // For instance, value 'true' is converted to '1'
                    var boolToIntValue = Convert.ToInt32(boolValue);
                    propertyNamesAndValuesList.Add($"[{prop.Name}] = {boolToIntValue.ToString()}");
                }
                else if (prop.PropertyType == typeof(bool?))
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = " + nullValue);
                    else
                    {
                        var boolValue = (bool)value;

                        // Converting the value to integer is neccessary for DB to handle
                        // For instance, value 'true' is converted to '1'
                        var boolToIntValue = Convert.ToInt32(boolValue);
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = {boolToIntValue.ToString()}");
                    }
                }
                else
                {
                    var value = prop.GetValue(businessObject);

                    if (value == null)
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = " + nullValue);
                    else
                        propertyNamesAndValuesList.Add($"[{prop.Name}] = {prop.GetValue(businessObject).ToString()}");
                }
            }

            propertyNamesAndValues = string.Join(", ", propertyNamesAndValuesList);
            return propertyNamesAndValues;
        }
    }
}
