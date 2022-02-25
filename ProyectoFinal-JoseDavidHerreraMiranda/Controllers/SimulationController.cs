using CosmosDBApi.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_JoseDavidHerreraMiranda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_JoseDavidHerreraMiranda.Controllers
{
    public class SimulationController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;

        public SimulationController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        [Route("getSimulations")]
        public async Task<List<Simulation>> Index()
        {
            try
            {
                return (await _cosmosDbService.GetSimulationsAsync("SELECT * FROM c WHERE c.type = 'Simulation' AND c.active = true")).ToList();
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("addSimulation")]
        public async Task CreateAsync([Bind("Description,DaysByWeek,Completed,Active")] Simulation Simulation)
        {
            try
            {
                Simulation.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddSimulationAsync(Simulation);
            }
            catch
            {
                throw;
            }
        }

        [ActionName("viewSimulationEdit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
                return BadRequest();

            Simulation Simulation = await _cosmosDbService.GetSimulationAsync(id);

            if (Simulation == null)
                return NotFound();

            return View(Simulation);
        }

        [HttpPost]
        [Route("updateSimulation")]
        public async Task<ActionResult> EditAsync([Bind("Id")] Simulation Simulation)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateSimulationAsync(Simulation.Id, Simulation);
                return RedirectToAction("Index");
            }
            return View(Simulation);
        }

        [HttpPost]
        [ActionName("deleteSimulation")]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] Simulation Simulation)
        {
            if (ModelState.IsValid)
            {
                Simulation.Active = false;
                await _cosmosDbService.UpdateSimulationAsync(Simulation.Id, Simulation);
                return RedirectToAction("Index");
            }
            return View(Simulation);
        }

        [ActionName("getSimulationDetails")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetSimulationAsync(id));
        }
    }
}
