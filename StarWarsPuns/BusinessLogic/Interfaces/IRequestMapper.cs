using Alexa.NET.Request;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface IRequestMapper
  {
    IRequestRouter GetRequestHandler(SkillRequest skillRequest);
  }
}