using Microsoft.Extensions.Logging;
using StarWarsPuns.Core;
using System.Collections.Generic;
using StarWarsPuns.BusinessLogic.Interfaces;

namespace StarWarsPuns.BusinessLogic
{
  public class SessionEndedRequestRouter : BaseRequestRouter<SessionEndedRequestRouter>
  {
    public SessionEndedRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<SessionEndedRequestRouter> logger, IEnumerable<ISessionEndedRequestHandler> requestHandlers) : base(RequestType.SessionEndedRequest, skillRequestValidator, logger, requestHandlers) { }
  }
}