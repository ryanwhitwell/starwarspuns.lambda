
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System;
using Microsoft.Extensions.Logging;
using StarWarsPuns.Core;
using System.Collections.Generic;
using System.Linq;
using StarWarsPuns.BusinessLogic.Interfaces;

namespace StarWarsPuns.BusinessLogic
{
  public class IntentRequestRouter : BaseRequestRouter<IntentRequestRouter>
  {
    public IntentRequestRouter(ILogger<IntentRequestRouter> logger, IEnumerable<IIntentRequestHandler> intentRequestHandlers) : base(RequestType.IntentRequest, logger, intentRequestHandlers) { }

    public override async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest)
    {
      base.Logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      IntentRequest intentRequest = skillRequest.Request as IntentRequest;

      if (intentRequest.Intent.ConfirmationStatus == "DENIED")
      {
        return string.Format("Okay").Tell(true);
      }

      // Get the right handler for the IntentRequest based on the name of the intent
      IIntentRequestHandler requestHandler = base.RequestHandlers.Where(x => x.HandlerName == intentRequest.Intent.Name).FirstOrDefault() as IIntentRequestHandler;

      if (requestHandler == null)
      {
        throw new NotSupportedException(string.Format("Cannot successfully route IntentRequest '{0}'.", intentRequest.Intent.Name));
      }

      // Handle the request
      SkillResponse skillResponse = await Task.Run(() => requestHandler.Handle(skillRequest));

      base.Logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      return skillResponse;
    }
  }
}