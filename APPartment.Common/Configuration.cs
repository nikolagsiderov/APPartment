using System.IO;

namespace APPartment.Common
{
    public static class Configuration
    {
        private static string KoletoConnectionString = "Server=KOLENCETO\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string KoletoAPI = "https://localhost:44305/api";
        private static string KoletoWebURL = "http://localhost:55171/";

        private static string JorkataConnectionString = "Server=DESKTOP-VLH0QE3\\SQLEXPRESS03;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string JorkataAPI = "https://localhost:44323/api";
        private static string JorkataWebURL = string.Empty;

        public static string DefaultConnectionString => KoletoConnectionString;

        public static string DefaultAPI => KoletoAPI;

        public static string DefaultBaseURL => KoletoWebURL;

        public static string ImagesPath
        {
            get
            {
                string currentDir = Directory.GetCurrentDirectory();
                string parent = Directory.GetParent(currentDir).FullName;
                string imagesFileSharePath = $"{parent}\\APPartment.Web\\wwwroot\\fileshare\\images";

                return imagesFileSharePath;
            }
        }
    }
}
