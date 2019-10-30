using System.Threading.Tasks;
using StarWarsPuns.Models;

namespace StarWarsPuns.Data.Interfaces
{
  public interface ITokenUserRepository
  {
    Task Save(TokenUser tokenUser);
    Task<TokenUser> Load(string id);
    Task Delete(string id);
  }
}
