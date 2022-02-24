using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_JoseDavidHerreraMiranda.Models
{
    public class ProductByMachine
    {
        [JsonProperty(PropertyName = "machine")]
        public Machine Machine { get; set; }

        [JsonProperty(PropertyName = "product")]
        public Product Product { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "createdon")]
        public DateTime CreatedOn { get; set; }
    }
}
