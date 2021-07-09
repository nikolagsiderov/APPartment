using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("SurveyParticipantStatus", Schema = "dbo")]
    public class SurveyParticipantStatus : LookupObject
    {
    }
}
