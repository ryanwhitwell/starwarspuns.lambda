using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class Cancel : BaseRequestHandler<Cancel>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Cancel; } }
    
    public Cancel(ILogger<Cancel> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest)
    {
      logger.LogTrace("BEGIN Cancel. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = "Okay".Tell(true);

      logger.LogTrace("END Cancel. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}