using System.ComponentModel.DataAnnotations;

namespace APPartment.UI.Attributes
{
    public class APPUIHintAttribute : UIHintAttribute
    {
        public APPUIHintAttribute(string template) : base(template)
        {
        }

        public int Row { get; set; } = 0;

        public int Col { get; set; } = 0;

        public int Order { get; set; } = 99;

        public string Section { get; set; }
    }
}
