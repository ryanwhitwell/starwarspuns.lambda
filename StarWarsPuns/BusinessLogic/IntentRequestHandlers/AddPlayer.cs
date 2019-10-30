using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Models;
using StarWarsPuns.Core;
using System;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class AddPlayer : BaseRequestHandler<AddPlayer>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.AddPlayer; } }
    
    public AddPlayer(ILogger<AddPlayer> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      string playerName = Configuration.TEXT_INFO.ToTitleCase(intentRequest.Intent.Slots["player"].Value);

      Player existingPlayer = tokenUser.Players.FirstOrDefault(x => x.Name == playerName);

      SkillResponse response;
      if (existingPlayer != null)
      {
        // Don't update any data
        response = string.Format("{0} is already in your list of tokens.", existingPlayer.Name).Tell(true);
      }
      else
      {
        // Add new Player data
        tokenUser.Players.Add(new Player() { Name = playerName });
        response = string.Format("Alright, I added {0} to your list of tokens.", playerName).Tell(true);
      }

      logger.LogTrace("END AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}