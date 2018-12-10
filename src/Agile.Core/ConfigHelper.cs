using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Agile.Core
{
    public static class ConfigHelper
    {
        private static IConfiguration _configuration;

        static ConfigHelper()
        {
            var fileName = "appsettings.json";            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile(fileName, false, true);

            _configuration = builder.Build();
        }

        public static IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        public static string GetSectionValue(string key)
        {
            return _configuration.GetSection(key).Value;
        }
    }
}
