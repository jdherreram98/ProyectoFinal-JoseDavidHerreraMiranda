using CosmosDBApi.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_JoseDavidHerreraMiranda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_JoseDavidHerreraMiranda.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;

        public ProductController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet]
        [Route("getProducts")]
        public async Task<List<Product>> Index()
        {
            try
            {
                return (await _cosmosDbService.GetProductsAsync("SELECT * FROM c WHERE c.type = 'Product' AND c.active = true")).ToList();
            }
            catch
            {
                throw;
            }
        }


        [HttpPost]
        [Route("addProduct")]
        public async Task CreateAsync([Bind("Type,Name,Price,Active")] Product Product)
        {
            try
            {
                Product.Id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddProductAsync(Product);
            }
            catch
            {
                throw;
            }
        }

        [ActionName("viewProductEdit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
                return BadRequest();

            Product Product = await _cosmosDbService.GetProductAsync(id);

            if (Product == null)
                return NotFound();

            return View(Product);
        }

        [HttpPost]
        [Route("updateProduct")]
        public async Task<ActionResult> EditAsync([Bind("Id")] Product Product)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateProductAsync(Product.Id, Product);
                return RedirectToAction("Index");
            }
            return View(Product);
        }

        [HttpPost]
        [ActionName("deleteProduct")]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] Product Product)
        {
            if (ModelState.IsValid)
            {
                Product.Active = false;
                await _cosmosDbService.UpdateProductAsync(Product.Id, Product);
                return RedirectToAction("Index");
            }
            return View(Product);
        }

        [ActionName("getProductDetails")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            return View(await _cosmosDbService.GetProductAsync(id));
        }
    }
}
