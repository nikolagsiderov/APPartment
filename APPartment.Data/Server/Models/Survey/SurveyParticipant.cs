using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("SurveyParticipant", Schema = "dbo")]
    public class SurveyParticipant : BaseObject
    {
        [FieldMappingForMainTable]
        public long SurveyID { get; set; }

        [FieldMappingForMainTable]
        public long UserID { get; set; }

        [FieldMappingForMainTable]
        public long StatusID { get; set; }
    }
}
