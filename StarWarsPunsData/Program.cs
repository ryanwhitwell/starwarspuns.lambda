using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime.CredentialManagement;
using Microsoft.Extensions.Configuration;
using StarWarsPuns.Models;

namespace StarWarsPunData.Main
{
  public class Program
  {
    private static readonly IConfigurationRoot CONFIG = InitializeConfig();
    
    private static IConfigurationRoot InitializeConfig()
    {
      IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

      return builder.Build();
    }

    private static AmazonDynamoDBClient GetDynamoDBClient()
    {
      AWSOptions options = CONFIG.GetAWSOptions();
      SharedCredentialsFile file = new SharedCredentialsFile(options.ProfilesLocation);

      CredentialProfile profile = null;
      if (!file.TryGetProfile(options.Profile, out profile))
      {
        throw new Exception(String.Format("There was a problem locating AWS Profile {0}", options.Profile));
      }

      Console.WriteLine("Using AWS Profile: {0}", profile.Name);

      AmazonDynamoDBClient client = new AmazonDynamoDBClient(profile.Options.AccessKey, profile.Options.SecretKey, options.Region);

      return client;
    }

    public static void Rebuild(TableOperator tableOperator)
    {
      Task.Run(() => tableOperator.DeleteTable()).Wait();
      Task.Run(() => tableOperator.CreateTable()).Wait();
    }

    public static void Populate(TableOperator tableOperator, string filePath)
    {
      List<StarWarsPun> puns = new List<StarWarsPun>();

      puns.Add(new StarWarsPun(0, "Do yo know what?", "Chicken Butt."));

      // Read in puns from file
      // Add puns to list

      Task.Run(() => tableOperator.AddItems(puns)).Wait();
    }

    public static void Main(string[] args)
    {
      Console.WriteLine("Starting Star Wars Puns Data");
      Console.WriteLine("Starting Star Wars Puns Data - Provisioning");

      AmazonDynamoDBClient client = GetDynamoDBClient();
      

      string tableName = CONFIG.GetSection("Application")["TableName"];
      Console.WriteLine(string.Format("Start Working on Table: {0}", tableName));

      TableOperator tableOperator = new TableOperator(client, tableName);

      string command = args[0];
      string path = args.Length == 2 ? args [1] : "";

      switch(command)
      {
        case "rebuild":
        {
          Rebuild(tableOperator);
          break;
        }
        case "populate":
        {
          Populate(tableOperator, path);
          break;
        }
        default: 
          throw new ArgumentOutOfRangeException(command);
      }

      Console.WriteLine(string.Format("Finished Working on Table: {0}", tableName));
      Console.WriteLine("Finished Star Wars Puns Data - Provisioning");
      Console.WriteLine("Closing Star Wars Puns Data");
    }
  }
}
