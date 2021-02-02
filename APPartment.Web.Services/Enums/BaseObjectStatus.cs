namespace APPartment.Web.Services.Enums
{
    public class BaseObjectStatus
    {
        public const string Inventory1 = "Well supplied";
        public const string Inventory2 = "Supplied";
        public const string Inventory3 = "Low supply";

        public const string Hygiene1 = "Well cleaned";
        public const string Hygiene2 = "Clean";
        public const string Hygiene3 = "Not clean";

        public const string Issues1 = "Not present";
        public const string Issues2 = "Present";
        public const string Issues3 = "High priority";

        public const string Chores1 = "Low priority";
        public const string Chores2 = "Medium priority";
        public const string Chores3 = "High priority";

        public const string Surveys1 = "Optional";
        public const string Surveys2 = "Recommended";
        public const string Surveys3 = "Mandatory";

        public const string Critical = "Critical";
    }
}
