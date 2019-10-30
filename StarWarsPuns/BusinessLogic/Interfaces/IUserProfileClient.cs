using System.Threading.Tasks;

namespace StarWarsPuns.BusinessLogic.Interfaces
{
  public interface IUserProfileClient
  {
    Task<string> GetUserId(string accessToken);
  }
}