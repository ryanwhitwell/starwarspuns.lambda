using Alexa.NET.Request;

namespace StarWarsPuns.BusinessLogic
{
  public interface ISkillRequestValidator
  {
    bool IsValid(SkillRequest skillRequest);
  }
}