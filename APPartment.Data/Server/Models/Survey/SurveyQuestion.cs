using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("SurveyQuestion", Schema = "dbo")]
    public class SurveyQuestion : BaseObject
    {
        [FieldMappingForMainTable]
        public long SurveyID { get; set; }
    }
}
