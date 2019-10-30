using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Microsoft.Extensions.Logging;
using StarWarsPuns.BusinessLogic.Interfaces;
using StarWarsPuns.Models;
using StarWarsPuns.Core;
using System.Text;
using System;

namespace StarWarsPuns.BusinessLogic.IntentRequestHandlers
{
  public class WhatCanIBuy : BaseRequestHandler<WhatCanIBuy>, IIntentRequestHandler
  {
    public string HandlerName { get { return IntentRequestName.WhatCanIBuy; } }
    
    public WhatCanIBuy(ILogger<WhatCanIBuy> logger, ISkillRequestValidator skillRequestValidator) : base(logger, skillRequestValidator) { }

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
      
      logger.LogTrace("BEGIN WhatCanIBuy. RequestId: {0}.", skillRequest.Request.RequestId);

      SkillResponse response = string.Format("You can subscribe to {0}. {0} allows you to store all of your tokens forever. If you would like to subscribe, you can say something like, purchase {0}.", 
      Configuration.File.GetSection("InSkillProducts").GetSection("PointsPersistence")["Name"])
      .Tell(false);
      
      logger.LogTrace("END WhatCanIBuy. RequestId: {0}.", skillRequest.Request.RequestId);

      return response;
    }
  }
}