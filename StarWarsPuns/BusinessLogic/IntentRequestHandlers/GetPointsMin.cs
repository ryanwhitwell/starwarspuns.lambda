using System;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Models;
using StarWarsPuns.Core;
using System.Text;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class GetPointsMin : BaseRequestHandler<GetPointsMin>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.GetPointsMin; } }
    
    public GetPointsMin(ILogger<GetPointsMin> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN GetPointsMin. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      Player[] playersScoreAscending = tokenUser.Players.OrderBy(x => x.Points).ToArray();

      SkillResponse response;

      if (playersScoreAscending == null || playersScoreAscending.Length <= 0)
      {
        response = string.Format("Hmm, you don't see anyone in your list of tokens.").Tell(true);
      }
      else
      {
        int lowScore = playersScoreAscending[0].Points;
        Player[] lowScorePlayers = playersScoreAscending.Where(x => x.Points == lowScore).ToArray();

        if (lowScorePlayers.Length == playersScoreAscending.Count())
        {
          string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
          response = string.Format("All tokens are tied with a score of {0} {1}.", lowScore, pointsWord).Tell(true);
        }
        else if (lowScorePlayers.Length > 1)
        {
          StringBuilder responsePhraseBuilder = new StringBuilder();

          for (int i = 0; i < lowScorePlayers.Count(); i++)
          {
            Player currentPlayer = lowScorePlayers[i];
            if (i == 0)
            {
              responsePhraseBuilder.AppendFormat("{0}", currentPlayer.Name);
              continue;
            }

            responsePhraseBuilder.AppendFormat(" and {0}", currentPlayer.Name);
          }

          string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
          responsePhraseBuilder.AppendFormat(" are tied for the lowest score with {0} {1}.", lowScore, pointsWord);

          response = responsePhraseBuilder.ToString().Tell(true);
        }
        else
        {
          string pointsWord = Math.Abs(lowScore) != 1 ? "points" : "point";
          response = string.Format("{0} has the lowest score with {1} {2}.", lowScorePlayers[0].Name, lowScore, pointsWord).Tell(true);
        }
      }

      logger.LogTrace("END GetPointsMin. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}