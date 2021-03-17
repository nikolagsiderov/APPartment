using APPartment.Data.Server.Models.MetaObjects;
using APPartment.ORM.Framework.Attributes;
using APPartment.ORM.Framework.Declarations;
using APPartment.UI.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APPartment.Data.Server.Models.Base
{
    public abstract class BaseObject : IBaseObject
    {
        [FieldMappingForObjectTablePrimaryKey]
        [APPUIHint(Templates.Hidden)]
        public long ObjectId { get; set; }

        [FieldMappingForObjectTable]
        [APPUIHint(Templates.Hidden)]
        public long ObjectTypeId { get; set; }

        [FieldMappingForObjectTable]
        [Required]
        [APPUIHint(Templates.Input)]
        public string Name { get; set; }

        [FieldMappingForObjectTable]
        [APPUIHint(Templates.TextArea)]
        public string Details { get; set; }

        [FieldMappingForObjectTable]
        [APPUIHint(Templates.Hidden)]
        public long CreatedById { get; set; }

        [FieldMappingForObjectTable]
        [APPUIHint(Templates.Hidden)]
        public DateTime CreatedDate { get; set; }

        [FieldMappingForObjectTable]
        [APPUIHint(Templates.Hidden)]
        public long? ModifiedById { get; set; }

        [FieldMappingForObjectTable]
        [APPUIHint(Templates.Hidden)]
        public DateTime? ModifiedDate { get; set; }

        public List<string> Comments { get; set; }

        public List<Image> Images { get; set; }

        [APPUIHint(Templates.Hidden)]
        public string LastUpdated { get; set; }

        [APPUIHint(Templates.Hidden)]
        public string LastUpdatedBy { get; set; }

        [APPUIHint(Templates.Hidden)]
        public string LastUpdate { get; set; }
    }
}
