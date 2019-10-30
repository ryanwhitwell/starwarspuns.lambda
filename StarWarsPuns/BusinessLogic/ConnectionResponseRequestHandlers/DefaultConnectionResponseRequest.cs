using System;
using Alexa.NET.InSkillPricing.Responses;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Core;
using StarWarsPuns.Data;
using StarWarsPuns.Models;

namespace StarWarsPuns.BusinessLogic.ConnectionResponseRequestHandlers
{
  public class DefaultConnectionResponseRequest : BaseRequestHandler<DefaultConnectionResponseRequest>, IConnectionResponseRequestHandler
  {
    public string HandlerName { get { return ConnectionResponseRequestName.Default; } }
    
    public DefaultConnectionResponseRequest(ILogger<DefaultConnectionResponseRequest> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      ConnectionResponseRequest request = skillRequest.Request as ConnectionResponseRequest;
      ConnectionResponsePayload payload = request.Payload as ConnectionResponsePayload;

      if (payload == null)
      {
        throw new ArgumentNullException("payload");
      }

      if (string.IsNullOrWhiteSpace(payload.PurchaseResult))
      {
        throw new ArgumentNullException("purchaseResult");
      }

      SkillResponse response;
      switch(payload.PurchaseResult)
      {
        case PurchaseResult.Accepted:
        case PurchaseResult.AlreadyPurchased:
          tokenUser.HasPointsPersistence = true;
          tokenUser.UpsellTicks = 0;
          response = string.Format("Your tokens are available while your subscription is active. To get started, add a token by saying something like, add the color blue.").Tell(false);
          break;
        case PurchaseResult.Declined:
           response = string.Format("Don't forget to subscribe so you can keep your tokens forever.").Tell(true);
           break;
        case PurchaseResult.Error:
          response = string.Format("Please try again.").Tell(true);
          logger.LogError(string.Format("An error occurred while a user was attempting to purchase a product. User Id: {0}, Product Id: {1}, ConnectionResponsePayload: {2}.", tokenUser.Id,  Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Id"], JsonConvert.SerializeObject(payload)));
          break;
        default:
          throw new NotSupportedException(string.Format("PurchaseResult '{0}' is not supported.", payload.PurchaseResult));
      }

      logger.LogTrace("END Default. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }

}