using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Link;
using System.Collections.Generic;
using System.Linq;

namespace APPartment.Infrastructure.UI.Web.Html
{
    public static class ObjectLinksRenderer
    {
        public static List<string> BuildLinks(List<ObjectLinkPostViewModel> links)
        {
            return links
                .OrderByDescending(x => x.ID)
                .Take(20)
                .Select(e => $"<div class='row'><div class='col-md-2'>{e.ObjectAName}</div><div class='col-md-3'>{e.ObjectLinkType}</div><div class='col-md-2'>{e.ObjectBName}</div></div>")
                .ToList();
        }

        public static string BuildPostLink(ObjectLinkPostViewModel link)
        {
            return $"<div class='row'><div class='col-md-2'>{link.ObjectAName}</div><div class='col-md-3'>{link.ObjectLinkType}</div><div class='col-md-2'>{link.ObjectBName}</div></div>";
        }
    }
}
