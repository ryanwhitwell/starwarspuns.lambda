using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StarWarsPuns.BusinessLogic.Interfaces;

namespace StarWarsPuns.BusinessLogic
{
  public class RequestBusinessLogic : IRequestBusinessLogic
  {
    private ILogger<RequestBusinessLogic> logger;
    private IRequestMapper requestMapper;
    
    public RequestBusinessLogic(ILogger<RequestBusinessLogic> logger, IRequestMapper requestMapper)
    {
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }

      if (requestMapper == null)
      {
        throw new ArgumentNullException("requestMapper");
      }

      this.logger = logger;
      this.requestMapper = requestMapper;
    }

    public async Task<SkillResponse> HandleSkillRequest(SkillRequest skillRequest, ILambdaContext lambdaContext)
    {
      if (lambdaContext == null)
      {
        throw new ArgumentNullException("lambdaContext");
      }
      
      this.logger.LogTrace("BEGIN Handling request type '{0}'. RequestId: {1}.", skillRequest.Request.Type, skillRequest.Request.RequestId);

      // Determine the correct handler
      IRequestRouter requestHandler = this.requestMapper.GetRequestHandler(skillRequest);

      // Handle the request
      SkillResponse response = response = await requestHandler.GetSkillResponse(skillRequest);
      
      this.logger.LogTrace("END Handling request type '{0}'. RequestId: {1}. Response: {2}", skillRequest.Request.Type, skillRequest.Request.RequestId, JsonConvert.SerializeObject(response));

      return response;
    }
  }
}