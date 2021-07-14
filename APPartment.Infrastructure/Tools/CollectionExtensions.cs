using APPartment.ORM.Framework.Declarations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.Infrastructure.Tools
{
    public static class CollectionExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(this List<T> collection)
            where T : IBaseObject, new()
        {
            var selectList = new List<SelectListItem>();

            foreach (var item in collection)
            {
                selectList.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
            }

            return selectList;
        }

        public static List<SelectListItem> ToLookupSelectList<T>(this List<T> collection)
            where T : ILookupObject, new()
        {
            var selectList = new List<SelectListItem>();

            foreach (var item in collection)
            {
                selectList.Add(new SelectListItem() { Text = item.Name, Value = item.ID.ToString() });
            }

            return selectList;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }
    }
}
