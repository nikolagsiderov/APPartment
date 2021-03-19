using System.ComponentModel.DataAnnotations;

namespace APPartment.UI.Attributes
{
    public class APPUIHintAttribute : UIHintAttribute
    {
        public APPUIHintAttribute(string template) : base(template)
        {
        }
    }
}
