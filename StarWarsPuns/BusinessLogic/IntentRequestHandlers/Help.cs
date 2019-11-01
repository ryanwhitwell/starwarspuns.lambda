using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class Help : BaseRequestHandler<Help>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Help; } }
    
    public Help(ILogger<Help> logger) : base(logger) { }

    public SkillResponse Handle(SkillRequest skillRequest)
    {
      logger.LogTrace("BEGIN Help. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = string.Format("To hear a pun you can say something like, <emphasis>tell</emphasis> me a pun, or <emphasis>give</emphasis> me a star wars pun. " +
        "If you need more help, please check the instructions provided in the description of this skill in the Alexa skill catalog. So, what can I help you with?")
        .Tell(false);

      logger.LogTrace("END Help. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}