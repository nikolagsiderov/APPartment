namespace APPartment.ORM.Framework
{
    public static class Configuration
    {
        private static string KoletoConnectionString = "Server=KOLENCETO\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string JorkataConnectionString = "Server=DESKTOP-VLH0QE3\\SQLEXPRESS03;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static string DefaultConnectionString
        {
            get
            {
                return KoletoConnectionString;
            }
        }
    }
}
