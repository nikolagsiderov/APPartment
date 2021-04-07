namespace APPartment.Infrastructure.UI.Common.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestID { get; set; }

        public bool ShowRequestID => !string.IsNullOrEmpty(RequestID);
    }
}
