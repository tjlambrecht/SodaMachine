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
            _moneyData.Money += amount;
            Console.WriteLine("Adding " + amount + " to credit");
        }

        public void Order(string sodaName)
        {
            // split string on space
            var sodaModel = _sodaInventoryData.Get(sodaName);

            if (sodaModel != null)
            {
                if (_moneyData.Money >= sodaModel.Price && sodaModel.InventoryAmount > 0)
                {
                    Console.WriteLine($"Giving {sodaModel.Name} out");
                    _moneyData.Money -= sodaModel.Price;
                    Console.WriteLine("Giving " + _moneyData.Money + " out in change");
                    _moneyData.Money = 0;
                    sodaModel.InventoryAmount--;
                }
                else if (sodaModel.Name == sodaName && sodaModel.InventoryAmount == 0)
                {
                    Console.WriteLine($"No {sodaModel.Name} left");
                }
                else if (sodaModel.Name == sodaName)
                {
                    Console.WriteLine("Need " + (sodaModel.Price - _moneyData.Money) + " more");
                }
            }
            else
            {
                Console.WriteLine("No such soda");
            }
        }

        public void SmsOrder(string sodaName)
        {
            var sodaModel = _sodaInventoryData.Get(sodaName);

            if (sodaModel != null)
            {
                if (sodaModel.InventoryAmount > 0)
                {
                    Console.WriteLine($"Giving {sodaModel.Name} out");
                    sodaModel.InventoryAmount--;
                }
            }
            else
            {
                Console.WriteLine("No such soda");
            }
        }

        public void Recall()
        {
            Console.WriteLine("Returning " + _moneyData.Money + " to customer");
            _moneyData.Money = 0;
        }

    }
}
