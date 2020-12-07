using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL.DataContext
{
    public class AppConfiguration
    {
        public string sqlConnectionString { get; set; }
        public AppConfiguration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            IConfigurationRoot root = configurationBuilder.Build();
            var appSetting = root.GetSection("ConnectionStrings:CarticiaDb");
            sqlConnectionString = appSetting.Value;
        }
    }
}
