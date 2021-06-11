using SodaMachine.Models;
using System.Collections.Generic;
using System.Linq;

namespace SodaMachine.Data
{
    public class SodaInventoryData
    {
        public List<SodaModel> SodaInventory = new List<SodaModel>()
        { 
            new SodaModel { Name = "coke", Price = 20, InventoryAmount = 5 },
            new SodaModel { Name = "sprite", Price = 15, InventoryAmount = 3 },
            new SodaModel { Name = "fanta", Price = 15, InventoryAmount = 3 }
        };

        public SodaModel Get(string sodaName)
        {
            return SodaInventory.FirstOrDefault(x => x.Name == sodaName);
        }

    }
}
