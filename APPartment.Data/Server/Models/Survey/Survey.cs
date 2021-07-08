using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("Survey", Schema = "dbo")]
    public class Survey : BaseObject
    {
        [FieldMappingForMainTable]
        public DateTime ExpireDate { get; set; }

        [FieldMappingForMainTable]
        public long SurveyTypeID { get; set; }
    }
}
