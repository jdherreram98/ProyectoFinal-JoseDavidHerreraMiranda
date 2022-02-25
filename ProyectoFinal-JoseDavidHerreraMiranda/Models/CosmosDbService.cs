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
        #region product interface methods

        Task<IEnumerable<Product>> GetProductsAsync(string query);
        Task<Product> GetProductAsync(string id);
        Task AddProductAsync(Product item);
        Task UpdateProductAsync(string id, Product item);
        Task DeleteProductAsync(string id);

        #endregion

        #region simulation interface methods

        Task<IEnumerable<Simulation>> GetSimulationsAsync(string query);
        Task<Simulation> GetSimulationAsync(string id);
        Task AddSimulationAsync(Simulation item);
        Task UpdateSimulationAsync(string id, Simulation item);
        Task DeleteSimulationAsync(string id);

        #endregion

        #region machine interface methods

        Task<IEnumerable<Machine>> GetMachinesAsync(string query);
        Task<Machine> GetMachineAsync(string id);
        Task AddMachineAsync(Machine item);
        Task UpdateMachineAsync(string id, Machine item);
        Task DeleteMachineAsync(string id);

        #endregion
    }

    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        #region product db methods

        public async Task AddProductAsync(Product item)
        {
            item.Type = "Product";
            item.CreatedDate = DateTime.Now;
            item.LastModifiedDate = DateTime.Now;
            await _container.CreateItemAsync<Product>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteProductAsync(string id)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(id));
        }

        public async Task UpdateProductAsync(string id, Product item)
        {
            item.Type = "Product";
            item.LastModifiedDate = DateTime.Now;
            await _container.UpsertItemAsync<Product>(item, new PartitionKey(id));
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

        #endregion

        #region simulation db methods

        public async Task AddSimulationAsync(Simulation item)
        {
            item.Type = "Simulation";
            item.CreatedDate = DateTime.Now;
            item.LastModifiedDate = DateTime.Now;
            await _container.CreateItemAsync<Simulation>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteSimulationAsync(string id)
        {
            await _container.DeleteItemAsync<Simulation>(id, new PartitionKey(id));
        }

        public async Task UpdateSimulationAsync(string id, Simulation item)
        {
            item.Type = "Simulation";
            item.LastModifiedDate = DateTime.Now;
            await _container.UpsertItemAsync<Simulation>(item, new PartitionKey(id));
        }

        public async Task<Simulation> GetSimulationAsync(string id)
        {
            try
            {
                ItemResponse<Simulation> response = await _container.ReadItemAsync<Simulation>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return null;
            }
        }

        public async Task<IEnumerable<Simulation>> GetSimulationsAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Simulation>(new QueryDefinition(queryString));
            List<Simulation> results = new List<Simulation>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        #endregion

        #region machine db methods

        public async Task AddMachineAsync(Machine item)
        {
            item.Type = "Machine";
            item.CreatedDate = DateTime.Now;
            item.LastModifiedDate = DateTime.Now;
            await _container.CreateItemAsync<Machine>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteMachineAsync(string id)
        {
            await _container.DeleteItemAsync<Machine>(id, new PartitionKey(id));
        }

        public async Task UpdateMachineAsync(string id, Machine item)
        {
            item.LastModifiedDate = DateTime.Now;
            item.Type = "Machine";
            await _container.UpsertItemAsync<Machine>(item, new PartitionKey(id));
        }

        public async Task<Machine> GetMachineAsync(string id)
        {
            try
            {
                ItemResponse<Machine> response = await _container.ReadItemAsync<Machine>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return null;
            }
        }

        public async Task<IEnumerable<Machine>> GetMachinesAsync(string queryString)
        {
            var query = _container.GetItemQueryIterator<Machine>(new QueryDefinition(queryString));
            List<Machine> results = new List<Machine>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        #endregion
    }
}
