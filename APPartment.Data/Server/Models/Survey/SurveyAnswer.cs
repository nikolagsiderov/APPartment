using APPartment.Data.Server.Models.Base;
using APPartment.ORM.Framework.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("SurveyAnswer", Schema = "dbo")]
    public class SurveyAnswer : BaseObject
    {
        [FieldMappingForMainTable]
        public string Answer { get; set; }

        [FieldMappingForMainTable]
        public bool IsCorrect { get; set; }

        [FieldMappingForMainTable]
        public int? ChoiceCap { get; set; }

        [FieldMappingForMainTable]
        public long QuestionID { get; set; }
    }
}
