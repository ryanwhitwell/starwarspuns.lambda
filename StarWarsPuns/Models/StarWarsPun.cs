using System;
using Amazon.DynamoDBv2.DataModel;

namespace StarWarsPuns.Models
{
  [DynamoDBTable("StarWarsPun")]
  public class StarWarsPun
  {
    public StarWarsPun() { }
    
    public StarWarsPun(int id, string question, string answer)
    {
      if (id < 0)
      {
        throw new ArgumentOutOfRangeException("id");
      }

      if (String.IsNullOrWhiteSpace(question))
      {
        throw new ArgumentNullException("question");
      }

      if (String.IsNullOrWhiteSpace(answer))
      {
        throw new ArgumentNullException("answer");
      }
      
      this.Id = id;
      this.Question = question;
      this.Answer = answer;
    }
    
    [DynamoDBHashKey]
    public int Id { get; set; }

    [DynamoDBProperty]
    public string Question { get; set; }

    [DynamoDBProperty]
    public string Answer { get; set; }
  }
}