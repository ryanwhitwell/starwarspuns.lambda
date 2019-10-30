using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;

namespace StarWarsPuns.Models
{
  [JsonObject]
  public class UserProfile
  {
    [JsonProperty("user_id")]
    public string Id { get; set; }
  }
}