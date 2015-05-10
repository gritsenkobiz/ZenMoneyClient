using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Gritsenko.Universal.Abstract
{
    public interface IDispatcherHelper
    {
        void Run(Action action);
        Task RunAsync(Action action);
    }
}