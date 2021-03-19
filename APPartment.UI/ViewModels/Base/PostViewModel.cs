using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class PostViewModel : IBaseObject
    {
        [Required]
        [APPUIHint(Templates.Input)]
        public string Name { get; set; }

        [APPUIHint(Templates.TextArea)]
        public string Details { get; set; }

        #region Hidden properties
        [APPUIHint(Templates.Hidden)]
        public long Id { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectId { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectTypeId { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long CreatedById { get; set; }

        [APPUIHint(Templates.Hidden)]
        public DateTime CreatedDate { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long? ModifiedById { get; set; }

        [APPUIHint(Templates.Hidden)]
        public DateTime? ModifiedDate { get; set; }

        [APPUIHint(Templates.Hidden)]
        public List<string> Comments { get; set; }

        [APPUIHint(Templates.Hidden)]
        public List<ImagePostViewModel> Images { get; set; }

        [APPUIHint(Templates.Hidden)]
        public string LastUpdated { get; set; }

        [APPUIHint(Templates.Hidden)]
        public string LastUpdatedBy { get; set; }

        [APPUIHint(Templates.Hidden)]
        public string LastUpdate { get; set; }
        #endregion
    }
}
