﻿using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.MetaObjects
{
    [Table("Link", Schema = "dbo")]
    public class Link : IdentityBaseObject
    {
        [FieldMappingForMainTable]
        public long LinkTypeId { get; set; }

        [FieldMappingForMainTable]
        public long LinkedObjectId { get; set; }

        [FieldMappingForMainTable]
        public long TargetObjectId { get; set; }
    }
}
