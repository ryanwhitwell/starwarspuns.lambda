using System.IO;
using Microsoft.Extensions.Configuration;

namespace StarWarsPuns.Core
{
  public static class Configuration
  {
    private static string CONFIG_FILE_NAME = "appsettings.json";
    public static readonly IConfigurationRoot File = LoadConfigurationFile();

    private static IConfigurationRoot LoadConfigurationFile()
    {
      IConfigurationBuilder builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile(Configuration.CONFIG_FILE_NAME, optional: false, reloadOnChange: false);

      IConfigurationRoot configurationRoot = builder.Build();

      return configurationRoot;
    }
  }
}