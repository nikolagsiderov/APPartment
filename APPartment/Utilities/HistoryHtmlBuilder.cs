using APPartment.Data;
using APPartment.Enums;
using APPartment.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APPartment.Utilities
{
    public class HistoryHtmlBuilder
    {
        private DataAccessContext context;
        private TimeConverter timeConverter = new TimeConverter();

        public HistoryHtmlBuilder(DataAccessContext context)
        {
            this.context = context;
        }

        public string BuildLastUpdateBaseObjectHistoryForWidget(long objectId)
        {
            var result = string.Empty;

            var historyEvent = context.Audits.Where(x => x.ObjectId == objectId || x.TargetObjectId == objectId).OrderByDescending(x => x.Id).FirstOrDefault();

            var oldValues = new Dictionary<string, string>();
            var newValues = new Dictionary<string, string>();
            var isCreateEvent = string.IsNullOrEmpty(historyEvent.OldValues) && !string.IsNullOrEmpty(historyEvent.NewValues);
            var isUpdateEvent = !string.IsNullOrEmpty(historyEvent.OldValues) && !string.IsNullOrEmpty(historyEvent.NewValues);
            var isDeleteEvent = string.IsNullOrEmpty(historyEvent.NewValues);

            if (!string.IsNullOrEmpty(historyEvent.OldValues))
            {
                oldValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(historyEvent.OldValues);
            }

            if (!string.IsNullOrEmpty(historyEvent.NewValues))
            {
                newValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(historyEvent.NewValues);
            }

            var objectObjectId = historyEvent.ObjectId;
            var objectName = string.Empty;

            if (newValues.ContainsKey("Name"))
            {
                objectName = newValues["Name"];
            }

            var historyEventString = new StringBuilder();
            var referenceLink = $"<strong>[ID: {objectObjectId}, Name: {objectName}]</strong>";
            var isSubObject = historyEvent.TargetObjectId == null ? false : true;
            long objectObjectTypeId = context.Objects.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().ObjectTypeId;

            if (isCreateEvent)
            {
                if (isSubObject)
                {
                    if (context.Objects.Any(x => x.ObjectId == historyEvent.TargetObjectId))
                    {
                        var parentObjectTypeId = context.Objects.Where(x => x.ObjectId == historyEvent.TargetObjectId).FirstOrDefault().ObjectTypeId;
                        var appendString = string.Empty;

                        if (parentObjectTypeId == (int)ObjectTypes.Inventory || parentObjectTypeId == (int)ObjectTypes.Hygiene || parentObjectTypeId == (int)ObjectTypes.Issue)
                        {
                            var audit = context.Audits.OrderByDescending(x => x.Id).Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                            var auditValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(audit.NewValues);

                            appendString = string.Format($"<strong>[ID: {audit.ObjectId}, Name: {auditValues["Name"]}]</strong>.");
                        }

                        switch (objectObjectTypeId)
                        {
                            case (int)ObjectTypes.Comment:
                                historyEventString.Append("Posted a <strong>comment</strong> in ");
                                historyEventString.Append(appendString);
                                break;
                            case (int)ObjectTypes.Image:
                                historyEventString.Append("Attached an <strong>image</strong> in ");
                                historyEventString.Append(appendString);
                                break;
                        }
                    }
                }
                else
                {
                    historyEventString.Append("Created an object ");

                    if (objectObjectTypeId == (int)ObjectTypes.Inventory || objectObjectTypeId == (int)ObjectTypes.Hygiene || objectObjectTypeId == (int)ObjectTypes.Issue)
                    {
                        historyEventString.Append($"{referenceLink}.");
                    }
                }
            }
            else if (isUpdateEvent)
            {
                var modifications = 0;

                if (isSubObject)
                {
                    if (objectObjectTypeId == (int)ObjectTypes.Comment)
                    {
                        // TODO: Implement this case when comments become editable.
                        // ATM we will only display parent objects history
                    }
                    else if (objectObjectTypeId == (int)ObjectTypes.Image)
                    {
                        // TODO: Implement this case if images become editable.
                        // ATM we will only display parent objects history
                    }
                }
                else
                {
                    var isCompletedNewValue = string.Empty;
                    var statusNewValue = string.Empty;

                    if (newValues.ContainsKey("IsCompleted"))
                        isCompletedNewValue = newValues["IsCompleted"];

                    if (newValues.ContainsKey("Status"))
                        statusNewValue = newValues["Status"];

                    switch (objectObjectTypeId)
                    {
                        case (int)ObjectTypes.Inventory:

                            foreach (var column in newValues.Keys)
                            {
                                if (oldValues[column] != newValues[column])
                                {
                                    if (modifications >= 1)
                                    {
                                        historyEventString.Append("<br/>");
                                    }

                                    if (column == "IsCompleted")
                                    {
                                        var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                        historyEventString.Append("Marked ");

                                        if (wasMarkedAsCompleted)
                                        {
                                            historyEventString.Append(string.Format($"as <strong>supplied</strong>."));
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"as <strong>not supplied</strong>."));
                                        }
                                    }
                                    else if (column == "Status")
                                    {
                                        historyEventString.Append("Set status as ");

                                        switch (statusNewValue)
                                        {
                                            case "1":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Inventory1}</strong>."));
                                                break;
                                            case "2":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Inventory2}</strong>."));
                                                break;
                                            case "3":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Inventory3}</strong>."));
                                                break;
                                            case "4":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                    }

                                    modifications++;
                                }
                            }
                            break;
                        case (int)ObjectTypes.Hygiene:

                            foreach (var column in newValues.Keys)
                            {
                                if (oldValues[column] != newValues[column])
                                {
                                    if (modifications >= 1)
                                    {
                                        historyEventString.Append("<br/>");
                                    }

                                    if (column == "IsCompleted")
                                    {
                                        var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                        historyEventString.Append("Marked ");

                                        if (wasMarkedAsCompleted)
                                        {
                                            historyEventString.Append(string.Format($"as <strong>cleaned</strong>."));
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"as <strong>due cleaning</strong>."));
                                        }
                                    }
                                    else if (column == "Status")
                                    {
                                        historyEventString.Append("Set status as ");

                                        switch (statusNewValue)
                                        {
                                            case "1":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Hygiene1}</strong>."));
                                                break;
                                            case "2":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Hygiene2}</strong>."));
                                                break;
                                            case "3":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Hygiene3}</strong>."));
                                                break;
                                            case "4":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                    }

                                    modifications++;
                                }
                            }
                            break;
                        case (int)ObjectTypes.Issue:

                            foreach (var column in newValues.Keys)
                            {
                                if (oldValues[column] != newValues[column])
                                {
                                    if (modifications >= 1)
                                    {
                                        historyEventString.Append("<br/>");
                                    }

                                    if (column == "IsCompleted")
                                    {
                                        var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                        historyEventString.Append("Marked ");

                                        if (wasMarkedAsCompleted)
                                        {
                                            historyEventString.Append(string.Format($"as <strong>closed</strong>."));
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"as <strong>open</strong>."));
                                        }
                                    }
                                    else if (column == "Status")
                                    {
                                        historyEventString.Append("Set status as ");

                                        switch (statusNewValue)
                                        {
                                            case "1":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Issues1}</strong>."));
                                                break;
                                            case "2":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Issues2}</strong>."));
                                                break;
                                            case "3":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Issues3}</strong>."));
                                                break;
                                            case "4":
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                    }

                                    modifications++;
                                }
                            }
                            break;
                    }

                    historyEventString.Append($"<br/> Done in {referenceLink}");
                }
            }

            result = historyEventString.ToString();

            return result;
        }

        public List<string> BuildBaseObjectHistory(List<Audit> history)
        {
            var result = new List<string>();

            foreach (var historyEvent in history.OrderByDescending(x => x.Id))
            {
                var oldValues = new Dictionary<string, string>();
                var newValues = new Dictionary<string, string>();
                var isCreateEvent = string.IsNullOrEmpty(historyEvent.OldValues) && !string.IsNullOrEmpty(historyEvent.NewValues);
                var isUpdateEvent = !string.IsNullOrEmpty(historyEvent.OldValues) && !string.IsNullOrEmpty(historyEvent.NewValues);
                var isDeleteEvent = string.IsNullOrEmpty(historyEvent.NewValues);

                if (!string.IsNullOrEmpty(historyEvent.OldValues))
                {
                    oldValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(historyEvent.OldValues);
                }

                if (!string.IsNullOrEmpty(historyEvent.NewValues))
                {
                    newValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(historyEvent.NewValues);
                }

                var historyEventString = new StringBuilder();
                var username = $"<strong>{context.Users.Find(historyEvent.UserId).Username}</strong>";
                var isSubObject = historyEvent.TargetObjectId == null ? false : true;
                long objectType = 0;
                var when = historyEvent.When.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                if (isDeleteEvent)
                {
                    continue;
                }
                else
                {
                    if (context.Objects.Any(x => x.ObjectId == historyEvent.ObjectId))
                    {
                        objectType = context.Objects.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().ObjectTypeId;
                    }
                }

                if (isCreateEvent)
                {
                    historyEventString.Append(username);

                    if (isSubObject)
                    {
                        if (objectType == (int)ObjectTypes.Comment)
                        {
                            historyEventString.Append(" posted a <strong>comment</strong>.");
                        }
                        else if (objectType == (int)ObjectTypes.Image)
                        {
                            historyEventString.Append(" attached an <strong>image</strong>.");
                        }
                    }
                    else
                    {
                        historyEventString.Append(" created this object.");
                    }
                }
                else if (isUpdateEvent)
                {
                    var modifications = 0;

                    if (isSubObject)
                    {
                        if (objectType == (int)ObjectTypes.Comment)
                        {
                            // TODO: Implement this case when comments become editable.
                        }
                        else if (objectType == (int)ObjectTypes.Image)
                        {
                            // TODO: Implement this case if images become editable.
                        }
                    }
                    else
                    {
                        var isCompletedNewValue = string.Empty;
                        var statusNewValue = string.Empty;

                        if (newValues.ContainsKey("IsCompleted"))
                            isCompletedNewValue = newValues["IsCompleted"];

                        if (newValues.ContainsKey("Status"))
                            statusNewValue = newValues["Status"];

                        switch (objectType)
                        {
                            case (int)ObjectTypes.Inventory:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append("<strong>supplied</strong>.");
                                            }
                                            else
                                            {
                                                historyEventString.Append("<strong>not supplied</strong>.");
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Inventory1}</strong>.");
                                                    break;
                                                case "2":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Inventory2}</strong>.");
                                                    break;
                                                case "3":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Inventory3}</strong>.");
                                                    break;
                                                case "4":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Critical}</strong>.");
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }
                                break;
                            case (int)ObjectTypes.Hygiene:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append("<strong>cleaned</strong>.");
                                            }
                                            else
                                            {
                                                historyEventString.Append("<strong>due cleaning</strong>.");
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Hygiene1}</strong>.");
                                                    break;
                                                case "2":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Hygiene2}</strong>.");
                                                    break;
                                                case "3":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Hygiene3}</strong>.");
                                                    break;
                                                case "4":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Critical}</strong>.");
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }
                                break;
                            case (int)ObjectTypes.Issue:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append("<strong>closed</strong>.");
                                            }
                                            else
                                            {
                                                historyEventString.Append("<strong>open</strong>.");
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Issues1}</strong>.");
                                                    break;
                                                case "2":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Issues2}</strong>.");
                                                    break;
                                                case "3":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Issues3}</strong>.");
                                                    break;
                                                case "4":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Critical}</strong>.");
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }
                                break;
                            case (int)ObjectTypes.Survey:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append("<strong>completed</strong>.");
                                            }
                                            else
                                            {
                                                historyEventString.Append("<strong>pending</strong>.");
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Surveys1}</strong>.");
                                                    break;
                                                case "2":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Surveys2}</strong>.");
                                                    break;
                                                case "3":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Surveys3}</strong>.");
                                                    break;
                                                case "4":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Critical}</strong>.");
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }
                                break;
                            case (int)ObjectTypes.Chore:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append("<strong>completed</strong>.");
                                            }
                                            else
                                            {
                                                historyEventString.Append("<strong>pending</strong>.");
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Chores1}</strong>.");
                                                    break;
                                                case "2":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Chores2}</strong>.");
                                                    break;
                                                case "3":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Chores3}</strong>.");
                                                    break;
                                                case "4":
                                                    historyEventString.Append($"<strong>{BaseObjectStatus.Critical}</strong>.");
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }
                                break;
                        }
                    }

                    historyEventString.Append($"<br/> Modifications done by {username}.");
                }
                else if (isDeleteEvent)
                {
                    historyEventString.Append(username);

                    if (isSubObject)
                    {
                        historyEventString.Append(" <strong>deleted</strong> ");

                        if (objectType == (int)ObjectTypes.Comment)
                        {
                            historyEventString.Append("a comment.");
                        }
                        else if (objectType == (int)ObjectTypes.Image)
                        {
                            historyEventString.Append("an image.");
                        }
                    }
                }

                historyEventString.Append(string.Format(" <br/> <strong><span style=\"font-size: x-small; font-style: italic;\">{0} ({1})</span></strong>", timeConverter.CalculateRelativeTime(historyEvent.When), when));

                result.Add(historyEventString.ToString());
            }

            return result;
        }

        public List<string> BuildHomeHistory(List<Audit> history)
        {
            var result = new List<string>();

            foreach (var historyEvent in history.OrderByDescending(x => x.Id))
            {
                var oldValues = new Dictionary<string, string>();
                var newValues = new Dictionary<string, string>();
                var isCreateEvent = string.IsNullOrEmpty(historyEvent.OldValues) && !string.IsNullOrEmpty(historyEvent.NewValues);
                var isUpdateEvent = !string.IsNullOrEmpty(historyEvent.OldValues) && !string.IsNullOrEmpty(historyEvent.NewValues);
                var isDeleteEvent = string.IsNullOrEmpty(historyEvent.NewValues);

                if (!string.IsNullOrEmpty(historyEvent.OldValues))
                {
                    oldValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(historyEvent.OldValues);
                }

                if (!string.IsNullOrEmpty(historyEvent.NewValues))
                {
                    newValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(historyEvent.NewValues);
                }

                var objectObjectId = historyEvent.ObjectId;
                var objectName = string.Empty;

                if (newValues.ContainsKey("Name"))
                {
                    objectName = newValues["Name"];
                }

                var historyEventString = new StringBuilder();
                var username = $"<strong>{context.Users.Find(historyEvent.UserId).Username}</strong>";
                var referenceLink = $"<strong>[ID: {objectObjectId}, Name: {objectName}]</strong>";
                var moduleLink = string.Empty;
                var isSubObject = historyEvent.TargetObjectId == null ? false : true;
                var subObjectTargetObject = new object();
                long objectObjectTypeId = context.Objects.Where(x => x.ObjectId == historyEvent.ObjectId).FirstOrDefault().ObjectTypeId;
                var when = historyEvent.When.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                historyEventString.Append(username);

                if (isCreateEvent)
                {
                    if (isSubObject)
                    {
                        if (context.Objects.Any(x => x.ObjectId == historyEvent.TargetObjectId))
                        {
                            var parentObjectTypeId = context.Objects.Where(x => x.ObjectId == historyEvent.TargetObjectId).FirstOrDefault().ObjectTypeId;

                            var objectObjectIdAndNameHtml = string.Empty;

                            switch (objectObjectTypeId)
                            {
                                case (int)ObjectTypes.Comment:
                                    historyEventString.Append($" posted a <strong>comment</strong> in ");

                                    switch (parentObjectTypeId)
                                    {
                                        case (int)ObjectTypes.Inventory:
                                            var thisInventoryObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisInventoryObject.ObjectId}, Name: {thisInventoryObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Inventory/Index'>Inventory</a>."));
                                            break;
                                        case (int)ObjectTypes.Hygiene:
                                            var thisHygieneObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisHygieneObject.ObjectId}, Name: {thisHygieneObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Hygiene/Index'>Hygiene</a>."));
                                            break;
                                        case (int)ObjectTypes.Issue:
                                            var thisIssueObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisIssueObject.ObjectId}, Name: {thisIssueObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Issues/Index'>Issues</a>."));
                                            break;
                                        case (int)ObjectTypes.Survey:
                                            var thisSurveyObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisSurveyObject.ObjectId}, Name: {thisSurveyObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Surveys/Index'>Surveys</a>."));
                                            break;
                                        case (int)ObjectTypes.Chore:
                                            var thisChoreObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisChoreObject.ObjectId}, Name: {thisChoreObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Chores/Index'>Chores</a>."));
                                            break;
                                    }
                                    break;
                                case (int)ObjectTypes.Image:
                                    historyEventString.Append(" attached an <strong>image</strong> in ");

                                    switch (parentObjectTypeId)
                                    {
                                        case (int)ObjectTypes.Inventory:
                                            var thisInventoryObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisInventoryObject.ObjectId}, Name: {thisInventoryObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Inventory/Index'>Inventory</a>."));
                                            break;
                                        case (int)ObjectTypes.Hygiene:
                                            var thisHygieneObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisHygieneObject.ObjectId}, Name: {thisHygieneObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Hygiene/Index'>Hygiene</a>."));
                                            break;
                                        case (int)ObjectTypes.Issue:
                                            var thisIssueObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisIssueObject.ObjectId}, Name: {thisIssueObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Issues/Index'>Issues</a>."));
                                            break;
                                        case (int)ObjectTypes.Survey:
                                            var thisSurveyObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisSurveyObject.ObjectId}, Name: {thisSurveyObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Surveys/Index'>Surveys</a>."));
                                            break;
                                        case (int)ObjectTypes.Chore:
                                            var thisChoreObject = context.Inventories.Where(x => x.ObjectId == historyEvent.TargetObjectId).First();
                                            objectObjectIdAndNameHtml = $"<strong>[ID: {thisChoreObject.ObjectId}, Name: {thisChoreObject.Name}]</strong>";
                                            historyEventString.Append(string.Format($"{objectObjectIdAndNameHtml} in <a href='/Chores/Index'>Chores</a>."));
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        var statusOldValue = string.Empty;
                        var statusNewValue = string.Empty;

                        if (oldValues.ContainsKey("Status"))
                            statusOldValue = oldValues["Status"];

                        if (newValues.ContainsKey("Status"))
                            statusNewValue = newValues["Status"];

                        switch (objectObjectTypeId)
                        {
                            case (int)ObjectTypes.Inventory:
                                historyEventString.Append(string.Format($" created an object {referenceLink} in <a href='/Inventory/Index'>Inventory</a>."));
                                break;
                            case (int)ObjectTypes.Hygiene:
                                historyEventString.Append(string.Format($" created an object {referenceLink} in <a href='/Hygiene/Index'>Hygiene</a>."));
                                break;
                            case (int)ObjectTypes.Issue:
                                historyEventString.Append(string.Format($" created an object {referenceLink} in <a href='/Issues/Index'>Issues</a>."));
                                break;
                            case (int)ObjectTypes.Survey:
                                historyEventString.Append(string.Format($" created an object {referenceLink} in <a href='/Surveys/Index'>Surveys</a>."));
                                break;
                            case (int)ObjectTypes.Chore:
                                historyEventString.Append(string.Format($" created an object {referenceLink} in <a href='/Chores/Index'>Chores</a>."));
                                break;
                            case (int)ObjectTypes.HouseStatus:
                                if (statusOldValue != statusNewValue)
                                {
                                    var houseStatus = int.Parse(statusNewValue);
                                    var statusString = string.Empty;

                                    if (houseStatus == (int)HomeStatus.Green)
                                    {
                                        statusString = "<strong>free to enter</strong>";
                                    }
                                    else if (houseStatus == (int)HomeStatus.Yellow)
                                    {
                                        statusString = "<strong>enter catiously</strong>";
                                    }
                                    else if (houseStatus == (int)HomeStatus.Red)
                                    {
                                        statusString = "<strong>do not enter</strong>";
                                    }

                                    historyEventString.Append(string.Format(" set home status as {0}.", statusString));
                                }
                                break;
                        }
                    }
                }
                else if (isUpdateEvent)
                {
                    historyEventString.Clear();
                    var modifications = 0;

                    if (isSubObject)
                    {
                        if (objectObjectTypeId == (int)ObjectTypes.Comment)
                        {
                            // TODO: Implement this case when comments become editable.
                            // ATM we will only display parent objects history
                        }
                        else if (objectObjectTypeId == (int)ObjectTypes.Image)
                        {
                            // TODO: Implement this case if images become editable.
                            // ATM we will only display parent objects history
                        }
                    }
                    else
                    {
                        var isCompletedNewValue = string.Empty;
                        var statusOldValue = string.Empty;
                        var statusNewValue = string.Empty;

                        if (newValues.ContainsKey("IsCompleted"))
                            isCompletedNewValue = newValues["IsCompleted"];

                        if (oldValues.ContainsKey("Status"))
                            statusOldValue = oldValues["Status"];

                        if (newValues.ContainsKey("Status"))
                            statusNewValue = newValues["Status"];

                        switch (objectObjectTypeId)
                        {
                            case (int)ObjectTypes.House:
                                var oldObjectName = oldValues["Name"];

                                if (oldObjectName != objectName)
                                {
                                    historyEventString.Append(string.Format($"{username} updated home's name. Previous value: <span style=\"text-decoration: line-through\">{oldObjectName}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{objectName}</span>."));
                                }
                                break;
                            case (int)ObjectTypes.HouseStatus:
                                if (statusOldValue != statusNewValue)
                                {
                                    var houseStatus = int.Parse(statusNewValue);
                                    var statusString = string.Empty;

                                    if (houseStatus == (int)HomeStatus.Green)
                                    {
                                        statusString = $"<strong>{HomeStatusString.Green}</strong>";
                                    }
                                    else if (houseStatus == (int)HomeStatus.Yellow)
                                    {
                                        statusString = $"<strong>{HomeStatusString.Yellow}</strong>";
                                    }
                                    else if (houseStatus == (int)HomeStatus.Red)
                                    {
                                        statusString = $"<strong>{HomeStatusString.Red}</strong>";
                                    }

                                    historyEventString.Append(string.Format("{1} set home status as {0}.", statusString, username));
                                }
                                break;
                            case (int)ObjectTypes.Inventory:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Inventory1}</strong>."));
                                            }
                                            else
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Inventory1}</strong>."));
                                                    break;
                                                case "2":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Inventory2}</strong>."));
                                                    break;
                                                case "3":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Inventory3}</strong>."));
                                                    break;
                                                case "4":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }

                                historyEventString.Append($"<br/> Done in {referenceLink} from <a href='/Inventory/Index'>Inventory</a> by {username}.");
                                break;
                            case (int)ObjectTypes.Hygiene:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Hygiene1}</strong>."));
                                            }
                                            else
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Hygiene1}</strong>."));
                                                    break;
                                                case "2":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Hygiene2}</strong>."));
                                                    break;
                                                case "3":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Hygiene3}</strong>."));
                                                    break;
                                                case "4":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }

                                historyEventString.Append($"<br/> Done in {referenceLink} from <a href='/Hygiene/Index'>Hygiene</a> by {username}.");
                                break;
                            case (int)ObjectTypes.Issue:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Issues1}</strong>."));
                                            }
                                            else
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Issues1}</strong>."));
                                                    break;
                                                case "2":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Issues2}</strong>."));
                                                    break;
                                                case "3":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Issues3}</strong>."));
                                                    break;
                                                case "4":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }

                                historyEventString.Append($"<br/> Done in {referenceLink} from <a href='/Issues/Index'>Issues</a> by {username}.");
                                break;
                            case (int)ObjectTypes.Survey:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Surveys1}</strong>."));
                                            }
                                            else
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Surveys1}</strong>."));
                                                    break;
                                                case "2":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Surveys2}</strong>."));
                                                    break;
                                                case "3":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Surveys3}</strong>."));
                                                    break;
                                                case "4":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }

                                historyEventString.Append($"<br/> Done in {referenceLink} from <a href='/Surveys/Index'>Surveys</a> by {username}.");
                                break;
                            case (int)ObjectTypes.Chore:

                                foreach (var column in newValues.Keys)
                                {
                                    if (oldValues[column] != newValues[column])
                                    {
                                        if (modifications >= 1)
                                        {
                                            historyEventString.Append("<br/>");
                                        }

                                        if (column == "IsCompleted")
                                        {
                                            var wasMarkedAsCompleted = bool.Parse(isCompletedNewValue);

                                            historyEventString.Append("Marked as ");

                                            if (wasMarkedAsCompleted)
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Chores1}</strong>."));
                                            }
                                            else
                                            {
                                                historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                            }
                                        }
                                        else if (column == "Status")
                                        {
                                            historyEventString.Append("Set status as ");

                                            switch (statusNewValue)
                                            {
                                                case "1":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Chores1}</strong>."));
                                                    break;
                                                case "2":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Chores2}</strong>."));
                                                    break;
                                                case "3":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Chores3}</strong>."));
                                                    break;
                                                case "4":
                                                    historyEventString.Append(string.Format($"<strong>{BaseObjectStatus.Critical}</strong>."));
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            historyEventString.Append(string.Format($"Updated {column.ToLower()} column. Previous value: <span style=\"text-decoration: line-through\">{oldValues[column]}</span>. <strong>Current value</strong>: <span style=\"background-color: #90EE90\">{newValues[column]}</span>."));
                                        }

                                        modifications++;
                                    }
                                }

                                historyEventString.Append($"<br/> Done in {referenceLink} from <a href='/Chores/Index'>Chores</a> by {username}.");
                                break;
                        }
                    }
                }

                // Only implemented cases get added to the results list
                if (historyEventString.ToString().Length == username.Length)
                {
                    // TODO: Implement case...
                }
                else
                {
                    historyEventString.Append(string.Format(" <br/> <strong><span style=\"font-size: x-small; font-style: italic;\">{0} ({1})</span></strong>", timeConverter.CalculateRelativeTime(historyEvent.When), when));

                    result.Add(historyEventString.ToString());
                }
            }

            return result;
        }
    }
}
