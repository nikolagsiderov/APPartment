using System.ComponentModel.DataAnnotations;

namespace APPartment.Infrastructure.Attributes
{
    // UIHintAttribute is inherited only for MVC reasons
    public class APPUIHintAttribute : UIHintAttribute
    {
        public APPUIHintAttribute(string template) : base(template)
        {
            this.Template = template;
        }

        public string Template { get; set; }

        public int Row { get; set; } = 0;

        public int Col { get; set; } = 0;

        public int Order { get; set; } = 99;

        public string Section { get; set; }

        public string SelectList { get; set; }
    }
}
