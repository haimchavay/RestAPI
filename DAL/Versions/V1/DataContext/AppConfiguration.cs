using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL.Versions.V1.DataContext
{
    public class AppConfiguration
    {
        public string sqlConnectionString { get; set; }
        public AppConfiguration(string dbName)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            IConfigurationRoot root = configurationBuilder.Build();
            var appSetting = root.GetSection("ConnectionStrings:" + dbName);
            sqlConnectionString = appSetting.Value;
        }
    }
}
