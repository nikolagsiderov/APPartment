using APPartment.ORM.Framework.Declarations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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
    }
}
