﻿using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;
using APPartment.UI.Utilities.Constants;
using APPartment.UI.ViewModels.Clingons.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class PostViewModel : IBaseObject
    {
        [Required]
        [APPUIHint(Templates.Input)]
        public virtual string Name { get; set; }

        [APPUIHint(Templates.TextArea)]
        public string Details { get; set; }

        #region Hidden properties
        [APPUIHint(Templates.Hidden)]
        public long ID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long ObjectTypeID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long CreatedByID { get; set; }

        [APPUIHint(Templates.Hidden)]
        public DateTime CreatedDate { get; set; }

        [APPUIHint(Templates.Hidden)]
        public long? ModifiedByID { get; set; }

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
