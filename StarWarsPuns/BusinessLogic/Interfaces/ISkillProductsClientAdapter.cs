using Alexa.NET.Request;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface ISkillProductsClientAdapter
  {
    ISkillProductsClient GetClient(SkillRequest skillRequest);
  }
}