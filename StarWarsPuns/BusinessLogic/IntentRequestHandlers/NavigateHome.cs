using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class NavigateHome : BaseRequestHandler<NavigateHome>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.NavigateHome; } }
    
    public NavigateHome(ILogger<NavigateHome> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest)
    {
      logger.LogTrace("BEGIN NavigateHome. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = "Done".Tell(true);

      logger.LogTrace("END NavigateHome. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}