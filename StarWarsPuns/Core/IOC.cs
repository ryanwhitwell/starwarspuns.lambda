
using Microsoft.Extensions.DependencyInjection;
using StarWarsPuns.Data;
using StarWarsPuns.Data.Interfaces;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using StarWarsPuns.BusinessLogic;
using NLog.Config;
using NLog;
using StarWarsPuns.BusinessLogic.Interfaces;
using Amazon.DynamoDBv2.DataModel;
using Amazon;
using Amazon.DynamoDBv2;
using StarWarsPuns.BusinessLogic.IntentRequestHandlers;
using StarWarsPuns.BusinessLogic.LaunchRequestHandlers;

namespace StarWarsPuns.Core
{
  public static class IOC
  {
    public static readonly ServiceProvider Container = GetServiceProvider();

    private static ServiceProvider GetServiceProvider()
    {
      IServiceCollection serviceCollection = new ServiceCollection();

      AmazonDynamoDBClient dynamoDBClient = new AmazonDynamoDBClient(RegionEndpoint.USEast1);

      // Logging
      serviceCollection.AddLogging(loggingBuilder =>
        {
          loggingBuilder.ClearProviders();
          loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
          loggingBuilder.AddNLog();
        });

      // Business Logic
      serviceCollection.AddSingleton<IDynamoDBContext>(new DynamoDBContext(dynamoDBClient, new DynamoDBContextConfig() { ConsistentRead = true }))
                        .AddTransient<IRequestBusinessLogic,  RequestBusinessLogic>()
                        .AddTransient<IRequestMapper,         RequestMapper>()
                        .AddTransient<IStarWarsPunRepository, StarWarsPunRepository>();

      // SessionEndedRequest
      serviceCollection.AddTransient<IRequestRouter,              SessionEndedRequestRouter>()
                       .AddTransient<ISessionEndedRequestHandler, DefaultSessionEndedRequest>();

      // LaunchRequest
      serviceCollection.AddTransient<IRequestRouter,        LaunchRequestRouter>()
                       .AddTransient<ILaunchRequestHandler, DefaultLaunchRequest>();
        
      // IntentRequest  
      serviceCollection.AddTransient<IRequestRouter,        IntentRequestRouter>()
                       .AddTransient<IIntentRequestHandler, GetNewPun>()
                       .AddTransient<IIntentRequestHandler, Help>()
                       .AddTransient<IIntentRequestHandler, Fallback>()
                       .AddTransient<IIntentRequestHandler, Stop>()
                       .AddTransient<IIntentRequestHandler, Cancel>()
                       .AddTransient<IIntentRequestHandler, NavigateHome>();

      // Set logging configuration
      LoggingConfiguration nlogConfig = new NLogLoggingConfiguration(Configuration.File.GetSection("NLog"));
      LogManager.Configuration = nlogConfig;

      return serviceCollection.BuildServiceProvider();;
    }
  }
}