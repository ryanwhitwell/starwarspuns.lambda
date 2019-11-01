using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using StarWarsPuns.Data.Interfaces;
using StarWarsPuns.Models;
using StarWarsPuns.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace StarWarsPuns.Data
{
  public class StarWarsPunRepository : IStarWarsPunRepository
  {
    private static readonly string TABLE_NAME = Configuration.File.GetSection("Application")["DynamoDBTableName"];
    
    private IDynamoDBContext _context;
    private IAmazonDynamoDB _client;

    public StarWarsPunRepository(IDynamoDBContext context, IAmazonDynamoDB client)
    {
      _context = context ?? throw new ArgumentNullException("context");
      _client = client ?? throw new ArgumentNullException("client");
    }

    public async Task<StarWarsPun> Load(int id)
    {
      if (id < 0)
      {
        throw new ArgumentOutOfRangeException("id");
      }

      StarWarsPun pun = await _context.LoadAsync<StarWarsPun>(id);

      return pun;
    }

    public async Task<long> Count()
    {
      DescribeTableResponse response = await _client.DescribeTableAsync(TABLE_NAME);
      return response == null || response.Table == null ? 0 : response.Table.ItemCount;
    }
  }
}