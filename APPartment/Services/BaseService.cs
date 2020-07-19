using APPartment.Data;
using APPartment.Enums;
using APPartment.Models.Declaration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.Services
{
    public class BaseService<T>
        where T : class, IBaseObject
    {
        private DataAccessContext _context;

        public BaseService(DataAccessContext context)
        {
            this._context = context;
        }

        public bool ObjectExists(long id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public string GetStatuses(Type objectType)
        {
            var objectTypeString = objectType.ToString().Split('.').Last();
            var statuses = new List<string>();

            switch (objectTypeString)
            {
                case "Inventory":
                    statuses.Add(BaseObjectStatus.Inventory1);
                    statuses.Add(BaseObjectStatus.Inventory2);
                    statuses.Add(BaseObjectStatus.Inventory3);
                    break;
                case "Hygiene":
                    statuses.Add(BaseObjectStatus.Hygiene1);
                    statuses.Add(BaseObjectStatus.Hygiene2);
                    statuses.Add(BaseObjectStatus.Hygiene3);
                    break;
                case "Issue":
                    statuses.Add(BaseObjectStatus.Issues1);
                    statuses.Add(BaseObjectStatus.Issues2);
                    statuses.Add(BaseObjectStatus.Issues3);
                    break;
                case "Chore":
                    statuses.Add(BaseObjectStatus.Chores1);
                    statuses.Add(BaseObjectStatus.Chores2);
                    statuses.Add(BaseObjectStatus.Chores3);
                    break;
                case "Survey":
                    statuses.Add(BaseObjectStatus.Surveys1);
                    statuses.Add(BaseObjectStatus.Surveys2);
                    statuses.Add(BaseObjectStatus.Surveys3);
                    break;
            }

            statuses.Add(BaseObjectStatus.Critical);
            var result = string.Join(",", statuses);

            return result;
        }
    }
}
