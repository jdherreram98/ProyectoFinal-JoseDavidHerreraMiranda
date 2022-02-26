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

        /// <summary>
        /// view list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                return View((await _cosmosDbService.GetSimulationsAsync("SELECT * FROM c WHERE c.type = 'Simulation' AND c.active = true")).ToList());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// view create 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// create action
        /// </summary>
        /// <param name="Simulation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateAsync([Bind("Description,HoursByDay,DaysByWeek,Product,DaysOfSimulation,Completed,Active")] Simulation Simulation)
        {
            try
            {
                Simulation.Id = Guid.NewGuid().ToString();
                Simulation.Product.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddSimulationAsync(Simulation);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// view edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return BadRequest();

            Simulation Simulation = await _cosmosDbService.GetSimulationAsync(id);

            if (Simulation == null)
                return NotFound();

            return View(Simulation);
        }

        /// <summary>
        /// edit action
        /// </summary>
        /// <param name="Simulation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditAsync([Bind("Id,Description,HoursByDay,DaysByWeek,CreatedDate,Product,DaysOfSimulation,Completed,Active")] Simulation Simulation)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateSimulationAsync(Simulation.Id, Simulation);
                return RedirectToAction("Index");
            }
            return View(Simulation);
        }

        /// <summary>
        /// view for delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();

            Simulation Simulation = await _cosmosDbService.GetSimulationAsync(id);

            if (Simulation == null)
                return NotFound();

            return View(Simulation);
        }

        /// <summary>
        /// delete action
        /// </summary>
        /// <param name="Simulation"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeleteAsync([Bind("Id,Description,HoursByDay,DaysByWeek,DaysOfSimulation,Completed,Active")] Simulation Simulation)
        {
            if (ModelState.IsValid)
            {
                Simulation.Active = false;
                await _cosmosDbService.UpdateSimulationAsync(Simulation.Id, Simulation);
                return RedirectToAction("Index");
            }
            return View(Simulation);
        }

        /// <summary>
        /// view details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(string id)
        {
            return View(await _cosmosDbService.GetSimulationAsync(id));
        }

        /// <summary>
        /// view list of simulations not completed ready to execute
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListExecuteSimulation()
        {
            try
            {
                return View((await _cosmosDbService.GetSimulationsAsync("SELECT * FROM c WHERE c.type = 'Simulation' AND c.active = true AND c.completed = false")).ToList());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// view list of simulations completed 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> SimulationReport()
        {
            try
            {
                return View((await _cosmosDbService.GetSimulationsAsync("SELECT * FROM c WHERE c.type = 'Simulation' AND c.active = true AND c.completed = true")).ToList());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// execute simulation 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ExecuteSimulation(string id)
        {
            try
            {
                Simulation simulation = await _cosmosDbService.GetSimulationAsync(id);

                if (simulation == null)
                    return NotFound();

                // insert init of simulationreport
                var simulationReport = new SimulationReport { Id = Guid.NewGuid().ToString(), SimulationId = simulation.Id };

                await _cosmosDbService.AddSimulationReportAsync(simulationReport);

                Random randomClass = new Random();

                int weeks = simulation.DaysOfSimulation / 7;
                int months = simulation.DaysOfSimulation / 30;

                int day = 0;

                do
                {
                    var machines = (await _cosmosDbService.GetMachinesAsync("SELECT * FROM c WHERE c.type = 'Machine' AND c.active = true")).ToList();

                    day++;

                    foreach (var item in machines)
                    {
                        if (item.State)
                        {
                            var failure = randomClass.NextDouble();
                        }
                    }

                } while (day < simulation.DaysOfSimulation);

                return View();
            }
            catch
            {
                throw;
            }
        }
    }
}
