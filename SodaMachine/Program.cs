using Microsoft.Extensions.DependencyInjection;
using SodaMachine.Controllers;
using SodaMachine.Data;
using SodaMachine.Services;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        private static void Main(string[] args)
        {
            SetupDependencies();

            var sodaMachineController = _serviceProvider.GetService<SodaMachineController>();
            sodaMachineController.Start();
        }

        private static void SetupDependencies()
        {
            var _serviceCollection = new ServiceCollection();
            _serviceCollection.AddTransient<ISodaMachineController, SodaMachineController>();
            _serviceCollection.AddTransient<ISodaMachineService, SodaMachineService>();
            _serviceCollection.AddSingleton<MoneyData>();
            _serviceCollection.AddSingleton<SodaInventoryData>();
            _serviceProvider = _serviceCollection.BuildServiceProvider(true);
        }
    }
    
}
