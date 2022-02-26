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

        /// <summary>
        /// view list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                return View((await _cosmosDbService.GetMachinesAsync("SELECT * FROM c WHERE c.type = 'Machine' AND c.active = true")).ToList());
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
        /// <param name="Machine"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateAsync([Bind("Name,CostByHour,ProductsByHour,HoursToBeRepared,State,Active")] Machine Machine)
        {
            try
            {
                Machine.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddMachineAsync(Machine);
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

            Machine Machine = await _cosmosDbService.GetMachineAsync(id);

            if (Machine == null)
                return NotFound();

            return View(Machine);
        }

        /// <summary>
        /// edit action
        /// </summary>
        /// <param name="Machine"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditAsync([Bind("Id,Name,CostByHour,ProductsByHour,HoursToBeRepared,CreatedDate,State,Active")] Machine Machine)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateMachineAsync(Machine.Id, Machine);
                return RedirectToAction("Index");
            }
            return View(Machine);
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

            Machine Machine = await _cosmosDbService.GetMachineAsync(id);

            if (Machine == null)
                return NotFound();

            return View(Machine);
        }

        /// <summary>
        /// delete action
        /// </summary>
        /// <param name="Machine"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DeleteAsync([Bind("Id,Name,CostByHour,ProductsByHour,HoursToBeRepared,State,Active")] Machine Machine)
        {
            if (ModelState.IsValid)
            {
                Machine.Active = false;
                await _cosmosDbService.UpdateMachineAsync(Machine.Id, Machine);
                return RedirectToAction("Index");
            }
            return View(Machine);
        }

        /// <summary>
        /// view details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(string id)
        {
            return View(await _cosmosDbService.GetMachineAsync(id));
        }
    }
}
