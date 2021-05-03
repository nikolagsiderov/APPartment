using APPartment.Data.Server.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace APPartment.Data.Server.Models.Survey
{
    [Table("SurveyQuestion", Schema = "dbo")]
    public class SurveyQuestion : BaseObject
    {
        public long SurveyID { get; set; }
    }
}
