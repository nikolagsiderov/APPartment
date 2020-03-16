using APPartment.Data;
using APPartment.Enums;
using APPartment.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APPartment.Utilities
{
    public class HistoryHtmlBuilder
    {
        public List<string> BuildBaseObjectHistory(List<History> history, DataAccessContext context)
        {
            var result = new List<string>();

            foreach (var historyEvent in history.OrderByDescending(x => x.Id))
            {
                var historyEventString = new StringBuilder();
                var username = context.Users.Find(historyEvent.UserId).Username;
                var isSubObject = historyEvent.TargetId == null ? false : true;
                long subObjectType = 0;
                var when = historyEvent.When.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                if (isSubObject)
                {
                    subObjectType = context.Objects.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().ObjectTypeId;
                }

                historyEventString.Append(username);

                if (historyEvent.FunctionTypeId == (int)HistoryFunctionTypes.Create)
                {
                    if (isSubObject)
                    {
                        if (subObjectType == (int)ObjectTypes.Comment)
                        {
                            historyEventString.Append(" posted a comment.");
                        }
                        else if (subObjectType == (int)ObjectTypes.Image)
                        {
                            historyEventString.Append(" attached an image.");
                        }
                    }
                    else
                    {
                        historyEventString.Append(" created this object.");
                    }
                }
                else if (historyEvent.FunctionTypeId == (int)HistoryFunctionTypes.Update)
                {
                    if (isSubObject)
                    {
                        if (subObjectType == (int)ObjectTypes.Comment)
                        {
                            // TODO: Implement this case when comments become editable.
                        }
                        else if (subObjectType == (int)ObjectTypes.Image)
                        {
                            // TODO: Implement this case if images become editable.
                        }
                    }
                    else
                    {
                        if (historyEvent.ColumnName == "IsCompleted")
                        {
                            var wasMarkedAsCompleted = bool.Parse(historyEvent.NewValue);

                            if (wasMarkedAsCompleted)
                            {
                                historyEventString.Append(" closed this object as completed (in its own context).");
                            }
                            else
                            {
                                historyEventString.Append(" opened this object as not completed (in its own context).");
                            }
                        }
                        else if (historyEvent.ColumnName == "Status")
                        {
                            switch (historyEvent.NewValue)
                            {
                                case "1":
                                    historyEventString.Append(" set status as trivial.");
                                    break;
                                case "2":
                                    historyEventString.Append(" set status as medium.");
                                    break;
                                case "3":
                                    historyEventString.Append(" set status as high.");
                                    break;
                                case "4":
                                    historyEventString.Append(" set status as critical.");
                                    break;
                            }
                        }
                        else
                        {
                            historyEventString.Append(string.Format(" updated {0} column. <br/> Previous value: <span style=\"text-decoration: line-through\">{1}</span> <br/> Current value: <span style=\"background-color: #90EE90\">{2}</span>"
                            , historyEvent.ColumnName.ToLower(), historyEvent.OldValue, historyEvent.NewValue));
                        }
                    }
                }
                else if (historyEvent.FunctionTypeId == (int)HistoryFunctionTypes.Delete)
                {
                    if (isSubObject)
                    {
                        if (subObjectType == (int)ObjectTypes.Comment)
                        {
                            historyEventString.Append(" deleted a comment.");
                        }
                        else if (subObjectType == (int)ObjectTypes.Image)
                        {
                            historyEventString.Append(" deleted an image.");
                        }
                    }
                }

                historyEventString.Append(string.Format(" <br/> <span style=\"font-size: x-small;\">{0}</span>", when));

                result.Add(historyEventString.ToString());
            }

            return result;
        }

        public List<string> BuildHomeHistory(List<History> history, DataAccessContext context)
        {
            var result = new List<string>();

            foreach (var historyEvent in history.OrderByDescending(x => x.Id))
            {
                var historyEventString = new StringBuilder();
                var username = context.Users.Find(historyEvent.UserId).Username;
                var isSubObject = historyEvent.TargetId == null ? false : true;
                var subObjectTargetObject = new object();
                long subObjectType = 0;
                long parentObjectType = 0;
                var when = historyEvent.When.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                if (isSubObject)
                {
                    subObjectType = context.Objects.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().ObjectTypeId;
                }
                else
                {
                    parentObjectType = context.Objects.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().ObjectTypeId;
                }

                historyEventString.Append(username);

                if (historyEvent.FunctionTypeId == (int)HistoryFunctionTypes.Create)
                {
                    if (isSubObject)
                    {
                        var parentObjectTypeId = context.Objects.Where(x => x.ObjectId == historyEvent.TargetId).FirstOrDefault().ObjectTypeId;

                        switch (subObjectType)
                        {
                            case (int)ObjectTypes.HouseStatus:
                                var houseStatus = context.HouseStatuses.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().Status;
                                var statusString = string.Empty;

                                if (houseStatus == (int)HomeStatus.Green)
                                {
                                    statusString = "free to enter";
                                }
                                else if (houseStatus == (int)HomeStatus.Yellow)
                                {
                                    statusString = "enter catiously";
                                }
                                else if (houseStatus == (int)HomeStatus.Red)
                                {
                                    statusString = "do not enter";
                                }

                                historyEventString.Append(string.Format(" set home status as {0}.", statusString));
                                break;
                            case (int)ObjectTypes.Comment:
                                switch (parentObjectTypeId)
                                {
                                    case (int)ObjectTypes.Inventory:
                                        var theInventory = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetId).FirstOrDefault();

                                        historyEventString.Append(string.Format(" posted a comment in object [Name: {0}] in Inventory.", theInventory.Name));
                                        break;
                                    case (int)ObjectTypes.Hygiene:
                                        var theHygiene = context.Hygienes.Where(x => x.ObjectId == historyEvent.TargetId).FirstOrDefault();

                                        historyEventString.Append(string.Format(" posted a comment in object [Name: {0}] in Hygiene.", theHygiene.Name));
                                        break;
                                    case (int)ObjectTypes.Issue:
                                        var theIssue = context.Hygienes.Where(x => x.ObjectId == historyEvent.TargetId).FirstOrDefault();

                                        historyEventString.Append(string.Format(" posted a comment in object [Name: {0}] in Issues.", theIssue.Name));
                                        break;
                                }
                                break;
                            case (int)ObjectTypes.Image:
                                switch (parentObjectTypeId)
                                {
                                    case (int)ObjectTypes.Inventory:
                                        var theInventory = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetId).FirstOrDefault();

                                        historyEventString.Append(string.Format(" attached an image in object [Name: {0}] in Inventory.", theInventory.Name));
                                        break;
                                    case (int)ObjectTypes.Hygiene:
                                        var theHygiene = context.Hygienes.Where(x => x.ObjectId == historyEvent.TargetId).FirstOrDefault();

                                        historyEventString.Append(string.Format(" attached an image in object [Name: {0}] in Hygiene.", theHygiene.Name));
                                        break;
                                    case (int)ObjectTypes.Issue:
                                        var theIssue = context.Hygienes.Where(x => x.ObjectId == historyEvent.TargetId).FirstOrDefault();

                                        historyEventString.Append(string.Format(" attached an image in object [Name: {0}] in Issues.", theIssue.Name));
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (parentObjectType)
                        {
                            case (int)ObjectTypes.Inventory:
                                var theInventoryObject = context.Inventories.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault();

                                historyEventString.Append(string.Format(" created an object [Name: {0}] in Inventory.", theInventoryObject.Name));
                                break;
                            case (int)ObjectTypes.Hygiene:
                                var theHygieneObject = context.Hygienes.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault();

                                historyEventString.Append(string.Format(" created an object [Name: {0}] in Hygiene.", theHygieneObject.Name));
                                break;
                            case (int)ObjectTypes.Issue:
                                var theIssueObject = context.Issues.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault();

                                historyEventString.Append(string.Format(" created an object [Name: {0}] in Issues.", theIssueObject.Name));
                                break;
                        }
                    }
                }
                else if (historyEvent.FunctionTypeId == (int)HistoryFunctionTypes.Update)
                {
                    if (isSubObject)
                    {
                        if (subObjectType == (int)ObjectTypes.Comment)
                        {
                            // TODO: Implement this case when comments become editable.
                            // ATM we will only display parent objects history
                        }
                        else if (subObjectType == (int)ObjectTypes.Image)
                        {
                            // TODO: Implement this case if images become editable.
                            // ATM we will only display parent objects history
                        }
                    }
                    else
                    {
                        switch (parentObjectType)
                        {
                            case (int)ObjectTypes.House:
                                var theHomeObject = context.Houses.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault();

                                if (historyEvent.ColumnName == "Name")
                                {
                                    historyEventString.Append(string.Format(" updated this home's name. <br/> Previous value: <span style=\"text-decoration: line-through\">{0}</span> <br/> Current value: <span style=\"background-color: #90EE90\">{1}</span>"
                                    , historyEvent.OldValue, historyEvent.NewValue));
                                }
                                break;
                            case (int)ObjectTypes.HouseStatus:
                                var houseStatus = context.HouseStatuses.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().Status;
                                var statusString = string.Empty;

                                if (houseStatus == (int)HomeStatus.Green)
                                {
                                    statusString = "free to enter";
                                }
                                else if (houseStatus == (int)HomeStatus.Yellow)
                                {
                                    statusString = "enter catiously";
                                }
                                else if (houseStatus == (int)HomeStatus.Red)
                                {
                                    statusString = "do not enter";
                                }

                                historyEventString.Append(string.Format(" set home status as {0}.", statusString));
                                break;
                            case (int)ObjectTypes.Inventory:
                                var theInventoryObject = context.Inventories.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault();

                                if (historyEvent.ColumnName == "IsCompleted")
                                {
                                    var wasMarkedAsCompleted = bool.Parse(historyEvent.NewValue);

                                    if (wasMarkedAsCompleted)
                                    {
                                        historyEventString.Append(string.Format(" marked an object [Name: {0}] as supplied in Inventory.", theInventoryObject.Name));
                                    }
                                    else
                                    {
                                        historyEventString.Append(string.Format(" marked an object [Name: {0}] as not supplied in Inventory.", theInventoryObject.Name));
                                    }
                                }
                                else if (historyEvent.ColumnName == "Status")
                                {
                                    switch (historyEvent.NewValue)
                                    {
                                        case "1":
                                            historyEventString.Append(string.Format(" set status as trivial for object [Name: {0}] in Inventory.", theInventoryObject.Name));
                                            break;
                                        case "2":
                                            historyEventString.Append(string.Format(" set status as medium for object [Name: {0}] in Inventory.", theInventoryObject.Name));
                                            break;
                                        case "3":
                                            historyEventString.Append(string.Format(" set status as high for object [Name: {0}] in Inventory.", theInventoryObject.Name));
                                            break;
                                        case "4":
                                            historyEventString.Append(string.Format(" set status as critical for object [Name: {0}] in Inventory.", theInventoryObject.Name));
                                            break;
                                    }
                                }
                                else
                                {
                                    historyEventString.Append(string.Format(" updated {0} column in object [Name: {3}] in Inventory. <br/> Previous value: <span style=\"text-decoration: line-through\">{1}</span> <br/> Current value: <span style=\"background-color: #90EE90\">{2}</span>"
                                    , historyEvent.ColumnName.ToLower(), historyEvent.OldValue, historyEvent.NewValue, theInventoryObject.Name));
                                }
                                break;
                            case (int)ObjectTypes.Hygiene:
                                var theHygieneObject = context.Hygienes.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault();

                                if (historyEvent.ColumnName == "IsCompleted")
                                {
                                    var wasMarkedAsCompleted = bool.Parse(historyEvent.NewValue);

                                    if (wasMarkedAsCompleted)
                                    {
                                        historyEventString.Append(string.Format(" marked an object [Name: {0}] as cleaned in Hygiene.", theHygieneObject.Name));
                                    }
                                    else
                                    {
                                        historyEventString.Append(string.Format(" marked an object [Name: {0}] as due cleaning in Hygiene.", theHygieneObject.Name));
                                    }
                                }
                                else if (historyEvent.ColumnName == "Status")
                                {
                                    switch (historyEvent.NewValue)
                                    {
                                        case "1":
                                            historyEventString.Append(string.Format(" set status as trivial for object [Name: {0}] in Hygiene.", theHygieneObject.Name));
                                            break;
                                        case "2":
                                            historyEventString.Append(string.Format(" set status as medium for object [Name: {0}] in Hygiene.", theHygieneObject.Name));
                                            break;
                                        case "3":
                                            historyEventString.Append(string.Format(" set status as high for object [Name: {0}] in Hygiene.", theHygieneObject.Name));
                                            break;
                                        case "4":
                                            historyEventString.Append(string.Format(" set status as critical for object [Name: {0}] in Hygiene.", theHygieneObject.Name));
                                            break;
                                    }
                                }
                                else
                                {
                                    historyEventString.Append(string.Format(" updated {0} column in object [Name: {3}] in Hygiene. <br/> Previous value: <span style=\"text-decoration: line-through\">{1}</span> <br/> Current value: <span style=\"background-color: #90EE90\">{2}</span>"
                                    , historyEvent.ColumnName.ToLower(), historyEvent.OldValue, historyEvent.NewValue, theHygieneObject.Name));
                                }
                                break;
                            case (int)ObjectTypes.Issue:
                                var theIssueObject = context.Issues.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault();

                                if (historyEvent.ColumnName == "IsCompleted")
                                {
                                    var wasMarkedAsCompleted = bool.Parse(historyEvent.NewValue);

                                    if (wasMarkedAsCompleted)
                                    {
                                        historyEventString.Append(string.Format(" marked an object [Name: {0}] as closed in Issues.", theIssueObject.Name));
                                    }
                                    else
                                    {
                                        historyEventString.Append(string.Format(" marked an object [Name: {0}] as open in Issues.", theIssueObject.Name));
                                    }
                                }
                                else if (historyEvent.ColumnName == "Status")
                                {
                                    switch (historyEvent.NewValue)
                                    {
                                        case "1":
                                            historyEventString.Append(string.Format(" set status as trivial for object [Name: {0}] in Issues.", theIssueObject.Name));
                                            break;
                                        case "2":
                                            historyEventString.Append(string.Format(" set status as medium for object [Name: {0}] in Issues.", theIssueObject.Name));
                                            break;
                                        case "3":
                                            historyEventString.Append(string.Format(" set status as high for object [Name: {0}] in Issues.", theIssueObject.Name));
                                            break;
                                        case "4":
                                            historyEventString.Append(string.Format(" set status as critical for object [Name: {0}] in Issues.", theIssueObject.Name));
                                            break;
                                    }
                                }
                                else
                                {
                                    historyEventString.Append(string.Format(" updated {0} column in object [Name: {3}] in Issues. <br/> Previous value: <span style=\"text-decoration: line-through\">{1}</span> <br/> Current value: <span style=\"background-color: #90EE90\">{2}</span>"
                                    , historyEvent.ColumnName.ToLower(), historyEvent.OldValue, historyEvent.NewValue, theIssueObject.Name));
                                }
                                break;
                        }
                    }
                }
                else if (historyEvent.FunctionTypeId == (int)HistoryFunctionTypes.Delete)
                {
                    switch (parentObjectType)
                    {
                        case (int)ObjectTypes.Inventory:
                            historyEventString.Append(" deleted an object in Inventory.");
                            break;
                        case (int)ObjectTypes.Hygiene:
                            historyEventString.Append(" deleted an object in Hygiene.");
                            break;
                        case (int)ObjectTypes.Issue:
                            historyEventString.Append(" deleted an object in Issues.");
                            break;
                    }
                }

                // Only implemented cases get added to the results list
                if (historyEventString.ToString().Length == username.Length)
                {
                    // The case is not implemented...
                }
                else
                {
                    historyEventString.Append(string.Format(" <br/> <span style=\"font-size: x-small;\">{0}</span>", when));

                    result.Add(historyEventString.ToString());
                }
            }

            return result;
        }
    }
}
