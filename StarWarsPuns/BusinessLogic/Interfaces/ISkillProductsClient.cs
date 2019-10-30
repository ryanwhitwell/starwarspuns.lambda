using System.Threading.Tasks;
using Alexa.NET.InSkillPricing;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface ISkillProductsClient
  {
    Task<InSkillProductsResponse> GetProducts();
  }
}