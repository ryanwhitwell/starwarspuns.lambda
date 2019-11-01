using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Models;
using StarWarsPuns.Core;
using System;
using StarWarsPuns.Data.Interfaces;
using System.Threading.Tasks;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class GetNewPun : BaseRequestHandler<GetNewPun>, IIntentRequestHandler
  {
    private IStarWarsPunRepository _repository;
    
    public string HandlerName { get { return IntentRequestName.GetNewPun; } }
    
    public GetNewPun(ILogger<GetNewPun> logger, IStarWarsPunRepository repository) : base(logger) 
    { 
      if (repository == null)
      {
        throw new ArgumentNullException("repository");
      }

      _repository = repository;
    }

    public SkillResponse Handle(SkillRequest skillRequest)
    {
      logger.LogTrace("BEGIN AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      long itemCount = Task.Run(async () => await _repository.Count()).Result;

      Random random = new Random();
      int randomId = random.Next(0, (Int32)itemCount);

      StarWarsPun pun = Task.Run(async () => await _repository.Load(randomId)).Result;

      SkillResponse response = string.Format("{0}<break time=\"3s\"/>{1}", pun.Question, pun.Answer).Tell(true);

      logger.LogTrace("END AddPlayer. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}