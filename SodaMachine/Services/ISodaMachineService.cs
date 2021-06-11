using System;
using System.Collections.Generic;
using System.Text;

namespace SodaMachine.Services
{
    public interface ISodaMachineService
    {
        void InsertMoney(int amount);

        void Order(string sodaName);

        void SmsOrder(string sodaName);

        void Recall();
    }
}
