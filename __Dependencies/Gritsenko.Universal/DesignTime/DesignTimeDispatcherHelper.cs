using System;
using System.Threading.Tasks;
using Gritsenko.Universal.Abstract;

namespace Gritsenko.Universal.DesignTime
{
    public class DesignTimeDispatcherHelper : IDispatcherHelper
    {
        public void Run(Action action)
        {
            action();
        }

        public async Task RunAsync(Action action)
        {
            action();
        }
    }
}