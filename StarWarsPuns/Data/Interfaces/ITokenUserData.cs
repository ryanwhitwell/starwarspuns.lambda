using System.Threading.Tasks;
using StarWarsPuns.Models;

namespace StarWarsPuns.Data.Interfaces
{
  public interface ITokenUserData
  {
    Task Save(TokenUser tokenUser);
    Task<TokenUser> Get(string id);
    Task Delete(string id);
    Task<bool> Exists(string id);
  }
}
