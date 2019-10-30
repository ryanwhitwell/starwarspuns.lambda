using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace StarWarsPunData.Main
{
  public class TableOperator
  {
    private AmazonDynamoDBClient client;
    private string tableName;
    
    public TableOperator(AmazonDynamoDBClient client, string tableName)
    {
      if (client == null)
      {
        throw new ArgumentNullException("client");
      }

      if (String.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException("tableName");
      }

      this.client = client;
      this.tableName = tableName;
    }

    private async Task WaitUntilTableReady()
    {
      string status = null;
      do
      {
        System.Threading.Thread.Sleep(5000); // Wait 5 seconds.
        try
        {
          DescribeTableResponse res = await client.DescribeTableAsync(new DescribeTableRequest
          {
            TableName = tableName
          });

          Console.WriteLine("Table name: {0}, status: {1}",
            res.Table.TableName,
            res.Table.TableStatus);
          
          status = res.Table.TableStatus;
        }
        catch (ResourceNotFoundException ex)
        {
          Console.WriteLine(ex);
        }
      } while (status != "ACTIVE");
    }

    public async Task AddItems<T>(IEnumerable<T> items)
    {
      IDynamoDBContext context = new DynamoDBContext(client, new DynamoDBContextConfig() { ConsistentRead = true });
      BatchWrite<T> batch = context.CreateBatchWrite<T>();

      batch.AddPutItems(items);

      await batch.ExecuteAsync();
    }

    public async Task CreateTable()
    {
      
      Console.WriteLine("\n*** Creating table {0} ***", tableName);
      CreateTableRequest request = new CreateTableRequest
      {
        AttributeDefinitions = new List<AttributeDefinition>()
        {
          new AttributeDefinition
          {
            AttributeName = "Id",
            AttributeType = "N"
          }
        },
        KeySchema = new List<KeySchemaElement>
        {
          new KeySchemaElement
          {
            AttributeName = "Id",
            KeyType = "HASH"
          }
        },
        ProvisionedThroughput = new ProvisionedThroughput
        {
          ReadCapacityUnits = 5,
          WriteCapacityUnits = 5
        },
        TableName = tableName
      };

      int s = 0;
      do
      {
        try
        {
          CreateTableResponse response = await client.CreateTableAsync(request);
          TableDescription tableDescription = response.TableDescription;
          Console.WriteLine("{1}: {0} \t ReadsPerSec: {2} \t WritesPerSec: {3}",
                    tableDescription.TableStatus,
                    tableDescription.TableName,
                    tableDescription.ProvisionedThroughput.ReadCapacityUnits,
                    tableDescription.ProvisionedThroughput.WriteCapacityUnits);

          string status = tableDescription.TableStatus;
          Console.WriteLine(tableName + " - " + status);

          await this.WaitUntilTableReady();
          s++;
        }
        catch (Exception ex)
        {
          if (ex is ResourceInUseException)
          {
            System.Threading.Thread.Sleep(5000);
            continue;
          }

          s++;
        }

      } while (s <= 0);
    }

    public async Task DeleteTable()
    {
      try
      {
        Console.WriteLine("\n*** Deleting table {0} ***", tableName);
        DeleteTableRequest request = new DeleteTableRequest
        {
          TableName = tableName
        };

        DeleteTableResponse response = await client.DeleteTableAsync(request);

        Console.WriteLine("Table {0} is being deleted...", tableName);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}