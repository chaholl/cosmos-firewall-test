using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;

namespace Cat.Test
{
    public static class TimerTriggerPollCosmos
    {
        [FunctionName("TimerTriggerPollCosmos")]
        public static async void RunAsync([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("CosmosConnectionString");
                string databaseId = Environment.GetEnvironmentVariable("CosmosDatabase");

                var cosmosClient = new CosmosClient(connectionString);

                var createResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
                log.LogInformation("Created Database: {0}\n", createResponse.Database.Id);

                var deleteResponse = await createResponse.Database.DeleteAsync();
                log.LogInformation("Deleted Database: {0}\n", deleteResponse.Database.Id);

                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Unable to connect to Cosmos");
            }

        }
    }


}
