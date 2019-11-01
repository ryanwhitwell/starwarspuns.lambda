using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;
using StarWarsPuns.Data.Interfaces;

namespace StarWarsPuns.BusinessLogic.LaunchRequestHandlers
{
  public class DefaultLaunchRequest : BaseRequestHandler<DefaultLaunchRequest>, ILaunchRequestHandler
  {
    private static readonly int LESS_PUN_COUNT = 5;
    
    private IStarWarsPunRepository _repository;
    
    public string HandlerName { get { return LaunchRequestName.Default; } }
    
    public DefaultLaunchRequest(ILogger<DefaultLaunchRequest> logger, IStarWarsPunRepository repository) : base(logger)
    {
      _repository = repository ?? throw new ArgumentNullException("repository");
     }

    public SkillResponse Handle(SkillRequest skillRequest)
    {
      logger.LogTrace("BEGIN Default. RequestId: {0}.", skillRequest.Request.RequestId);

      long itemCount = Task.Run(async () => await _repository.Count()).Result;

      SkillResponse response = string.Format("Welcome to {0}. I can entertain you with over {1} puns. " +
        "To hear a pun you can say something like, <emphasis>tell</emphasis> me a pun, or <emphasis>give</emphasis> me a star wars pun. " +
        "So, what can I help you with?", 
        Configuration.File.GetSection("Application")["SkillName"],
        itemCount - LESS_PUN_COUNT)
        .Tell(false);

      logger.LogTrace("END Default. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}