using Amazon.Lambda.Core;
using System.Threading.Tasks;
using StarWarsPuns.Core;
using NLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Alexa.NET.Request;
using Alexa.NET.Response;
using StarWarsPuns.BusinessLogic.Interfaces;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace StarWarsPuns
{
  public class Function
  {
    // Initialize Configuration
    private static readonly IConfigurationRoot configurationFile = Configuration.File;
    
    // Initialize DI Container
    private static readonly ServiceProvider container = IOC.Container;
    
    private IRequestBusinessLogic _businessLogic = container.GetService<IRequestBusinessLogic>();

    public async Task<SkillResponse> FunctionHandler(SkillRequest skillRequest, ILambdaContext context)
    {
      // Skill ID verified by AWS Lambda service configuration
      if (skillRequest.Version == "WARMING")
      {
        return null;
      }

      Logger logger = LogManager.GetCurrentClassLogger();

      SkillResponse response;
      try
      {
        logger.Log(LogLevel.Debug, "SkillRequest: " + JsonConvert.SerializeObject(skillRequest));
        
        response = await _businessLogic.HandleSkillRequest(skillRequest, context);
      }
      catch (Exception e)
      {
        logger.Log(LogLevel.Error, e);

        response = string.Format("I'm sorry, but I seem to be having trouble handling your request.").Tell(true);
      }

      logger.Log(LogLevel.Debug, "SkillResponse: " + JsonConvert.SerializeObject(response));

      return response;
    }
  }
}
