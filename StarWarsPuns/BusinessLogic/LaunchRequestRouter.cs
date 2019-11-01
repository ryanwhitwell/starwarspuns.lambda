using Microsoft.Extensions.Logging;
using StarWarsPuns.Core;
using System.Collections.Generic;
using StarWarsPuns.BusinessLogic.Interfaces;

namespace StarWarsPuns.BusinessLogic
{
  public class LaunchRequestRouter : BaseRequestRouter<LaunchRequestRouter>
  {
    public LaunchRequestRouter(ILogger<LaunchRequestRouter> logger, IEnumerable<ILaunchRequestHandler> requestHandlers) : base(RequestType.LaunchRequest, logger, requestHandlers) { }
  }
}