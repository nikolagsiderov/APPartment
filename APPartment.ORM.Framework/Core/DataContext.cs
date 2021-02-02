using APPartment.Data.Core;
using APPartment.Data.Models.Core;
using APPartment.Data.Models.Declarations;
using APPartment.ORM.Framework.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APPObject = APPartment.Data.Models.Core.Object;

namespace APPartment.ORM.Framework.Core
{
    public class DataContext<T>
        where T : class, IObject
    {
        private DataAccessContext context;

        public DataContext(DataAccessContext context)
        {
            this.context = context;
        }

        #region Base Operations
        public async Task SaveAsync(T objectModel, long? userId, long? homeId, long? targetObjectId)
        {
            var objectTypeName = objectModel.GetType().Name;
            var objectTypeId = context.Set<ObjectType>().Where(x => x.Name == objectTypeName).FirstOrDefault().Id;

            var _object = new APPObject()
            {
                CreatedById = (long)userId,
                ModifiedById = (long)userId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ObjectTypeId = objectTypeId
            };

            await context.AddAsync(_object);
            await context.SaveChangesAsync();

            objectModel.ObjectId = _object.ObjectId;

            await this.SaveChangesAsync(true, (long)homeId, (long)userId, targetObjectId, _object.ObjectId, objectModel, ContextExecutionTypes.Create);
        }

        public void Save(T objectModel, long? userId, long? homeId, long? targetObjectId)
        {
            var objectTypeName = objectModel.GetType().Name;
            var objectTypeId = context.Set<ObjectType>().Where(x => x.Name == objectTypeName).FirstOrDefault().Id;

            var _object = new APPObject()
            {
                CreatedById = (long)userId,
                ModifiedById = (long)userId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ObjectTypeId = objectTypeId
            };

            context.Add(_object);
            context.SaveChanges();

            objectModel.ObjectId = _object.ObjectId;

            Task.Run(async () => await this.SaveChangesAsync(true, (long)homeId, (long)userId, targetObjectId, _object.ObjectId, objectModel, ContextExecutionTypes.Create)).Wait();
        }

        public async Task UpdateAsync(T objectModel, long? userId, long? homeId, long? targetObjectId)
        {
            var _object = context.Set<APPObject>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            _object.ModifiedById = (long)userId;
            _object.ModifiedDate = DateTime.Now;

            context.Update(_object);
            await context.SaveChangesAsync();

            await this.SaveChangesAsync(true, (long)homeId, (long)userId, targetObjectId, _object.ObjectId, objectModel, ContextExecutionTypes.Update);
        }

        public void Update(T objectModel, long? userId, long? homeId, long? targetObjectId)
        {
            var _object = context.Set<APPObject>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            _object.ModifiedById = (long)userId;
            _object.ModifiedDate = DateTime.Now;

            context.Update(_object);
            context.SaveChanges();

            Task.Run(async () => await this.SaveChangesAsync(true, (long)homeId, (long)userId, targetObjectId, _object.ObjectId, objectModel, ContextExecutionTypes.Update)).Wait();
        }

        public async Task DeleteAsync(T objectModel, long? userId, long? homeId, long? targetObjectId)
        {
            var _object = context.Set<APPObject>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            context.Remove(_object);

            await this.SaveChangesAsync(true, (long)homeId, (long)userId, targetObjectId, 0, objectModel, ContextExecutionTypes.Delete);
        }

        public void Delete(T objectModel, long? userId, long? homeId, long? targetObjectId)
        {
            var _object = context.Set<APPObject>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            context.Remove(_object);

            Task.Run(async () => await this.SaveChangesAsync(true, (long)homeId, (long)userId, targetObjectId, 0, objectModel, ContextExecutionTypes.Delete)).Wait();
        }
        #endregion

        #region Audit Operations
        private async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, long homeId, long userId, long? targetObjectId, long objectId, T objectModel,
            ContextExecutionTypes contextExecutionType, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (contextExecutionType)
            {
                case ContextExecutionTypes.Create:
                    context.Add(objectModel);
                    break;
                case ContextExecutionTypes.Update:
                    context.Update(objectModel);
                    break;
                case ContextExecutionTypes.Delete:
                    context.Remove(objectModel);
                    break;
            }

            var auditEntries = OnBeforeSaveChanges(homeId, userId, targetObjectId, objectId);
            var result = await context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSaveChanges(auditEntries);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges(long homeId, long userId, long? targetObjectId, long objectId)
        {
            context.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Metadata.GetTableName();
                auditEntry.HomeId = homeId;
                auditEntry.UserId = userId;
                auditEntry.TargetObjectId = targetObjectId;
                auditEntry.ObjectId = objectId;
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                var latestAuditValues = context.Audits.OrderByDescending(x => x.Id).Where(x => x.ObjectId == objectId).FirstOrDefault().NewValues;
                                var auditProperties = JsonConvert.DeserializeObject<Dictionary<string, string>>(latestAuditValues);

                                auditEntry.OldValues[propertyName] = auditProperties[propertyName];
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                context.Audits.Add(auditEntry.ToAudit());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                context.Audits.Add(auditEntry.ToAudit());
            }

            return context.SaveChangesAsync();
        }
        #endregion
    }
}
