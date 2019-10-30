using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using StarWarsPuns.Data.Interfaces;
using StarWarsPuns.Models;

namespace StarWarsPuns.Data
{
  public class TokenUserRepository : ITokenUserRepository
  {
    private IDynamoDBContext _context;

    public TokenUserRepository(IDynamoDBContext context)
    {
      _context = context ?? throw new ArgumentNullException("context");
    }

    public async Task Save(TokenUser user)
    {
      if (user == null)
      {
        throw new ArgumentNullException("user");
      }

      await _context.SaveAsync<TokenUser>(user);
    }

    public async Task Delete(string id)
    {
      if (String.IsNullOrWhiteSpace(id))
      {
        throw new ArgumentNullException("id");
      }

      await _context.DeleteAsync<TokenUser>(id);
    }

    public async Task<TokenUser> Load(string id)
    {
      if (String.IsNullOrWhiteSpace(id))
      {
        throw new ArgumentNullException("id");
      }

      TokenUser user = await _context.LoadAsync<TokenUser>(id);

      return user;
    }
  }
}