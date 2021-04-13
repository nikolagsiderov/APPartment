namespace APPartment.Common
{
    public static class Configuration
    {
        private static string KoletoConnectionString = "Server=KOLENCETO\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string KoletoAPI = "https://localhost:44322/api";

        private static string JorkataConnectionString = "Server=DESKTOP-VLH0QE3\\SQLEXPRESS03;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string JorkataAPI = "https://localhost:44323/api";

        public static string DefaultConnectionString
        {
            get
            {
                return JorkataConnectionString;
            }
        }

        public static string DefaultAPI
        {
            get
            {
                return JorkataAPI;
            }
        }
    }
}
