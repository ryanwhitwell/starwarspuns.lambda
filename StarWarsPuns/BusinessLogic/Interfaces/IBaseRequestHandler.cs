using Alexa.NET.Request;
using Alexa.NET.Response;
using StarWarsPuns.Models;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface IBaseRequestHandler
  {
    SkillResponse Handle(SkillRequest skillRequest);
    string HandlerName { get; }
  }
}