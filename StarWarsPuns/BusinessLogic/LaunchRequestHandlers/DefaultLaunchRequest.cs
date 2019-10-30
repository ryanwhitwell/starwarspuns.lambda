using System;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;
using StarWarsPuns.Models;

namespace StarWarsPuns.BusinessLogic.LaunchRequestHandlers
{
  public class DefaultLaunchRequest : BaseRequestHandler<DefaultLaunchRequest>, ILaunchRequestHandler
  {
    public string HandlerName { get { return LaunchRequestName.Default; } }
    
    public DefaultLaunchRequest(ILogger<DefaultLaunchRequest> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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

      SkillResponse response;
      if (tokenUser.HasPointsPersistence)
      {
        response = string.Format("Welcome to {0}. You currently have Points Persistence so your tokens are available for as long as your subscription is active. " +
          "To add a new token you can say something like, <emphasis>add</emphasis> the color blue, or to add points to an existing token, you can say something like, " +
          "<emphasis>add</emphasis> three points to red. So, what can I help you with?", 
          Configuration.File.GetSection("Application")["SkillName"])
          .Tell(false);
      }
      else
      {
        response = string.Format("Welcome to {0}. Your tokens will only be availble {1} without a subscription to {2}. " +
          "To add a new token you can say something like, <emphasis>add</emphasis> the color blue, or to add points to an existing token, " +
          "you can say something like, <emphasis>add</emphasis> three points to red. So, what can I help you with?", 
          Configuration.File.GetSection("Application")["SkillName"], 
          tokenUser.TTLPhrase(),
          Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Name"])
          .Tell(false);
      }

      logger.LogTrace("END Default. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}