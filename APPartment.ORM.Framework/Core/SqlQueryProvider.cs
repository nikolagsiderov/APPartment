namespace APPartment.ORM.Framework.Core
{
    public static class SqlQueryProvider
    {
        public static string SelectBusinessObjectById(string mainTableName, string id)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE [Id] = {1}", mainTableName, id);
        }

        public static string SelectBusinessObjectByClause(string mainTableName, string sqlClause)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE {1}", mainTableName, sqlClause);
        }

        public static string SelectBusinessObjects(string mainTableName)
        {
            return string.Format("SELECT * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId]", mainTableName);
        }

        public static string SelectBusinessObjectsByClause(string mainTableName, string sqlClause)
        {
            return string.Format("SELECT * FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE {1}", mainTableName, sqlClause);
        }

        public static string SelectLookupObjectById(string mainTableName, string id)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] WHERE [Id] = {1}", mainTableName, id);
        }

        public static string SelectLookupObjectByClause(string mainTableName, string sqlClause)
        {
            return string.Format("SELECT TOP(1) * FROM [dbo].[{0}] WHERE {1}", mainTableName, sqlClause);
        }

        public static string SelectLookupObjects(string mainTableName)
        {
            return string.Format("SELECT * FROM [dbo].[{0}]", mainTableName);
        }

        public static string SelectLookupObjectsByClause(string mainTableName, string sqlClause)
        {
            return string.Format("SELECT * FROM [dbo].[{0}] WHERE {1}", mainTableName, sqlClause);
        }

        public static string InsertBaseObject(string propsNamesForMainTable, string propValuesForMainTable)
        {
            return string.Format("INSERT INTO [dbo].[Object] ({0}) VALUES ({1})" + "SELECT SCOPE_IDENTITY()", propsNamesForMainTable, propValuesForMainTable);
        }

        public static string InsertBusinessObject(string mainTableName, string propsNamesForMainTable, string propValuesForMainTable)
        {
            return string.Format("INSERT INTO [dbo].[{0}] ({1}) VALUES ({2})", mainTableName, propsNamesForMainTable, propValuesForMainTable);
        }

        public static string UpdateBaseObject(string updateProps, string objectId)
        {
            return string.Format("UPDATE [dbo].[Object] SET {0} WHERE ObjectId = {1}", updateProps, objectId);
        }

        public static string UpdateBusinessObject(string mainTableName, string updateProps, string objectId)
        {
            return string.Format("UPDATE [dbo].[{0}] SET {1} WHERE ObjectId = {2}", mainTableName, updateProps, objectId);
        }

        public static string DeleteBusinessObject(string mainTableName, string objectId)
        {
            return string.Format("DELETE FROM [dbo].[{0}] WHERE ObjectId = {1}", mainTableName, objectId);
        }

        public static string DeleteBaseObject(string objectId)
        {
            return string.Format("DELETE FROM [dbo].[Object] WHERE ObjectId = {0}", objectId);
        }

        public static string AnyCountBusinessObjects(string mainTableName)
        {
            return string.Format("SELECT COUNT(*) FROM [dbo].[{0}]", mainTableName);
        }

        public static string AnyCountBusinessObjects(string mainTableName, string sqlClause)
        {
            return string.Format("SELECT COUNT(*) FROM [dbo].[{0}] LEFT JOIN [dbo].[Object] ON [dbo].[{0}].[ObjectId] = [dbo].[Object].[ObjectId] WHERE {1}", mainTableName, sqlClause);
        }
    }
}
