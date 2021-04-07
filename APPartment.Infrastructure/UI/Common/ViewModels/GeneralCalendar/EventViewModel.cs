namespace APPartment.Infrastructure.UI.Common.ViewModels.GeneralCalendar
{
    public class EventViewModel
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public bool AllDay { get; set; }
    }
}
