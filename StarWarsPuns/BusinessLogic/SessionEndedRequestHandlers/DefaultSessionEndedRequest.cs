using System;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;
using StarWarsPuns.Models;

namespace StarWarsPuns.BusinessLogic.LaunchRequestHandlers
{
  public class DefaultSessionEndedRequest : BaseRequestHandler<DefaultSessionEndedRequest>, ISessionEndedRequestHandler
  {
    public string HandlerName { get { return SessionEndedRequestName.Default; } }
    
    public DefaultSessionEndedRequest(ILogger<DefaultSessionEndedRequest> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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

      logger.LogTrace("BEGIN Default. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = string.Format("Hmm, alright.").Tell(true);

      logger.LogTrace("END Default. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}