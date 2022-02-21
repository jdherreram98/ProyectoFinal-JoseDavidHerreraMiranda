using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDBApi.Models
{

    public interface ICosmosDbService
    {
        Task<IEnumerable<Items>> GetItemsAsync(string query);
        Task<Items> GetItemAsync(string id);
        Task AddItemAsync(Items item);
        Task UpdateItemsAsync(string id, Items item);
        Task DeleteItemAsync(string id);

    }

    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Items item)
        {
            await _container.CreateItemAsync<Items>(item, new PartitionKey(item.Id));
        }
        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<Items>(id, new PartitionKey(id));
        }
        public async Task<Items> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Items> response = await _container.ReadItemAsync<Items>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return null;
            }
        }

        public async Task<IEnumerable<Items>> GetItemsAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Items>(new QueryDefinition(queryString));
            List<Items> results = new List<Items>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateItemsAsync(string id, Items item)
        {
            await _container.UpsertItemAsync<Items>(item, new PartitionKey(id));
        }
    }
}
