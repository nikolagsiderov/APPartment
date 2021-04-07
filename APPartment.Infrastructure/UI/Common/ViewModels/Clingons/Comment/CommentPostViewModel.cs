using APPartment.Infrastructure.Services.Attributes;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Web.Attributes;
using APPartment.Infrastructure.UI.Web.Constants;
using APPComment = APPartment.Data.Server.Models.Clingons.Comment;

namespace APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Comment
{
    [IMapFrom(typeof(APPComment))]
    public class CommentPostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Hidden)]
        public long TargetObjectID { get; set; }
    }
}
