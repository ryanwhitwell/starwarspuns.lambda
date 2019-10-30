
using Microsoft.Extensions.Logging;
using StarWarsPuns.Core;
using System.Collections.Generic;
using StarWarsPuns.BusinessLogic.Interfaces;

namespace StarWarsPuns.BusinessLogic
{
  public class ConnectionResponseRequestRouter : BaseRequestRouter<ConnectionResponseRequestRouter>
  {
    public ConnectionResponseRequestRouter(ISkillRequestValidator skillRequestValidator, ILogger<ConnectionResponseRequestRouter> logger, IEnumerable<IConnectionResponseRequestHandler> requestHandlers) : base(RequestType.ConnectionResponseRequest, skillRequestValidator, logger, requestHandlers) { }
  }
}