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
        public List<string> BuildBaseObjectHistory(List<History> history, long objectId, DataAccessContext context)
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
    }
}
