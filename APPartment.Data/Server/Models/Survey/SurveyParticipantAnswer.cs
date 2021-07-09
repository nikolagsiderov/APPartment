using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("SurveyParticipantAnswer", Schema = "dbo")]
    public class SurveyParticipantAnswer : BaseObject
    {
        [FieldMappingForMainTable]
        public long SurveyParticipantID { get; set; }

        [FieldMappingForMainTable]
        public long AnswerID { get; set; }
    }
}
