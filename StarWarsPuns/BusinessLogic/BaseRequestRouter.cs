
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using System;
using Microsoft.Extensions.Logging;
using StarWarsPuns.Core;
using System.Collections.Generic;
using System.Linq;
using StarWarsPuns.BusinessLogic.Interfaces;

namespace StarWarsPuns.BusinessLogic
{
  public abstract class BaseRequestRouter<T> : IRequestRouter
  {
    private IEnumerable<IBaseRequestHandler> requestHandlers;

    private ILogger<T> logger;

    private RequestType requestType;

    public RequestType RequestType { get { return this.requestType; }}

    public IEnumerable<IBaseRequestHandler> RequestHandlers { get { return this.requestHandlers; }}

    public ILogger<T> Logger { get { return this.logger; }}

    public BaseRequestRouter(RequestType requestType, ILogger<T> logger, IEnumerable<IBaseRequestHandler> requestHandlers)
    {
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      if (requestHandlers == null || requestHandlers.Count() <= 0)
      {
        throw new ArgumentNullException("requestHandlers");
      }

      this.requestType = requestType;
      this.logger = logger;
      this.requestHandlers = requestHandlers;
    }

    public virtual IBaseRequestHandler GetRequestHandler()
    {
      return requestHandlers.FirstOrDefault();
    }

    public virtual async Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest)
    {
      this.logger.LogTrace("BEGIN GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      IBaseRequestHandler requestHandler = this.GetRequestHandler();

      if (requestHandler == null)
      {
        throw new Exception(string.Format("Cannot successfully route request. The request handler is not bound to the container.", skillRequest.Request.Type));
      }

      // Handle the request
      SkillResponse skillResponse = await Task.Run(() => requestHandler.Handle(skillRequest));

      this.logger.LogTrace("END GetSkillResponse. RequestId: {0}.", skillRequest.Request.RequestId);

      return skillResponse;
    }
  }
}