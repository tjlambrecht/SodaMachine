using SodaMachine.Data;
using SodaMachine.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SodaMachine.Controllers
{
    public class SodaMachineController
    {
        private readonly SodaMachineService _sodaMachineService;
        private readonly MoneyData _moneyData;

        private readonly List<string> _orderTypeList = new List<string>
        {
            "insert",
            "order",
            "sms order",
            "recall"
        };

        public SodaMachineController(SodaMachineService sodaMachineService, MoneyData moneyData)
        {
            _sodaMachineService = sodaMachineService;
            _moneyData = moneyData;
        }

        /// <summary>
        /// This is the starter method for the machine
        /// </summary>
        public void Start()
        {
            while (true)
            {
                Console.WriteLine("\n\nAvailable commands:");
                Console.WriteLine("insert (money) - Money put into money slot");
                Console.WriteLine("order (coke, sprite, fanta) - Order from machines buttons");
                Console.WriteLine("sms order (coke, sprite, fanta) - Order sent by sms");
                Console.WriteLine("recall - gives money back");
                Console.WriteLine("-------");
                Console.WriteLine("Inserted money: " + _moneyData.Money);
                Console.WriteLine("-------\n\n");

                var input = Console.ReadLine();

                var orderType = _orderTypeList.FirstOrDefault(x => input.StartsWith(x));
                var orderParameters = input.Substring(orderType.Length).Trim().Split(' ');

                switch (orderType)
                {
                    case "insert":
                        var moneyAmount = int.Parse(orderParameters[0]);
                        _sodaMachineService.InsertMoney(moneyAmount);
                        break;

                    case "order":
                        var orderSodaName = orderParameters[0];
                        _sodaMachineService.Order(orderSodaName);
                        break;

                    case "sms order":
                        var smsOrderSodaName = orderParameters[0];
                        _sodaMachineService.SmsOrder(smsOrderSodaName);
                        break;

                    case "recall":
                        _sodaMachineService.Recall();
                        break;
                }

            }
        }
    }

}
