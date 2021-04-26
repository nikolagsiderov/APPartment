using Microsoft.AspNetCore.Http;
using System;

namespace APPartment.Infrastructure.Tools
{
    public static class HumanSizeConverter
    {
        public static string ConvertFileLength(IFormFile file)
        {
            string[] sizes = { "bytes", "kB", "MB", "GB", "TB" };
            double len = file.Length;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);

            return result;
        }
    }
}
