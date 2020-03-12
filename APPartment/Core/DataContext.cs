﻿using APPartment.Data;
using APPartment.Enums;
using APPartment.Models;
using APPartment.Models.Declaration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APPartment.Core
{
    public class DataContext<T>
        where T : class, IObject
    {
        public async Task SaveAsync(T objectModel, DataAccessContext context, long userId, long? targetObjectId)
        {
            var objectTypeName = objectModel.GetType().Name;
            var objectTypeId = context.Set<ObjectType>().Where(x => x.Name == objectTypeName).FirstOrDefault().Id;

            var _object = new Models.Object()
            {
                CreatedById = userId,
                ModifiedById = userId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ObjectTypeId = objectTypeId
            };

            await context.AddAsync(_object);
            await context.SaveChangesAsync();

            PopulateHistory((int)HistoryFunctionTypes.Create, _object, context, userId, targetObjectId);

            objectModel.ObjectId = _object.ObjectId;

            await context.AddAsync<T>(objectModel);
            await context.SaveChangesAsync();
        }

        public void Save(T objectModel, DataAccessContext context, long userId, long? targetObjectId)
        {
            var objectTypeName = objectModel.GetType().Name;
            var objectTypeId = context.Set<ObjectType>().Where(x => x.Name == objectTypeName).FirstOrDefault().Id;

            var _object = new Models.Object()
            {
                CreatedById = userId,
                ModifiedById = userId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ObjectTypeId = objectTypeId
            };

            context.Add(_object);
            context.SaveChanges();

            PopulateHistory((int)HistoryFunctionTypes.Create, _object, context, userId, targetObjectId);

            objectModel.ObjectId = _object.ObjectId;

            context.Add<T>(objectModel);
            context.SaveChanges();
        }

        public async Task UpdateAsync(T objectModel, DataAccessContext context, long userId)
        {
            var _object = context.Set<Models.Object>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            _object.ModifiedById = userId;
            _object.ModifiedDate = DateTime.Now;

            context.Update(_object);
            await context.SaveChangesAsync();

            PopulateHistory((int)HistoryFunctionTypes.Update, _object, context, userId, null);

            context.Update(objectModel);
            await context.SaveChangesAsync();
        }

        public void Update(T objectModel, DataAccessContext context, long userId)
        {
            var _object = context.Set<Models.Object>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            _object.ModifiedById = userId;
            _object.ModifiedDate = DateTime.Now;

            context.Update(_object);
            context.SaveChanges();

            PopulateHistory((int)HistoryFunctionTypes.Update, _object, context, userId, null);

            context.Update(objectModel);
            context.SaveChanges();
        }

        public async Task DeleteAsync(T objectModel, DataAccessContext context, long userId, long? targetObjectId)
        {
            var _object = context.Set<Models.Object>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            PopulateHistory((int)HistoryFunctionTypes.Delete, _object, context, userId, targetObjectId);

            context.Remove(_object);
            context.Remove(objectModel);

            await context.SaveChangesAsync();
        }

        public void Delete(T objectModel, DataAccessContext context, long userId, long? targetObjectId)
        {
            var _object = context.Set<Models.Object>().Where(x => x.ObjectId == objectModel.ObjectId).FirstOrDefault();

            PopulateHistory((int)HistoryFunctionTypes.Delete, _object, context, userId, targetObjectId);

            context.Remove(_object);
            context.Remove(objectModel);

            context.SaveChanges();
        }

        #region History
        private void PopulateHistory(int historyFunctionType, Models.Object @object, DataAccessContext context, long userId, long? targetObjectId)
        {
            if (historyFunctionType == (int)HistoryFunctionTypes.Create)
            {
                var isSubObject = DetermineIfCurrentObjectIsSubObject(@object);

                if (isSubObject)
                {
                    var history = new History()
                    {
                        FunctionTypeId = historyFunctionType,
                        UserId = userId,
                        TargetId = targetObjectId,
                        ObjectId = @object.ObjectId
                    };

                    this.SaveHistory(history, context, userId);
                }
                else
                {
                    var history = new History()
                    {
                        FunctionTypeId = historyFunctionType,
                        UserId = userId,
                        ObjectId = @object.ObjectId
                    };

                    this.SaveHistory(history, context, userId);
                }
            }
            else if (historyFunctionType == (int)HistoryFunctionTypes.Update)
            {

            }
            else if (historyFunctionType == (int)HistoryFunctionTypes.Delete)
            {
                var isSubObject = DetermineIfCurrentObjectIsSubObject(@object);

                if (isSubObject)
                {

                }
                else
                {
                    DeleteObjectMetadataSubObjects(@object, context, userId);

                    // TODO: Determine how we will save info about deleted object, how to access it later to display?
                }
            }
        }

        private void DeleteObjectMetadataSubObjects(Models.Object @object, DataAccessContext context, long userId)
        {
            if (@object.ObjectTypeId == (long)ObjectTypes.House)
            {
                var currentHouseId = context.Set<House>().Where(x => x.ObjectId == @object.ObjectId).FirstOrDefault().Id;

                var hasAnyHouseStatuses = context.HouseStatuses.Any(x => x.HouseId == currentHouseId);
                var hasAnyHouseSettings = context.HouseSettings.Any(x => x.HouseId == currentHouseId);

                if (hasAnyHouseStatuses)
                {
                    var houseStatuses = context.HouseStatuses.Where(x => x.HouseId == currentHouseId);

                    foreach (var houseStatus in houseStatuses)
                    {
                        context.Remove(houseStatus);
                    }
                }

                if (hasAnyHouseSettings)
                {
                    var houseSettings = context.HouseSettings.Where(x => x.HouseId == currentHouseId);

                    foreach (var houseSetting in houseSettings)
                    {
                        context.Remove(houseSetting);
                    }
                }
            }
            else if (@object.ObjectTypeId == (long)ObjectTypes.Invetory || @object.ObjectTypeId == (long)ObjectTypes.Hygiene ||
                @object.ObjectTypeId == (long)ObjectTypes.Issue)
            {
                var hasAnyComments = context.Comments.Any(x => x.TargetId == @object.ObjectId);
                var hasAnyImages = context.Images.Any(x => x.TargetId == @object.ObjectId);

                if (hasAnyComments)
                {
                    var comments = context.Comments.Where(x => x.TargetId == @object.ObjectId);

                    foreach (var comment in comments)
                    {
                        context.Remove(comment);
                    }
                }

                if (hasAnyImages)
                {
                    var images = context.Images.Where(x => x.TargetId == @object.ObjectId);

                    foreach (var image in images)
                    {
                        context.Remove(image);
                    }
                }
            }
        }

        private bool DetermineIfCurrentObjectIsSubObject(Models.Object @object)
        {
            var result = false;

            result = @object.ObjectTypeId switch
            {
                3 => true,
                4 => true,
                9 => true,
                10 => true,
                11 => true,
                _ => false,
            };

            return result;
        }

        public void SaveHistory(History objectModel, DataAccessContext context, long userId)
        {
            var objectTypeName = objectModel.GetType().Name;
            var objectTypeId = context.Set<ObjectType>().Where(x => x.Name == objectTypeName).FirstOrDefault().Id;

            var _object = new Models.Object()
            {
                CreatedById = userId,
                ModifiedById = userId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ObjectTypeId = objectTypeId
            };

            context.Add(_object);
            context.SaveChanges();

            context.Add<History>(objectModel);
            context.SaveChanges();
        }
        #endregion
    }
}