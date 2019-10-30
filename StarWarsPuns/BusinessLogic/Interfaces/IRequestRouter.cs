using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using StarWarsPuns.Core;
using StarWarsPuns.Models;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface IRequestRouter
  {
    Task<SkillResponse> GetSkillResponse(SkillRequest skillRequest, TokenUser tokenUser);
    RequestType RequestType { get; }
  }
}