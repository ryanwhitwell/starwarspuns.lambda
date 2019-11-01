using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class Stop : BaseRequestHandler<Stop>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Stop; } }
    
    public Stop(ILogger<Stop> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest)
    {
      logger.LogTrace("BEGIN Stop. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = "Alright".Tell(true);

      logger.LogTrace("END Stop. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}