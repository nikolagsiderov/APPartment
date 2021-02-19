namespace APPartment.Data.Core
{
    public static class Configuration
    {
        private static string defaultConnectionString = "Server=KOLENCETO\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static string DefaultConnectionString
        {
            get
            {
                return defaultConnectionString;
            }
        }
    }
}
