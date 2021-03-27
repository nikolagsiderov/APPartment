using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;
using APPartment.UI.ViewModels.Clingons.Image;
using System;
using System.Collections.Generic;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class GridItemViewModel : IBaseObject
    {
        [GridFieldDisplay]
        public long ID { get; set; }

        [GridFieldDisplay]
        public virtual string Name { get; set; }

        [GridFieldDisplay]
        public string Details { get; set; }

        #region Hidden properties
        public long ObjectID { get; set; }

        public long ObjectTypeID { get; set; }

        public long CreatedByID { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedByID { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<string> Comments { get; set; }

        public List<ImagePostViewModel> Images { get; set; }

        public List<string> ActionsHtml { get; set; } = new List<string>();

        public string LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public string LastUpdate { get; set; }
        #endregion
    }
}
