namespace APPartment.ORM.Framework
{
    public static class Configuration
    {
        private static string KoletoConnectionString = "Server=DESKTOP-QHJSV8M\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string KoletoApi = "https://localhost:44398/api";

        private static string JorkataConnectionString = "Server=DESKTOP-VLH0QE3\\SQLEXPRESS03;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string JorkataApi = string.Empty;

        public static string DefaultConnectionString
        {
            get
            {
                return KoletoConnectionString;
            }
        }

        public static string DefaultAPI
        {
            get
            {
                return KoletoApi;
            }
        }
    }
}
