using SodaMachine.Data;
using SodaMachine.Services;
using SodaMachine.Models;
using System;
using Xunit;
using System.Collections.Generic;

namespace SodaMachineTests
{
    public class SodaMachineServiceTests
    {
        private class SodaMachineServiceTestsData
        {
            public MoneyData MoneyData { get; set; }
            public SodaInventoryData SodaInventoryData { get; set; }
            public SodaMachineService SodaMachineService { get; set; }
        }

        private SodaMachineServiceTestsData Setup(int money, List<SodaModel> sodaInventory)
        {
            var moneyData = new MoneyData() { Money = money };
            var sodaInventoryData = new SodaInventoryData() { SodaInventory = sodaInventory };
            var sodaMachineService = new SodaMachineService(moneyData, sodaInventoryData);

            var data = new SodaMachineServiceTestsData()
            {
                MoneyData = moneyData,
                SodaInventoryData = sodaInventoryData,
                SodaMachineService = sodaMachineService
            };

            return data;
        }

        [Fact]
        public void InsertMoney_GivenAnyAmount_ShouldAddMoney()
        {
            var data = Setup(0, new List<SodaModel>());

            Assert.True(data.MoneyData.Money == 0);

            data.SodaMachineService.InsertMoney(100);

            Assert.True(data.MoneyData.Money == 100);
        }

        [Fact]
        public void Order_GivenCorrectMoneyAndInventory_ShouldHaveZeroMoneyAndOneLessInventory()
        {
            var data = Setup(100, new List<SodaModel>() { new SodaModel { Name = "coke", Price = 20, InventoryAmount = 1 } });

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);

            data.SodaMachineService.Order("coke");

            Assert.True(data.MoneyData.Money == 0);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);
        }

        [Fact]
        public void Recall_GivenAnyMoney_ShouldHaveZeroMoney()
        {
            var data = Setup(100, new List<SodaModel>());

            Assert.True(data.MoneyData.Money == 100);

            data.SodaMachineService.Recall();

            Assert.True(data.MoneyData.Money == 0);
        }

    }
}
