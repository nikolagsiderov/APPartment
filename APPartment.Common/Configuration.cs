﻿namespace APPartment.Common
{
    public static class Configuration
    {
        private static string KoletoConnectionString = "Server=KOLENCETO\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string KoletoAPI = "https://localhost:44310/api";

        private static string JorkataConnectionString = "Server=DESKTOP-VLH0QE3\\SQLEXPRESS03;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string JorkataAPI = string.Empty;

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
                return KoletoAPI;
            }
        }
    }
}
