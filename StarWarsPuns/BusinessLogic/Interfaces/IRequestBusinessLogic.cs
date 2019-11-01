using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface IRequestBusinessLogic
  {
    Task<SkillResponse> HandleSkillRequest(SkillRequest skillRequest, ILambdaContext context);
  }
}