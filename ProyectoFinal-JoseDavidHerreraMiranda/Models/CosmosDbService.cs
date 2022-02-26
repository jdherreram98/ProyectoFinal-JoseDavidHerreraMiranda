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

        #region machine interface methods

        Task<IEnumerable<Machine>> GetMachinesAsync(string query);
        Task<Machine> GetMachineAsync(string id);
        Task AddMachineAsync(Machine item);
        Task UpdateMachineAsync(string id, Machine item);

        #endregion

        #region simulation interface methods

        Task<IEnumerable<Simulation>> GetSimulationsAsync(string query);
        Task<Simulation> GetSimulationAsync(string id);
        Task AddSimulationAsync(Simulation item);
        Task AddSimulationReportAsync(SimulationReport item);
        Task UpdateSimulationAsync(string id, Simulation item);

        #endregion

    }

    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        #region machine db methods

        /// <summary>
        /// add machine into the db
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddMachineAsync(Machine item)
        {
            var date = DateTime.Now;
            item.Type = "Machine";
            item.CreatedDate = date;
            item.LastModifiedDate = date;
            await _container.CreateItemAsync<Machine>(item, new PartitionKey(item.Id));
        }

        /// <summary>
        /// update machine by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateMachineAsync(string id, Machine item)
        {
            item.LastModifiedDate = DateTime.Now;
            item.Type = "Machine";
            await _container.UpsertItemAsync<Machine>(item, new PartitionKey(id));
        }

        /// <summary>
        /// get a specific machine by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// get list of active machines
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
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

        #region simulation db methods

        /// <summary>
        /// add simulation into the db
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddSimulationAsync(Simulation item)
        {
            var date = DateTime.Now;
            item.Type = "Simulation";
            item.CreatedDate = date;
            item.LastModifiedDate = date;
            await _container.CreateItemAsync<Simulation>(item, new PartitionKey(item.Id));
        }

        /// <summary>
        /// update simulation by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateSimulationAsync(string id, Simulation item)
        {
            item.Type = "Simulation";
            item.LastModifiedDate = DateTime.Now;
            await _container.UpsertItemAsync<Simulation>(item, new PartitionKey(id));
        }

        /// <summary>
        /// get a specific simulation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// get list of active simulations
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
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

        /// <summary>
        /// add simulationreport on execute
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddSimulationReportAsync(SimulationReport item)
        {
            var date = DateTime.Now;
            item.Type = "SimulationReport";
            item.CreatedDate = date;
            await _container.CreateItemAsync<SimulationReport>(item, new PartitionKey(item.Id));
        }

        #endregion


    }
}
