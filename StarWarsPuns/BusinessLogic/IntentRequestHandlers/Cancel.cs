using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Models;
using StarWarsPuns.Core;
using System;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class Cancel : BaseRequestHandler<Cancel>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.Cancel; } }
    
    public Cancel(ILogger<Cancel> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

    public SkillResponse Handle(SkillRequest skillRequest, TokenUser tokenUser)
    {
      if (!base.skillRequestValidator.IsValid(skillRequest))
      {
        throw new ArgumentNullException("skillRequest");
      }
      
      if (tokenUser == null)
      {
        throw new ArgumentNullException("tokenUser");
      }
      
      logger.LogTrace("BEGIN Cancel. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = "Okay".Tell(true);

      logger.LogTrace("END Cancel. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}