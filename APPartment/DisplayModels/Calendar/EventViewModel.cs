namespace APPartment.DisplayModels.Calendar
{
    public class EventViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public bool AllDay { get; set; }
    }
}
