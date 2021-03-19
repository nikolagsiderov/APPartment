using APPartment.ORM.Framework.Declarations;
using APPartment.UI.ViewModels.Image;
using System;
using System.Collections.Generic;

namespace APPartment.UI.ViewModels.Base
{
    public abstract class GridItemViewModel : IBaseObject
    {
        public string Name { get; set; }

        public string Details { get; set; }

        #region Hidden properties
        public long Id { get; set; }

        public long ObjectId { get; set; }

        public long ObjectTypeId { get; set; }

        public long CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ModifiedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<string> Comments { get; set; }

        public List<ImagePostViewModel> Images { get; set; }

        public string LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public string LastUpdate { get; set; }
        #endregion
    }
}
