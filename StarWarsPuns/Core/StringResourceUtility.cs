using System.Reflection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace StarWarsPuns.Core
{
  public static class StringResourceUtility
  {
    public static readonly IStringLocalizer Localizer = InitializeStringLocalizer();

    private static IStringLocalizer InitializeStringLocalizer()
    {
      IOptions<LocalizationOptions> options = Options.Create(new LocalizationOptions(){ ResourcesPath = "Resources" });
      ResourceManagerStringLocalizerFactory factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);

      System.Type type = typeof(StarWarsPuns);
      AssemblyName assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
      IStringLocalizer localizer = factory.Create("StarWarsPuns", assemblyName.Name);

      return localizer;
    }
  }
}