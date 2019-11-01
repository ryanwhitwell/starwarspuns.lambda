using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using StarWarsPuns.Core;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface IRequestRouter
  {
    Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest);
    RequestType RequestType { get; }
  }
}