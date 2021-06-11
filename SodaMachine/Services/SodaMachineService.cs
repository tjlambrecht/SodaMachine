using SodaMachine.Data;
using System;

namespace SodaMachine.Services
{
    public class SodaMachineService : ISodaMachineService
    {
        private readonly MoneyData _moneyData;
        private readonly SodaInventoryData _sodaInventoryData;

        public SodaMachineService(MoneyData moneyData, SodaInventoryData inventoryData)
        {
            _moneyData = moneyData;
            _sodaInventoryData = inventoryData;
        }

        public void InsertMoney(int amount)
        {
            if (amount < 0)
            {
                Console.WriteLine("The amount needs to be positive");
                return;
            }

            _moneyData.Money += amount;
            Console.WriteLine("Adding " + amount + " to credit");
        }

        public void Order(string sodaName)
        {
            var sodaModel = _sodaInventoryData.Get(sodaName);
            if (sodaModel == null)
            {
                Console.WriteLine("No such soda");
                return;
            }

            if (sodaModel.InventoryAmount <= 0)
            {
                Console.WriteLine($"No {sodaModel.Name} left");
                return;
            }

            if (_moneyData.Money < sodaModel.Price)
            {
                Console.WriteLine("Need " + (sodaModel.Price - _moneyData.Money) + " more");
                return;
            }

            Console.WriteLine($"Giving {sodaModel.Name} out");
            _moneyData.Money -= sodaModel.Price;
            sodaModel.InventoryAmount--;

            Console.WriteLine("Giving " + _moneyData.Money + " out in change");
            _moneyData.Money = 0;
        }

        public void SmsOrder(string sodaName)
        {
            var sodaModel = _sodaInventoryData.Get(sodaName);
            if (sodaModel == null)
            {
                Console.WriteLine("No such soda");
                return;
            }

            if (sodaModel.InventoryAmount <= 0)
            {
                Console.WriteLine($"No {sodaModel.Name} left");
                return;
            }

            Console.WriteLine($"Giving {sodaModel.Name} out");
            sodaModel.InventoryAmount--;
        }

        public void Recall()
        {
            Console.WriteLine("Returning " + _moneyData.Money + " to customer");
            _moneyData.Money = 0;
        }

    }
}
