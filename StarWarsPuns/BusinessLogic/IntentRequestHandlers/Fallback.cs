using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class Fallback : BaseRequestHandler<Fallback>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Fallback; } }
    
    public Fallback(ILogger<Fallback> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest)
    {
      logger.LogTrace("BEGIN Fallback. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = string.Format("Hmm, I'm not sure what you wanted there. Please try again.").Tell(false);

      logger.LogTrace("END Fallback. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}