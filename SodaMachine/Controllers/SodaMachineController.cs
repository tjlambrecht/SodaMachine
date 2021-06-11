using SodaMachine.Data;
using SodaMachine.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SodaMachine.Controllers
{
    public class SodaMachineController : ISodaMachineController
    {
        private readonly ISodaMachineService _sodaMachineService;
        private readonly MoneyData _moneyData;

        private readonly List<string> _vendingFunctionList = new List<string>
        {
            "insert",
            "order",
            "sms order",
            "recall"
        };

        public SodaMachineController(ISodaMachineService sodaMachineService, MoneyData moneyData)
        {
            _sodaMachineService = sodaMachineService;
            _moneyData = moneyData;
        }

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

                var vendingFunction = _vendingFunctionList.FirstOrDefault(x => input.StartsWith(x));
                var vendingFunctionParameters = input.Substring(vendingFunction.Length).Trim().Split(' ');

                switch (vendingFunction)
                {
                    case "insert":
                        var moneyAmount = int.Parse(vendingFunctionParameters[0]);
                        _sodaMachineService.InsertMoney(moneyAmount);
                        break;

                    case "order":
                        var orderSodaName = vendingFunctionParameters[0];
                        _sodaMachineService.Order(orderSodaName);
                        break;

                    case "sms order":
                        var smsOrderSodaName = vendingFunctionParameters[0];
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
