using System.Threading.Tasks;
using StarWarsPuns.Models;

namespace StarWarsPuns.Data.Interfaces
{
  public interface IStarWarsPunRepository
  {
    Task<StarWarsPun> Load(int id);
    Task<long> Count();
  }
}
