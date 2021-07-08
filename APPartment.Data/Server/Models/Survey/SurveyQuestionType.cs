using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("SurveyQuestionType", Schema = "dbo")]
    public class SurveyQuestionType : LookupObject
    {
    }
}
