using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Base;
using APPComment = APPartment.Data.Server.Models.Clingons.Comment;

namespace APPartment.UI.ViewModels.Clingons.Comment
{
    [IMapFrom(typeof(APPComment))]
    public class CommentPostViewModel : PostViewModel
    {
        [APPUIHint(Templates.Hidden)]
        public long TargetObjectID { get; set; }
    }
}
