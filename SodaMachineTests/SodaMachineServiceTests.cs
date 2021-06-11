using SodaMachine.Data;
using SodaMachine.Services;
using SodaMachine.Models;
using Xunit;
using System.Collections.Generic;

namespace SodaMachineTests
{
    public class SodaMachineServiceTests
    {
        #region Common
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
        #endregion

        #region InsertMoney
        [Fact]
        public void InsertMoney_GivenAnyAmount_ShouldAddMoney()
        {
            var data = Setup(100, new List<SodaModel>());

            Assert.True(data.MoneyData.Money == 100);

            data.SodaMachineService.InsertMoney(50);

            Assert.True(data.MoneyData.Money == 150);
        }

        [Fact]
        public void InsertMoney_GivenNegativeAmount_ShouldNotAddMoney()
        {
            var data = Setup(100, new List<SodaModel>());

            Assert.True(data.MoneyData.Money == 100);

            data.SodaMachineService.InsertMoney(-50);

            Assert.True(data.MoneyData.Money == 100);
        }
        #endregion

        #region Order
        [Fact]
        public void Order_GivenCorrectMoneyAndCorrectInventory_ShouldHaveZeroMoneyAndOneLessInventory()
        {
            var data = Setup(100, new List<SodaModel>() { new SodaModel { Name = "coke", Price = 20, InventoryAmount = 1 } });

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);

            data.SodaMachineService.Order("coke");

            Assert.True(data.MoneyData.Money == 0);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);
        }

        [Fact]
        public void Order_GivenIncorrectMoneyAndCorrectInventory_ShouldRejectOrder()
        {
            var data = Setup(15, new List<SodaModel>() { new SodaModel { Name = "coke", Price = 20, InventoryAmount = 1 } });

            Assert.True(data.MoneyData.Money == 15);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);

            data.SodaMachineService.Order("coke");

            Assert.True(data.MoneyData.Money == 15);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);
        }

        [Fact]
        public void Order_GivenCorrectMoneyAndIncorrectInventory_ShouldRejectOrder()
        {
            var data = Setup(100, new List<SodaModel>() { new SodaModel { Name = "coke", Price = 20, InventoryAmount = 0 } });

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);

            data.SodaMachineService.Order("coke");

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);
        }

        [Fact]
        public void Order_GivenCorrectMoneyAndCorrectInventory_ShouldOrderCorrectSoda()
        {
            var data = Setup(15, new List<SodaModel>() {
                new SodaModel { Name = "coke", Price = 20, InventoryAmount = 1 },
                new SodaModel { Name = "fanta", Price = 15, InventoryAmount = 10 } 
            });

            Assert.True(data.MoneyData.Money == 15);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);
            Assert.True(data.SodaInventoryData.SodaInventory[1].InventoryAmount == 10);

            data.SodaMachineService.Order("fanta");

            Assert.True(data.MoneyData.Money == 0);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);
            Assert.True(data.SodaInventoryData.SodaInventory[1].InventoryAmount == 9);
        }
        #endregion

        #region SmsOrder
        [Fact]
        public void SmsOrder_GivenCorrectMoneyAndCorrectInventory_ShouldReduceInventoryButNotChangeMoney()
        {
            var data = Setup(100, new List<SodaModel>() { new SodaModel { Name = "coke", Price = 20, InventoryAmount = 1 } });

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);

            data.SodaMachineService.SmsOrder("coke");

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);
        }

        [Fact]
        public void SmsOrder_GivenZeroMoneyAndCorrectInventory_ShouldReduceInventoryButNotChangeMoney()
        {
            var data = Setup(0, new List<SodaModel>() { new SodaModel { Name = "coke", Price = 20, InventoryAmount = 1 } });

            Assert.True(data.MoneyData.Money == 0);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 1);

            data.SodaMachineService.SmsOrder("coke");

            Assert.True(data.MoneyData.Money == 0);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);
        }

        [Fact]
        public void SmsOrder_GivenIncorrectInventory_ShouldRejectOrderButNotReturnMoney()
        {
            var data = Setup(100, new List<SodaModel>() { new SodaModel { Name = "coke", Price = 20, InventoryAmount = 0 } });

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);

            data.SodaMachineService.SmsOrder("coke");

            Assert.True(data.MoneyData.Money == 100);
            Assert.True(data.SodaInventoryData.SodaInventory[0].InventoryAmount == 0);
        }
        #endregion

        #region Recall
        [Fact]
        public void Recall_GivenAnyMoney_ShouldHaveZeroMoney()
        {
            var data = Setup(100, new List<SodaModel>());

            Assert.True(data.MoneyData.Money == 100);

            data.SodaMachineService.Recall();

            Assert.True(data.MoneyData.Money == 0);
        }
        #endregion

    }
}
