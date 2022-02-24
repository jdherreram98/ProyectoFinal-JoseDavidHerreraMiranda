using Microsoft.Azure.Cosmos;
using ProyectoFinal_JoseDavidHerreraMiranda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDBApi.Models
{

    public interface ICosmosDbService
    {
        Task<IEnumerable<Product>> GetProductsAsync(string query);
        Task<Product> GetProductAsync(string id);
        Task AddProductAsync(Product item);
        Task UpdateProductAsync(string id, Product item);
        Task DeleteProductAsync(string id);

    }

    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddProductAsync(Product item)
        {
            await _container.CreateItemAsync<Product>(item, new PartitionKey(item.Id));
        }
        public async Task DeleteProductAsync(string id)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(id));
        }
        public async Task<Product> GetProductAsync(string id)
        {
            try
            {
                ItemResponse<Product> response = await _container.ReadItemAsync<Product>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Product>(new QueryDefinition(queryString));
            List<Product> results = new List<Product>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateProductAsync(string id, Product item)
        {
            await _container.UpsertItemAsync<Product>(item, new PartitionKey(id));
        }
    }
}
