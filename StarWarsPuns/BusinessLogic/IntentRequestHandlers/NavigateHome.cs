using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Models;
using StarWarsPuns.Core;
using System;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class NavigateHome : BaseRequestHandler<NavigateHome>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.NavigateHome; } }
    
    public NavigateHome(ILogger<NavigateHome> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN NavigateHome. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = "Done".Tell(true);

      logger.LogTrace("END NavigateHome. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}