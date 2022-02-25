using CosmosDBApi.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_JoseDavidHerreraMiranda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_JoseDavidHerreraMiranda.Controllers
{
    public class MachineController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;

        public MachineController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        [Route("getMachines")]
        public async Task<ActionResult> Index()
        {
            try
            {
                return View( (await _cosmosDbService.GetMachinesAsync("SELECT * FROM c WHERE c.type = 'Machine' AND c.active = true")).ToList());
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("addMachine")]
        public async Task CreateAsync([Bind("Name,CostByHour,HoursToBeRepared,State,Active")] Machine Machine)
        {
            try
            {
                Machine.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddMachineAsync(Machine);
            }
            catch
            {
                throw;
            }
        }

        [ActionName("viewMachineEdit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
                return BadRequest();

            Machine Machine = await _cosmosDbService.GetMachineAsync(id);

            if (Machine == null)
                return NotFound();

            return View(Machine);
        }

        [HttpPost]
        [Route("updateMachine")]
        public async Task<ActionResult> EditAsync([Bind("Id")] Machine Machine)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateMachineAsync(Machine.Id, Machine);
                return RedirectToAction("Index");
            }
            return View(Machine);
        }        

        [HttpPost]
        [ActionName("deleteMachine")]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] Machine Machine)
        {
            if (ModelState.IsValid)
            {
                Machine.Active = false;
                await _cosmosDbService.UpdateMachineAsync(Machine.Id, Machine);
                return RedirectToAction("Index");
            }
            return View(Machine);
        }

        [ActionName("getMachineDetails")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetMachineAsync(id));
        }
    }
}
