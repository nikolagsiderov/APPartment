﻿namespace APPartment.ORM.Framework
{
    public static class Configuration
    {
        private static string koletoConnectionString = "Server=KOLENCETO\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        private static string aleksConnectionString = "Server=DESKTOP-MUOBF1E\\SQLEXPRESS;Database=APPartment;Trusted_Connection=True;MultipleActiveResultSets=true";
        public static string DefaultConnectionString
        {
            get
            {
                return aleksConnectionString;
            }
        }
    }
}
