using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GildedRose.Controllers
{
    [ApiController]
    [Route("api/itemcatalog")]
    public class ItemCatalogController : ControllerBase
    {

        private readonly ILogger<ItemCatalogController> _logger;
        private IItemCatalogDataService _catalogService;

        public ItemCatalogController(ILogger<ItemCatalogController> logger, IItemCatalogDataService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        [HttpGet]
        public List<CatalogItem> Get(DateTime? currentDate)
        {
            return _catalogService.GetItems(currentDate ?? DateTime.Now);
        }

        [HttpGet("{id}")]
        public CatalogItem Get(int id, DateTime? currentDate)
        {
            return _catalogService.GetItem(id, currentDate ?? DateTime.Now);
        }

        [HttpPost]
        public void LoadData(string filename){
            string[] lines = System.IO.File.ReadAllLines(@"C:\Projects\GildedRose\inventory.txt");

            var itemList = new List<CatalogItem>();
            foreach (string line in lines)
            {
                var item = new CatalogItem();
                string[] fields = line.Split(',');
                item.Name = fields.ElementAt(0);
                item.Category = fields.ElementAt(1);
                item.Sellin = Convert.ToInt32(fields.ElementAt(2));
                item.Quality = Convert.ToInt32(fields.ElementAt(3));
                itemList.Add(item);
            }

            _catalogService.LoadTableData(itemList);
        }
    }
}
