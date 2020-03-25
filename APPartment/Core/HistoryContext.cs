using APPartment.Data;
using APPartment.Enums;
using APPartment.Models;
using APPartment.Models.Declaration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace APPartment.Core
{
    public class HistoryContext<T>
        where T : class, IObject
    {
        private DataAccessContext context;

        public HistoryContext(DataAccessContext context)
        {
            this.context = context;
        }

        public void PopulateHistory(int historyFunctionType, T objectModel, Models.Object @object, long userId, long? targetObjectId, long houseId)
        {
            var now = DateTime.Now;

            if (historyFunctionType == (int)HistoryFunctionTypes.Create)
            {
                var isSubObject = DetermineIfCurrentObjectIsSubObject(@object);

                if (isSubObject)
                {
                    var history = new History()
                    {
                        FunctionTypeId = historyFunctionType,
                        When = now,
                        HouseId = houseId,
                        UserId = userId,
                        TargetId = targetObjectId,
                        ObjectId = @object.ObjectId
                    };

                    this.SaveHistory(history, userId);
                }
                else
                {
                    if (@object.ObjectTypeId == (int)ObjectTypes.HouseStatus)
                    {
                        var newHouseStatusModel = objectModel as HouseStatus;

                        var history = new History()
                        {
                            FunctionTypeId = historyFunctionType,
                            ColumnName = "Status",
                            NewValue = newHouseStatusModel.Status.ToString(),
                            When = now,
                            HouseId = houseId,
                            UserId = userId,
                            ObjectId = @object.ObjectId
                        };

                        this.SaveHistory(history, userId);
                    }
                    else
                    {
                        var history = new History()
                        {
                            FunctionTypeId = historyFunctionType,
                            When = now,
                            HouseId = houseId,
                            UserId = userId,
                            ObjectId = @object.ObjectId
                        };

                        this.SaveHistory(history, userId);
                    }
                }
            }
            else if (historyFunctionType == (int)HistoryFunctionTypes.Update)
            {
                var history = new History()
                {
                    FunctionTypeId = historyFunctionType,
                    When = now,
                    HouseId = houseId,
                    UserId = userId,
                    ObjectId = @object.ObjectId
                };

                // Parent Objects
                switch (@object.ObjectTypeId)
                {
                    case (long)ObjectTypes.User:
                        var oldUserModel = context.Users.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newuUserModel = objectModel as User;

                        if (oldUserModel.Username != newuUserModel.Username)
                        {
                            history.ColumnName = "Username";
                            history.OldValue = oldUserModel.Username;
                            history.NewValue = newuUserModel.Username;
                        }

                        this.SaveHistory(history, userId);
                        break;
                    case (long)ObjectTypes.House:
                        var oldHouseModel = context.Houses.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newHouseModel = objectModel as House;

                        if (oldHouseModel.Name != newHouseModel.Name)
                        {
                            history.ColumnName = "Name";
                            history.OldValue = oldHouseModel.Name;
                            history.NewValue = newHouseModel.Name;
                        }

                        this.SaveHistory(history, userId);
                        break;
                    case (long)ObjectTypes.Inventory:
                        var oldInventoryModel = context.Inventories.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newInventoryModel = objectModel as Inventory;

                        if (oldInventoryModel.Name != newInventoryModel.Name)
                        {
                            history.ColumnName = "Name";
                            history.OldValue = oldInventoryModel.Name;
                            history.NewValue = newInventoryModel.Name;

                            this.SaveHistory(history, userId);
                        }

                        if (oldInventoryModel.Details != newInventoryModel.Details)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Details";
                            history.OldValue = oldInventoryModel.Details;
                            history.NewValue = newInventoryModel.Details;

                            this.SaveHistory(history, userId);
                        }

                        if (oldInventoryModel.Status != newInventoryModel.Status)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Status";
                            history.OldValue = oldInventoryModel.Status.ToString();
                            history.NewValue = newInventoryModel.Status.ToString();

                            this.SaveHistory(history, userId);
                        }

                        if (oldInventoryModel.IsCompleted != newInventoryModel.IsCompleted)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "IsCompleted";
                            history.OldValue = oldInventoryModel.IsCompleted.ToString();
                            history.NewValue = newInventoryModel.IsCompleted.ToString();

                            this.SaveHistory(history, userId);
                        }
                        break;
                    case (long)ObjectTypes.Hygiene:
                        var oldHygieneModel = context.Hygienes.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newHygieneModel = objectModel as Hygiene;

                        if (oldHygieneModel.Name != newHygieneModel.Name)
                        {
                            history.ColumnName = "Name";
                            history.OldValue = oldHygieneModel.Name;
                            history.NewValue = newHygieneModel.Name;

                            this.SaveHistory(history, userId);
                        }

                        if (oldHygieneModel.Details != newHygieneModel.Details)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Details";
                            history.OldValue = oldHygieneModel.Details;
                            history.NewValue = newHygieneModel.Details;

                            this.SaveHistory(history, userId);
                        }

                        if (oldHygieneModel.Status != newHygieneModel.Status)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Status";
                            history.OldValue = oldHygieneModel.Status.ToString();
                            history.NewValue = newHygieneModel.Status.ToString();

                            this.SaveHistory(history, userId);
                        }

                        if (oldHygieneModel.IsCompleted != newHygieneModel.IsCompleted)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "IsCompleted";
                            history.OldValue = oldHygieneModel.IsCompleted.ToString();
                            history.NewValue = newHygieneModel.IsCompleted.ToString();

                            this.SaveHistory(history, userId);
                        }
                        break;
                    case (long)ObjectTypes.Issue:
                        var oldIssueModel = context.Issues.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newIssueModel = objectModel as Issue;

                        if (oldIssueModel.Name != newIssueModel.Name)
                        {
                            history.ColumnName = "Name";
                            history.OldValue = oldIssueModel.Name;
                            history.NewValue = newIssueModel.Name;

                            this.SaveHistory(history, userId);
                        }

                        if (oldIssueModel.Details != newIssueModel.Details)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Details";
                            history.OldValue = oldIssueModel.Details;
                            history.NewValue = newIssueModel.Details;

                            this.SaveHistory(history, userId);
                        }

                        if (oldIssueModel.Status != newIssueModel.Status)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Status";
                            history.OldValue = oldIssueModel.Status.ToString();
                            history.NewValue = newIssueModel.Status.ToString();

                            this.SaveHistory(history, userId);
                        }

                        if (oldIssueModel.IsCompleted != newIssueModel.IsCompleted)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "IsCompleted";
                            history.OldValue = oldIssueModel.IsCompleted.ToString();
                            history.NewValue = newIssueModel.IsCompleted.ToString();

                            this.SaveHistory(history, userId);
                        }
                        break;

                    case (long)ObjectTypes.Survey:
                        var oldSurveyModel = context.Surveys.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newSurveyModel = objectModel as Survey;

                        if (oldSurveyModel.Name != newSurveyModel.Name)
                        {
                            history.ColumnName = "Name";
                            history.OldValue = oldSurveyModel.Name;
                            history.NewValue = newSurveyModel.Name;

                            this.SaveHistory(history, userId);
                        }

                        if (oldSurveyModel.Details != newSurveyModel.Details)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Details";
                            history.OldValue = oldSurveyModel.Details;
                            history.NewValue = newSurveyModel.Details;

                            this.SaveHistory(history, userId);
                        }

                        if (oldSurveyModel.Status != newSurveyModel.Status)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "Status";
                            history.OldValue = oldSurveyModel.Status.ToString();
                            history.NewValue = newSurveyModel.Status.ToString();

                            this.SaveHistory(history, userId);
                        }

                        if (oldSurveyModel.IsCompleted != newSurveyModel.IsCompleted)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "IsCompleted";
                            history.OldValue = oldSurveyModel.IsCompleted.ToString();
                            history.NewValue = newSurveyModel.IsCompleted.ToString();

                            this.SaveHistory(history, userId);
                        }
                        break;
                }

                // Child objects - Metadata
                switch (@object.ObjectTypeId)
                {
                    case (long)ObjectTypes.HouseStatus:
                        var oldHouseStatusModel = context.HouseStatuses.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newHouseStatusModel = objectModel as HouseStatus;

                        if (oldHouseStatusModel.Status != newHouseStatusModel.Status)
                        {
                            history.ColumnName = "Status";
                            history.OldValue = oldHouseStatusModel.Status.ToString();
                            history.NewValue = newHouseStatusModel.Status.ToString();

                            this.SaveHistory(history, userId);
                        }
                        break;
                    case (long)ObjectTypes.HouseSettings:
                        var oldHouseSettingsModel = context.HouseSettings.AsNoTracking().Single(x => x.ObjectId == @object.ObjectId);
                        var newHouseSettingsModel = objectModel as HouseSettings;

                        if (oldHouseSettingsModel.RentDueDateDay != newHouseSettingsModel.RentDueDateDay)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "RentDueDateDay";
                            history.OldValue = oldHouseSettingsModel.RentDueDateDay.ToString();
                            history.NewValue = newHouseSettingsModel.RentDueDateDay.ToString();

                            this.SaveHistory(history, userId);
                        }

                        if (oldHouseSettingsModel.HouseName != newHouseSettingsModel.HouseName)
                        {
                            history = new History()
                            {
                                FunctionTypeId = historyFunctionType,
                                When = now,
                                HouseId = houseId,
                                UserId = userId,
                                ObjectId = @object.ObjectId
                            };

                            history.ColumnName = "HouseName";
                            history.OldValue = oldHouseSettingsModel.HouseName;
                            history.NewValue = newHouseSettingsModel.HouseName;

                            this.SaveHistory(history, userId);
                        }
                        break;
                    case (long)ObjectTypes.Comment:
                        // TODO: Implement this case, when comments become editable
                        break;
                    case (long)ObjectTypes.Image:
                        // Will we event implement a case where images are going to be updated?
                        break;
                }
            }
            else if (historyFunctionType == (int)HistoryFunctionTypes.Delete)
            {
                var isSubObject = DetermineIfCurrentObjectIsSubObject(@object);

                if (isSubObject)
                {
                    var history = new History()
                    {
                        FunctionTypeId = historyFunctionType,
                        When = now,
                        HouseId = houseId,
                        UserId = userId,
                        DeletedObjectDate = DateTime.Now,
                        TargetId = targetObjectId,
                        ObjectId = @object.ObjectId
                    };

                    switch (@object.ObjectTypeId)
                    {
                        case (long)ObjectTypes.Comment:
                            history.DeletedObjectObjectType = (long)ObjectTypes.Comment;

                            this.SaveHistory(history, userId);
                            break;
                        case (long)ObjectTypes.Image:
                            history.DeletedObjectObjectType = (long)ObjectTypes.Image;

                            this.SaveHistory(history, userId);
                            break;
                    }
                }
                else
                {
                    DeleteObjectMetadataSubObjects(@object);

                    var history = new History()
                    {
                        FunctionTypeId = historyFunctionType,
                        When = now,
                        HouseId = houseId,
                        DeletedObjectDate = DateTime.Now,
                        UserId = userId,
                        ObjectId = @object.ObjectId
                    };

                    switch (@object.ObjectTypeId)
                    {
                        case (long)ObjectTypes.User:
                            var userModel = objectModel as User;

                            history.DeletedObjectName = userModel.Username;
                            history.DeletedObjectObjectType = @object.ObjectTypeId;

                            this.SaveHistory(history, userId);
                            break;
                        case (long)ObjectTypes.House:
                            var houseModel = objectModel as House;

                            history.DeletedObjectName = houseModel.Name;
                            history.DeletedObjectObjectType = @object.ObjectTypeId;

                            this.SaveHistory(history, userId);
                            break;
                        case (long)ObjectTypes.Inventory:
                            var inventoryModel = objectModel as Inventory;

                            history.DeletedObjectName = inventoryModel.Name;
                            history.DeletedObjectObjectType = @object.ObjectTypeId;

                            this.SaveHistory(history, userId);
                            break;
                        case (long)ObjectTypes.Hygiene:
                            var hygieneModel = objectModel as Hygiene;

                            history.DeletedObjectName = hygieneModel.Name;
                            history.DeletedObjectObjectType = @object.ObjectTypeId;

                            this.SaveHistory(history, userId);
                            break;
                        case (long)ObjectTypes.Issue:
                            var issueModel = objectModel as Issue;

                            history.DeletedObjectName = issueModel.Name;
                            history.DeletedObjectObjectType = @object.ObjectTypeId;

                            this.SaveHistory(history, userId);
                            break;
                        case (long)ObjectTypes.Survey:
                            var surveyModel = objectModel as Survey;

                            history.DeletedObjectName = surveyModel.Name;
                            history.DeletedObjectObjectType = @object.ObjectTypeId;

                            this.SaveHistory(history, userId);
                            break;
                    }
                }
            }
        }

        private void DeleteObjectMetadataSubObjects(Models.Object @object)
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
            else if (@object.ObjectTypeId == (long)ObjectTypes.Inventory || @object.ObjectTypeId == (long)ObjectTypes.Hygiene ||
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
                4 => true, // HouseSettings
                9 => true, // Comment
                10 => true, // Image
                11 => true, // History
                _ => false,
            };

            return result;
        }

        public void SaveHistory(History objectModel, long userId)
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
    }
}
